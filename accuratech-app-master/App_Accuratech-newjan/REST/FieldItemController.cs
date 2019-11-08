using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
using Common;
using Common_Backend.Context;

namespace REST
{
    public class FieldItemController : ApiController
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext();
        public IEnumerable<SubItemEntityModel> Get(string name = "All")
        {
            return _dbContext.SubMenus.ToList().AsEnumerable();
        }


        public HttpResponseMessage Get(int id)
        {
            var subItemDetail = (from a in _dbContext.SubMenus where a.Id == id select a).FirstOrDefault();

            if (subItemDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, subItemDetail);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or SubItem Not Found");
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post([FromBody] SubItemEntityModel subItemEntityModel)
        {
            try
            {
                _dbContext.SubMenus.Add(subItemEntityModel);
                _dbContext.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, subItemEntityModel);
                message.Headers.Location = new Uri(Request.RequestUri + subItemEntityModel.Id.ToString());

                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        private bool SubItemExists(int id)
        {
            return _dbContext.SubMenus.Count(s => s.Id == id) > 0;
        }

        [System.Web.Http.HttpPut]
        public IHttpActionResult Put(int id, SubItemEntityModel subItemEntityModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subItemEntityModel.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(subItemEntityModel).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubItemExists(id))
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

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (_dbContext)
                {
                    var entity = _dbContext.SubMenus.FirstOrDefault(s => s.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Sub Item with ID = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        _dbContext.SubMenus.Remove(entity);
                        _dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
