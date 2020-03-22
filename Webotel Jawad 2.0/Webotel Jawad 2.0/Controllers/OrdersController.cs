using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Webotel_Jawad_2._0.Models;

namespace Webotel_Jawad_2._0.Controllers
{
    [EnableCors("http://localhost:4200", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Orders
        public System.Object GetOrders()
        {
            // on va utiliser LinQ pour chercher le name du customer et d'autres info de la table 
            var result = (from a in db.Orders
                          join b in db.Customers on a.CustomerID equals b.CustomerID

                          select new
                          {
                              a.OrderID,
                              a.OrderNo,
                              Customer=b.Name,
                              a.PMethod,
                              a.GTotal
                          }).ToList();
            return result;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult GetOrders(long id) // page de gestion vers page principal
        {
            
               var order=(from a in db.Orders
                         where a.OrderID == id

                         select new
                         {
                             a.OrderID,
                             a.OrderNo,
                             a.CustomerID,
                             a.PMethod,
                             a.GTotal,
                             DeletedOrderItemIDs=""
                         }).FirstOrDefault();

            var orderDetails=(from a in db.OrdersItems
                                join b in db.Services on a.itemID equals b.ItemID
                                where a.OrderID == id

                                select new
                                {
                                    a.OrderID,
                                    a.OrderItemID,
                                    a.itemID,
                                    ItemName = b.Name,
                                    b.Price,
                                    a.Quantity,
                                    Total = a.Quantity * b.Price
                                }).ToList();
            return Ok(new { order, orderDetails });
        }

        

        // POST: api/Orders
        [ResponseType(typeof(Orders))]
        public IHttpActionResult PostOrders(Orders orders)
        {  // on ajoute a la table Order
            try
            {
                if (orders.OrderID == 0)
                    db.Orders.Add(orders);
                else
                    db.Entry(orders).State = EntityState.Modified;
                foreach (var item in orders.OrdersItems) // la table OrderItems 
                {
                    if (item.OrderItemID == 0)
                        db.OrdersItems.Add(item);
                    else
                        db.Entry(item).State = EntityState.Modified;
                }
            }
            catch (Exception)
            {
                throw;
            }
            // on va delete pour OrderItems
            foreach (var id in orders.DeletedOrderItemIDs.Split(',').Where(x => x!=""))
            {
                OrdersItems x = db.OrdersItems.Find(Convert.ToInt64(id));
                db.OrdersItems.Remove(x);
            }
            db.SaveChanges();
            
            return Ok();
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult DeleteOrders(long id)
        {
            Orders orders = db.Orders.Include(y => y.OrdersItems)
                .SingleOrDefault(x => x.OrderID == id);
            foreach (var item in orders.OrdersItems.ToList())
            {
                db.OrdersItems.Remove(item);
            }
            db.Orders.Remove(orders);
            db.SaveChanges();

            return Ok(orders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdersExists(long id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}