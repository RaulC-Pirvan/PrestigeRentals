import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ForbiddenAccessComponent } from './pages/forbidden-access/forbidden-access.component';

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
    path: '**',
    redirectTo: 'not-found',
  },
];
