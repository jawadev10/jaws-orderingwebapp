import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private http:HttpClient) { }
  // pour cherche dans les Api ce qu'on veut faire, ici le get des clients 
 
  getCustomerList(){
      return this.http.get(environment.apiURL+'/Customers').toPromise();
   }

}
