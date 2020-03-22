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
    public class ServicesController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Services
        public IQueryable<Services> GetServices()
        {    // On a juste besoin de getter tout les services sans plus
            return db.Services;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}