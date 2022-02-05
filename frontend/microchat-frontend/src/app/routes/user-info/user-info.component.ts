import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { LogService } from 'src/app/services/log.service';
import { UserService } from 'src/app/services/user.service';
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
  
  email: string = '';
  name: string = '';
  surname: string = '';
  username: string = '';
  visibility: Visibility = {
    email: false,
    name: false,
    surname: false,
    username: false,
  }


  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private logService: LogService,
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

  delete() {
    this.accountService.deleteUser().subscribe(_ => {
      this.logService.messageSnackBar("account deleted");
      this.dialogRef.close();
    });
  }

  update() {
    if (this.email !== this.emailInitValue()) {
      this.accountService.updateEmail(this.email);
    }
    if (this.name !== this.nameInitValue()) {
      this.userService.updateName(this.name);
    }
    if (this.surname !== this.surnameInitValue()) {
      this.userService.updateSurname(this.surname);
    }
    if (this.username !== this.usernameInitValue()) {
      this.accountService.updateUsername(this.username);
    }
    this.dialogRef.close();
  }

  emailInitValue(): string | undefined {
    return this.user?.email
  }

  nameInitValue(): string | undefined {
    return this.userInfo?.name
  }

  surnameInitValue(): string | undefined {
    return this.userInfo?.surname
  }

  usernameInitValue(): string | undefined {
    return this.userInfo?.username || this.user?.username
  }

}
