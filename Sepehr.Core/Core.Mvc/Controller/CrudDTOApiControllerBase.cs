using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Core.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Core.Entity;
using Core.Cmn;
using Core.Cmn.EntityBase;
using Core.Ef.Exceptions;

namespace Core.Mvc.Controller
{
    public class CrudDTOApiControllerBase<EntityT, DTOT, ServiceT> : Core.Mvc.Controller.ApiControllerBase
        where EntityT : EntityBase<EntityT>, new()
        where DTOT : DtoBase<EntityT>, new()
        where ServiceT : IServiceBase<EntityT>
    {
        private ServiceT _service;

        public ServiceT Service
        {
            get { return _service; }
        }
        public CrudDTOApiControllerBase(ServiceT service)
        {
            _service = service;
        }

        public virtual DataSourceResult GetEntities([ModelBinder(typeof(ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var entities = _service.All();
            return entities.ToDataSourceResult(request, rEntity => new DTOT().SetModel(rEntity));
        }

        public virtual HttpResponseMessage PostEntity(DTOT dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addedEntity = _service.Create(dto.Model);
                    SetDTOAccordingPrimaryKeys(dto, true);

                    //dto.SetModel(addedEntity);
                    return Request.CreateResponse(HttpStatusCode.Created, new { Data = new[] { dto }, Total = 1 });
                }
                catch (DbEntityValidationExceptionBase ex)
                {
                    var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);


        }

        public virtual HttpResponseMessage PutEntity(DTOT dto)
        {
            //To Do: Date problem in validation , we diactive validation temporary
            if (ModelState.IsValid)
            {
                SetDTOAccordingPrimaryKeys(dto);

                var updatedEntityId = _service.Update(dto.Model);

                return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { dto }, Total = 1 });
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }


        public virtual HttpResponseMessage DeleteEntity(DTOT dto)
        {
            try
            {
                SetDTOAccordingPrimaryKeys(dto,true);

                var deletedEntity = _service.Delete(dto.Model);
            }
            catch (DbUpdateConcurrencyExceptionBase)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { dto } });
        }

        private void SetDTOAccordingPrimaryKeys(DTOT dto, bool deleteMode = false)
        {
            var entityInfo = dto.Model.EntityInfo();
            var keyColumns = entityInfo.KeyColumns;
           

            object[] values = new object[keyColumns.Count];

            var entity = dto.Model;

            keyColumns.Each(keyColumn =>
            {
                var i = 0;
                values[i] = entity[keyColumn.Key];
                i++;
            });

            var foundEntity = Service.Find(values);

            if (foundEntity != null && !deleteMode)
            {
                entityInfo.Properties.Where(prop => prop.Value.CanWrite).Each(prop =>
                {
                    foundEntity[prop.Key] = entity[prop.Key];
                });
           }

            dto.SetModel(foundEntity);

        }

    }
}
