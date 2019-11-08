using Common.Standard.Context;
using Common.Standard.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rest.Core.Controllers
{
    public class RegistrationController : ApiController
    {
        public IEnumerable<RegistrationModel> Get(int? id = null)
        {
            using (var dbContext = new DatabaseContext())
            {
                var listEntities = dbContext.Registrations.Include(x => x.RegistrationValues).Where(r => r.MenuItemId == id).ToList();
                return listEntities;
            }
        }

        public HttpResponseMessage Post([FromBody] RegistrationModel registrationModel)
        {
            try
            {
                using (var dbContext = new DatabaseContext())
                {
                    dbContext.Registrations.Add(registrationModel);
                    dbContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, registrationModel);
                    message.Headers.Location = new Uri(Request.RequestUri + registrationModel.Id.ToString());

                    return message;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
