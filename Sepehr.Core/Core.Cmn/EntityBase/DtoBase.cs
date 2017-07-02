using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.Cmn.EntityBase
{
    [DataContract]
    public abstract class DtoBase<T> : ModelBase<T>, IDto where T : IEntity
    {
        public DtoBase()
            : base()
        {

        }
        public DtoBase(T model)
            : base(model)
        {

        }

        [IgnoreDataMember]
        [JsonIgnore]
        [NotMapped]
        public string Culture { get; private set; }

        public static DTO GetDTO<DTO>(T model) where DTO : DtoBase<T>, new()
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var dto = new DTO()
            {
                Culture = culture.EnglishName
            };
            dto.SetModel(model);
            return dto;
        }

        public static DTO GetDTO<DTO>(T model, CultureInfo cultureInfo) where DTO : DtoBase<T>, new()
        {
            var dto = new DTO()
            {
                Culture = cultureInfo.EnglishName
            };
            dto.SetModel(model);
            return dto;
        }

        public static IEnumerable<DTO> GetDTOList<DTO>(IEnumerable<T> models) where DTO : DtoBase<T>, new()
        {
            if (models != null)
            {
                var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                foreach (var model in models.ToList<T>())
                {
                    var dto = new DTO()
                    {
                        Culture = culture.Name
                    };
                    dto.SetModel(model);
                    yield return dto;
                }
            }

        }


        public static IEnumerable<DTO> GetDTOList<DTO>(IEnumerable<T> models, CultureInfo cultureInfo)where DTO : DtoBase<T>, new()
        {
            if (models != null)
            {
                foreach (var model in models.ToList<T>())
                {
                    var dto = new DTO()
                    {
                        Culture = cultureInfo.Name
                    };
                    dto.SetModel(model);
                    yield return dto;
                }

            }

        }

    }
}
