import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { OrderItem } from 'src/app/shared/order-item.model';
import { ItemService } from 'src/app/shared/item.service';
import { Item } from 'src/app/shared/item.model';
import { NgForm } from '@angular/forms';
import { OrderService } from 'src/app/shared/order.service';


@Component({
  selector: 'app-order-items',
  templateUrl: './order-items.component.html',
  styles: []
})
export class OrderItemsComponent implements OnInit {
  formData:OrderItem;
  itemList:Item[];
  isValid:boolean = true;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef:MatDialogRef<OrderItemsComponent>,
    private itemService:ItemService,
    private orderService:OrderService) { }

  ngOnInit() { // la ligne d'en bas sert a getter les jsons des services , cf environnement + serviceItem
  this.itemService.getItemList().then(res => this.itemList = res as Item[]);

  if(this.data.orderItemIndex == null)
    this.formData ={
        OrderItemid:null,
        OrderId : this.data.OrderId,
        itemID:0,
        ItemName:'',
        Prix:0,
        Quantity:0,
        Total:0
      };
      else
      this.formData =Object.assign({},this.orderService.orderItems[this.data.orderItemIndex]);
  }
  updatePrice(ctrl){
    if(ctrl.selectedIndex==0){ // si on a rien choisi comme service
      this.formData.Prix=0;
      this.formData.ItemName='';
    }
    else { // on chope son prix dans la bdd
      this.formData.Prix=this.itemList[ctrl.selectedIndex-1].Price;
      this.formData.ItemName=this.itemList[ctrl.selectedIndex-1].Name;
    }
    this.updateTotal();
  }
  updateTotal(){
    this.formData.Total = parseFloat((this.formData.Quantity * this.formData.Prix).toFixed(2));
  }
  onSubmit(form:NgForm){
    // check la méthode juste en bas pour gérer les failles
    if(this.validateForm(form.value)){
        if(this.data.orderItemIndex == null) // si on souhaite edit et resubmit
          // on stocke comme si c'était un objet
          this.orderService.orderItems.push(form.value);
        else
          this.orderService.orderItems[this.data.orderItemIndex] = form.value; // pour resubmit
          this.dialogRef.close(); // on ferme la fenetre
    }
  }
  validateForm(formData:OrderItem){
    this.isValid =true;
    if(formData.itemID==0)
      this.isValid =false;
    else if(formData.Quantity==0) // gestion des nullables
      this.isValid =false;
    return this.isValid;
  }


}
