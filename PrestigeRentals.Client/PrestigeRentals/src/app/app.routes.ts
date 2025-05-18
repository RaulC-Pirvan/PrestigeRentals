import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ForbiddenAccessComponent } from './pages/forbidden-access/forbidden-access.component';
import { ContactComponent } from './pages/contact/contact.component';
import { LoginComponent } from './pages/login/login.component';
import { AuthRedirectGuard } from './guards/auth-redirect.guard';
import { SettingsComponent } from './pages/settings/settings.component';
import { DetailsSettingsFormComponent } from './pages/settings/components/details-settings-form/details-settings-form.component';

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
        path:'details',
        component: DetailsSettingsFormComponent
      },
    ]
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
];
