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
    public class OrdersItemsController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/OrdersItems
        public IQueryable<OrdersItems> GetOrdersItems()
        {
            return db.OrdersItems;
        }

        // GET: api/OrdersItems/5
        [ResponseType(typeof(OrdersItems))]
        public IHttpActionResult GetOrdersItems(long id)
        {
            OrdersItems ordersItems = db.OrdersItems.Find(id);
            if (ordersItems == null)
            {
                return NotFound();
            }

            return Ok(ordersItems);
        }

        // PUT: api/OrdersItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrdersItems(long id, OrdersItems ordersItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ordersItems.OrderItemID)
            {
                return BadRequest();
            }

            db.Entry(ordersItems).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/OrdersItems
        [ResponseType(typeof(OrdersItems))]
        public IHttpActionResult PostOrdersItems(OrdersItems ordersItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrdersItems.Add(ordersItems);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ordersItems.OrderItemID }, ordersItems);
        }

        // DELETE: api/OrdersItems/5
        [ResponseType(typeof(OrdersItems))]
        public IHttpActionResult DeleteOrdersItems(long id)
        {
            OrdersItems ordersItems = db.OrdersItems.Find(id);
            if (ordersItems == null)
            {
                return NotFound();
            }

            db.OrdersItems.Remove(ordersItems);
            db.SaveChanges();

            return Ok(ordersItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdersItemsExists(long id)
        {
            return db.OrdersItems.Count(e => e.OrderItemID == id) > 0;
        }
    }
}