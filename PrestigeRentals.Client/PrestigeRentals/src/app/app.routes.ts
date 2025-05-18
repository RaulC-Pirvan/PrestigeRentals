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
    path: 'contact',
    component: ContactComponent,
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
    path: '**',
    redirectTo: 'not-found',
  },
];
