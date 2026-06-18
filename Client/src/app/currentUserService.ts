import {BehaviorSubject, Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {ApiService, UserDto} from "./api.service";

@Injectable({
  providedIn: 'root'
})

export class CurrentUserService {
  private _user$: BehaviorSubject<UserDto | null> = new BehaviorSubject<UserDto | null>(null);
  currentUser$ = this._user$.asObservable();

  constructor(private api: ApiService) {}

  setCurrentUser(user: UserDto | null) {
    this._user$.next(user);
  }

  getCurrentUser$(): Observable<UserDto> {
    return this.api.me();
  }
}
