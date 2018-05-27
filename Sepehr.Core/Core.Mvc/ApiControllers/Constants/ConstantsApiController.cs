using Core.Rep.DTO;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
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
