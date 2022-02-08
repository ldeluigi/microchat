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
  
  email: string | undefined;
  name: string | undefined;
  surname: string | undefined;
  username: string | undefined;
  oldPass: string | undefined;
  newPass: string | undefined;
  hide = true;
  hideOld = true;
  visibility: Visibility = {
    email: false,
    name: false,
    surname: false,
    username: false,
  }


  constructor(
    private userService: UserService,
    private accountService: AccountService,
    public dialogRef: MatDialogRef<UserInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {id: string}) { }

  ngOnInit(): void {
    this.userService.userInfo(this.data.id).subscribe(info => {
      this.userInfo = info;
      this.name = this.nameInitValue();
      this.surname = this.surnameInitValue();
      this.username = this.usernameInitValue();
    });
    this.accountService.getInfo(this.data.id).subscribe(authInfo => {
      this.user = authInfo;
      this.username = this.usernameInitValue();
      this.email = this.emailInitValue();
    });
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
      this.dialogRef.close();
      this.accountService.logout("account deleted");
    });
  }

  update() {
    if (this.email && this.email !== this.emailInitValue()) {
      this.accountService.updateEmail(this.email);
    }
    if (this.name && this.name !== this.nameInitValue()) {
      this.userService.updateName(this.data.id, this.name);
    }
    if (this.surname && this.surname !== this.surnameInitValue()) {
      this.userService.updateSurname(this.data.id, this.surname);
    }
    if (this.username && this.username !== this.usernameInitValue()) {
      this.accountService.updateUsername(this.username);
    }
    if (this.oldPass && this.newPass) {
      this.accountService.updatePassword(this.oldPass, this.newPass);
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
