import {inject, NgModule} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  createUrlTreeFromSnapshot,
  RouterModule,
  RouterStateSnapshot,
  Routes,
  UrlTree
} from '@angular/router';
import {catchError, map, Observable, of, tap} from "rxjs";
import {CurrentUserService} from "./currentUserService";

function isLoggedIn(route: ActivatedRouteSnapshot, _: RouterStateSnapshot): Observable<boolean | UrlTree> {
  const currentUser: CurrentUserService = inject(CurrentUserService);
  return currentUser
    .getCurrentUser$()
    .pipe(
      map(_ => true),
      catchError(_ => {
        const url = createUrlTreeFromSnapshot(route, ['/login']);
        return of(url);
      }),
    );
}

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./equipments/equipments.component')
        .then(m => m.EquipmentsComponent)
    , canActivate: [isLoggedIn],
  },
  {
    path: 'equipment/:id',
    loadComponent: () =>
      import('./equipment/equipment.component').then(m => m.EquipmentComponent),
    canActivate: [isLoggedIn]
  },
  {
    path: 'equipment/:**', loadComponent: () =>
      import('./not-found/not-found.component').then(m => m.NotFoundComponent)
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./login/login.component').then(m => m.LoginComponent)
  },
  {
    path: '**', loadComponent: () =>
      import('./not-found/not-found.component').then(m => m.NotFoundComponent)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRouting {
}
