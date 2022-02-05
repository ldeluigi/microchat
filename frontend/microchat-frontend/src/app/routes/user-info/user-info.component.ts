import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/model/Chat';
import { AuthUserInfo } from 'src/model/LoggedUser';
import { UserInfo } from 'src/model/UserInfo';

interface Visibility {
  email: boolean,
  name: boolean,
  surname: boolean,
  username: boolean
}

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss']
})
export class UserInfoComponent implements OnInit {


  userInfo: UserInfo | undefined;
  user: AuthUserInfo | undefined;
  visibility: Visibility = {
    email: true,
    name: true,
    surname: true,
    username: true,
  }


  constructor(
    private userService: UserService,
    private accountService: AccountService,
    public dialogRef: MatDialogRef<UserInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {id: string}) { }

  ngOnInit(): void {
    this.userService.usersInfo(this.data.id).subscribe(info => this.userInfo = info);
    this.accountService.getInfo(this.data.id).subscribe(authInfo => this.user = authInfo);
  }

  modifiable(): string {
    return this.data.id === this.accountService.userValue?.userId ? "show_it" : "hide_it"
  }

  changeVisibility(elem: keyof Visibility) {
    console.log(elem, this.visibility[elem])
    this.visibility[elem] = !this.visibility[elem];
    console.log(elem, this.visibility[elem])
  }

  showElem(elem: keyof Visibility, modifier: boolean) {
    return this.visibility[elem] !== !modifier ? "show_it" : "hide_it";
  }

}
