import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ForbiddenAccessComponent } from './pages/forbidden-access/forbidden-access.component';
import { ContactComponent } from './pages/contact/contact.component';
import { LoginComponent } from './pages/login/login.component';
import { AuthRedirectGuard } from './guards/auth-redirect.guard';
import { SettingsComponent } from './pages/settings/settings.component';
import { DetailsSettingsFormComponent } from './pages/settings/components/details-settings-form/details-settings-form.component';
import { EmailSettingsFormComponent } from './pages/settings/components/email-settings-form/email-settings-form.component';
import { PasswordSettingsFormComponent } from './pages/settings/components/password-settings-form/password-settings-form.component';
import { DeactivateAccountFormComponent } from './pages/settings/components/deactivate-account-form/deactivate-account-form.component';
import { DeleteAccountFormComponent } from './pages/settings/components/delete-account-form/delete-account-form.component';
import { AboutUsComponent } from './pages/about-us/about-us.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { InventoryComponent } from './pages/inventory/inventory.component';
import { VehicleDetailComponent } from './pages/vehicle-detail/vehicle-detail.component';
import { OrderCheckoutComponent } from './pages/order-checkout/order-checkout.component';
import { OrderConfirmationComponent } from './pages/order-confirmation/order-confirmation.component';
import { AdminComponent } from './pages/admin/admin.component';
import { OrdersComponent } from './pages/admin/orders/orders.component';
import { ReviewsComponent } from './pages/admin/reviews/reviews.component';
import { TicketsComponent } from './pages/admin/tickets/tickets.component';
import { UsersComponent } from './pages/admin/users/users.component';
import { VehiclesComponent } from './pages/admin/vehicles/vehicles.component';
import { AdminGuard } from './guards/admin.guard';

export const routes: Routes = [
  {
    path: '',
    component: LandingPageComponent,
  },
  {
    path: 'not-found',
    component: NotFoundComponent,
  },
  {
    path: 'forbidden',
    component: ForbiddenAccessComponent,
  },
  {
    path: 'inventory',
    component: InventoryComponent
  },
  {
    path: 'vehicle/:id',
    component: VehicleDetailComponent
  },
  {
    path: 'contact',
    component: ContactComponent,
  },
  {
    path: 'order-checkout',
    component: OrderCheckoutComponent
  },
  {
    path: 'order-confirmation',
    component: OrderConfirmationComponent
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [AuthRedirectGuard]
  },
  {
    path: 'register',
    component: LoginComponent,
    canActivate: [AuthRedirectGuard]
  },
  {
    path: 'settings',
    component: SettingsComponent,
    children: [
      {
        path:'',
        pathMatch: 'full',
        redirectTo: 'details'
      },
      {
        path:'details',
        component: DetailsSettingsFormComponent
      },
      {
        path:'email',
        component: EmailSettingsFormComponent
      },
      {
        path:'password',
        component: PasswordSettingsFormComponent
      },
      {
        path:'deactivate',
        component: DeactivateAccountFormComponent
      },
      {
        path:'delete',
        component: DeleteAccountFormComponent
      },
    ]
  },
    {
    path: 'admin-dashboard',
    component: AdminComponent,
    children: [
      { path: '', redirectTo: 'orders', pathMatch: 'full' },
      { path: 'orders', component: OrdersComponent },
      { path: 'reviews', component: ReviewsComponent },
      { path: 'tickets', component: TicketsComponent },
      { path: 'users', component: UsersComponent },
      { path: 'vehicles', component: VehiclesComponent },
    ],
  },
  {
    path: 'about-us',
    component: AboutUsComponent
  },
  {
    path: 'profile',
    component: ProfileComponent
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
];
