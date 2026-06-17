import {inject, NgModule} from '@angular/core';
import {ActivatedRouteSnapshot, createUrlTreeFromSnapshot, RouterModule, RouterStateSnapshot, Routes, UrlTree} from '@angular/router';
import {NotFoundComponent} from "./not-found/not-found.component";
import {LoginComponent} from "./login/login.component";
import {ApiService} from "./api.service";
import {catchError, map, Observable, of, tap} from "rxjs";

function isLoggedIn(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
  const apiService: ApiService = inject(ApiService);
  return apiService
    .me()
    .pipe(
        map(_ => true),
        catchError(e => {
          const url = createUrlTreeFromSnapshot(route, ['/login']);
          return of(url);
        }),
      );
}

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
