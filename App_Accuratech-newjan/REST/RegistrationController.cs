using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common_Backend.Context;

namespace REST
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
