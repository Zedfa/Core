using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace Core.Mvc.Controllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CacheInfoController : ControllerBaseCr
    {

        public ActionResult Index()
        {
            return View();
        }
        // GET: api/CacheInfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CacheInfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CacheInfo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CacheInfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CacheInfo/5
        public void Delete(int id)
        {
        }
    }
}
