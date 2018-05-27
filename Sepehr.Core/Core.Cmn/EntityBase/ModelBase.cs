using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.Cmn
{
    [Serializable]
    [DataContract]
    public abstract class ModelBase<T> : EntityBase<T> , IModelBase<T>, IValidatableObject where T : IEntity
    {
        #region Private Members
        List<ValidationResult> _validationResults = new List<ValidationResult>();
        #endregion

        #region Ctor
        public ModelBase(T model)
        {
            SetModel(model);
        }
        public ModelBase()
        {
            var modelType = typeof(T);
            if (modelType.IsInterface || modelType.IsAbstract || modelType.GetConstructors().All(ctor => ctor.GetParameters().Count() > 0))
                throw new ArgumentException(@"you just can new this type use the constructor one that pass type of model ,
                                                because your model type is an iterface or abstract class or nonparameterless constractor.");
            SetModel(Activator.CreateInstance<T>());
        }

        #endregion

        #region Peroperties

        [ScriptIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        private T _model;

        [ScriptIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public T Model
        {
            get
            {
                return _model;
            }
            private set
            {
                if (value == null)
                {
                    throw new NullReferenceException("your model cannot be null! please report Mr.Chegini!");
                }
                else
                    _model = value;
            }
        }

        public T GetModel()
        {
            return _model;
        }

        [ScriptIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public List<ValidationResult> ValidationErrors
        {
            get { return _validationResults; }
        }

        #endregion

        #region Methods

        //public override long LongId
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}
        public IModelBase<T> SetModel(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("your model cannot be null! please report Mr.Chegini!");
            }

            _model = model;
            return this;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ValidationErrors;
        }

        public IEntity GetObjectModel()
        {

            return this.Model;
        }


        #endregion

    }
}
