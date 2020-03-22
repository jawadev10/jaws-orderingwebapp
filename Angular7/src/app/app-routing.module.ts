import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './orders/orders.component';
import { OrderComponent } from './orders/order/order.component';

//chemin de l'application web Routing
const routes: Routes = [
  {path:'',redirectTo:'order',pathMatch:'full'}, // par défaut
  {path:'orders',component:OrdersComponent},
  {path:'order',children:[
    {path:'',component:OrderComponent},
    {path:'edit/:id',component:OrderComponent}
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
