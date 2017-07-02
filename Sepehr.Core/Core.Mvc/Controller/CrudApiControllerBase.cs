
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
using Core.Ef.Exceptions;

namespace Core.Mvc.Controller
{
    public class CrudApiControllerBase<EntityT, ViewModelT, ServiceT> : Core.Mvc.Controller.ApiControllerBase
        where EntityT : EntityBase<EntityT>, new()
        where ViewModelT : IViewModelBase<EntityT>, new()
        where ServiceT : IServiceBase<EntityT>
    {
        private ServiceT _service;

        public ServiceT Service
        {
            get { return _service; }
        }
        public CrudApiControllerBase(ServiceT service)
        {
            _service = service;
        }

        public virtual DataSourceResult GetEntities([ModelBinder(typeof(ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var entities = _service.All();
            return entities.ToDataSourceResult(request, rEntity => new ViewModelT().SetModel(rEntity));
        }

        public virtual HttpResponseMessage PostEntity([ModelBinder(typeof(ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, ViewModelT viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addedEntity = _service.Create(viewModel.Model);
                    SetViewModelAccordingPrimaryKeys(viewModel);
                    //viewModel.SetModel(addedEntity);
                    return Request.CreateResponse(HttpStatusCode.Created, new { Data = new[] { viewModel }, Total = 1 });
                }
                catch (DbEntityValidationExceptionBase ex)
                {
                    var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);


        }

        public virtual HttpResponseMessage PutEntity([ModelBinder(typeof(ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, ViewModelT viewModel)
        {
            if (ModelState.IsValid)
            {
                SetViewModelAccordingPrimaryKeys(viewModel);

                var updatedEntityId = _service.Update(viewModel.Model);

                return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { viewModel }, Total = 1 });
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }


        public virtual HttpResponseMessage DeleteEntity(ViewModelT viewModel)
        {
            try
            {
                SetViewModelAccordingPrimaryKeys(viewModel,true);

                var deletedEntity = _service.Delete(viewModel.Model);
            }
            catch (DbUpdateConcurrencyExceptionBase)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Entity");
        }

        private void SetViewModelAccordingPrimaryKeys(ViewModelT viewModel, bool deleteMode=false)
        {
            var count = viewModel.Model.EntityInfo().KeyColumns.Count;

            object[] values = new object[count];

            var entity = viewModel.Model;

            viewModel.Model.EntityInfo().KeyColumns.Each(keyColumn =>
            {
                var i = 0;
                values[i] = entity[keyColumn.Key];
                i++;
            });

            var foundEntity = Service.Find(values);

            if (foundEntity != null && !deleteMode)
            {
                viewModel.Model.EntityInfo().WritableProperties.Each(prop =>
                {
                    foundEntity[prop.Key] = entity[prop.Key];
                });
           }

            viewModel.SetModel(foundEntity);

        }

    }
}
