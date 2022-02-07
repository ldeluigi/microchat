import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { Observable } from 'rxjs';
import { SignalRService } from 'src/app/services/signal-r.service';
import { Message } from 'src/model/Message';
import { ChatComponent } from '../chat/chat.component';
import { ContactComponent } from '../contact/contact.component';

import { HomeComponent } from './home.component';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockSignalR;

  beforeEach(async () => {
    mockSignalR = jasmine.createSpyObj(['connect', 'newMessage', 'editedMessage', 'deletedMessage', 'disconnect']);
    mockSignalR.newMessage.and.returnValue(new Observable<Message>());
    mockSignalR.editedMessage.and.returnValue(new Observable<Message>());
    mockSignalR.deletedMessage.and.returnValue(new Observable<string>());
    await TestBed.configureTestingModule({
      imports: [
        MatDialogModule,
        RouterTestingModule,
        HttpClientTestingModule,
        MatDialogModule,
        MatSnackBarModule
      ],
      declarations: [
        HomeComponent,
        ChatComponent,
        ContactComponent
      ],
      providers: [{provide: SignalRService, useValue: mockSignalR}]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  afterEach(() => fixture.destroy());
});