
PrestigeRentals - Restaurare baza de date PostgreSQL
-----------------------------------------------------

Acest fișier descrie pașii necesari pentru a restaura baza de date utilizată în proiectul PrestigeRentals,
folosind pgAdmin 4.

----------------------------
1. DESCHIDERE PGADMIN
----------------------------
- Lansează aplicația pgAdmin 4
- Conectează-te la serverul tău PostgreSQL (ex: PostgreSQL 17)

----------------------------
2. CREARE BAZĂ DE DATE NOUĂ
----------------------------
- Click dreapta pe "Databases" > "Create" > "Database..."
- Numește baza de date, ex: `prestigerentals`
- Click pe "Save"

----------------------------
3. RESTAURARE DIN BACKUP
----------------------------
- Click dreapta pe baza de date creată (`prestigerentals`) > "Restore..."
- La "Filename", selectează fișierul `prestigerentals.backup` din arhiva proiectului (folderul `db/`)
- La "Format", selectează `Custom` (dacă nu e deja)
- Click pe "Restore"

----------------------------
4. VERIFICARE
----------------------------
- După finalizarea restaurării, extinde baza de date și verifică dacă apar toate tabelele:
  (Users, Vehicles, Orders, Reviews etc.)

----------------------------
5. PROBLEME FRECVENTE
----------------------------
- Dacă apare eroare la restaurare: verifică versiunea PostgreSQL (ideal aceeași ca la export)
- Asigură-te că fișierul `.backup` nu a fost corupt/copiat parțial
- Dacă ai un fișier `.sql`, poți folosi și opțiunea "Query Tool" pentru rularea manuală

----------------------------
Succes!
----------------------------
