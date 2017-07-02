using Core.Rep.DTO;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Core.Mvc.ApiControllers
{
    public class ConstantsAPiController : Controller.ApiControllerBase
    {
        private IConstantService _constantService;
        public ConstantsAPiController(IConstantService constantService)
        {
            _constantService = constantService;

        }

        public List<ConstantDTO> GetConstantByNameOfCategory(string category)
        {
            var result = _constantService.GetConstantByNameOfCategory(category, false).Select(constant => new ConstantDTO(constant)).ToList();
            if (category.ToLower().Equals("resources"))
            {
                result.Add(new ConstantDTO { Key = "ShamsiDateTimeNow", Value = Core.Cmn.FarsiUtils.PersianDate.Now.ToString() });
            }
            return result;
        }

    }
}
