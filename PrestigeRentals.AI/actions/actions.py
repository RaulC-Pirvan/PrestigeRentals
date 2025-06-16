import requests
from typing import Any, Text, Dict, List
from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher
import random
import re

class ActionGetAvailableVehicles(Action):
    def name(self) -> Text:
        return "action_ask_available_vehicles"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

        print("🚨 [INFO] Fetching all available vehicles...")

        try:
            response = requests.get("https://localhost:7093/api/vehicle?onlyActive=true", verify=False)
            vehicles = response.json()
            random.shuffle(vehicles)

            if not vehicles:
                dispatcher.utter_message(text="Sorry, no vehicles are currently available.")
                return []

            message = "Here are some available vehicles:\n"
            for v in vehicles[:5]:  # max 5 sugestii
                link = f"/vehicle/{v['id']}"
                name = f"{v['make']} {v['model']} ({v['chassis']})"
                message += f"<li><span class='vehicle-link' data-link='{link}'>{name}</span></li>"

            message += "</ul>"

            dispatcher.utter_message(text=message)

        except Exception as e:
            print(f"[ERROR] Failed to fetch vehicles: {e}")
            dispatcher.utter_message(text=f"Failed to fetch vehicles: {e}")

        return []

class ActionRecommendByFeature(Action):
    def name(self) -> Text:
        return "action_recommend_by_feature"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

        print("🚨 [INFO] Feature-based recommendation requested.")

        user_msg = tracker.latest_message.get("text", "").lower()

        feature_keywords = {
            "hill assist": "hillAssist",
            "cruise control": "cruiseControl",
            "navigation": "navigation",
            "heads up display": "headsUpDisplay"
        }

        matched_features = [
            api_field for phrase, api_field in feature_keywords.items()
            if phrase in user_msg
        ]

        if not matched_features:
            dispatcher.utter_message(text="Please specify a feature like hill assist, navigation, cruise control or heads up display.")
            return []

        try:
            vehicle_res = requests.get("https://localhost:7093/api/vehicle?onlyActive=true", verify=False)
            print(f"[DEBUG] Vehicle fetch status: {vehicle_res.status_code}")
            print(f"[DEBUG] Vehicle fetch body:\n{vehicle_res.text}")

            try:
                vehicles = vehicle_res.json()
                random.shuffle(vehicles)
            except Exception as e:
                print(f"[ERROR] Failed to parse JSON from /vehicles: {e}")
                dispatcher.utter_message(text="Error parsing vehicle list.")
                return []

            matching = []

            for v in vehicles:
                vehicle_id = v.get("id")
                if not vehicle_id:
                    continue

                options_url = f"https://localhost:7093/api/vehicle/{vehicle_id}/options"
                print(f"[DEBUG] Fetching options for vehicleId={vehicle_id}: {options_url}")

                try:
                    options_res = requests.get(options_url, verify=False)
                    print(f"[DEBUG] Status: {options_res.status_code}")

                    if options_res.status_code != 200:
                        print(f"[WARN] No options data for vehicle {vehicle_id}")
                        continue

                    options = options_res.json()
                    if all(options.get(f, False) for f in matched_features):
                        print(f"[MATCH] {v['make']} {v['model']} matched")
                        matching.append(v)
                        random.shuffle(matching)

                except Exception as e:
                    print(f"[ERROR] Fetch failed for vehicleId={vehicle_id}: {e}")
                    continue

            if not matching:
                dispatcher.utter_message(text="No vehicles match those features at the moment.")
                return []

            response = "Here are some cars with those features:\n"
            for v in matching[:5]:
                link = f"http://localhost:4200/vehicle/{v['id']}"
                response += f"- [{v['make']} {v['model']}]({link}): {v['pricePerDay']}€/day\n"

            dispatcher.utter_message(text=response)

        except Exception as e:
            print(f"[ERROR] Unexpected failure in feature search: {e}")
            dispatcher.utter_message(text=f"Error getting vehicles: {e}")

        return []

class ActionFilterBySpecs(Action):
    def name(self) -> Text:
        return "action_filter_by_specs"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

        user_msg = tracker.latest_message.get("text", "").lower()
        print(f"🔍 User message: {user_msg}")

        try:
            response = requests.get("https://localhost:7093/api/vehicle?onlyActive=true", verify=False)
            vehicles = response.json()
            random.shuffle(vehicles)
        except Exception as e:
            dispatcher.utter_message(text=f"Couldn't load vehicles: {e}")
            return []

        filtered = []

        if "automatic" in user_msg:
            filtered = [v for v in vehicles if v.get("transmission", "").lower() == "automatic"]
        elif "manual" in user_msg:
            filtered = [v for v in vehicles if v.get("transmission", "").lower() == "manual"]
        elif any(t in user_msg for t in ["suv", "sedan", "limo", "pickup"]):
            for v in vehicles:
                if v.get("chassis", "").lower() in user_msg:
                    filtered.append(v)
        else:
            match = re.search(r"(\d+)\s*(hp|horsepower)", user_msg)
            if match:
                hp_value = int(match.group(1))
                filtered = [v for v in vehicles if v.get("horsepower", 0) >= hp_value]

        if not filtered:
            dispatcher.utter_message(text="No vehicles matched those specifications.")
            return []

        response = "Here are some vehicles:\n"
        for v in filtered[:5]:
            link = f"http://localhost:4200/vehicle/{v['id']}"
            response += f"- [{v['make']} {v['model']}]({link}): {v['horsepower']}hp, {v['transmission']}, {v['chassis']}\n"

        dispatcher.utter_message(text=response)
        return []
