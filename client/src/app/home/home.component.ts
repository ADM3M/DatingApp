import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service'; 

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users : any;

  constructor(private http: HttpClient, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegister(event : boolean)
  {
    this.registerMode = event;
  }

  checkIfUserLoggedIn() {
    return this.accountService.checkIfUserLoggedIn();
  }
}
