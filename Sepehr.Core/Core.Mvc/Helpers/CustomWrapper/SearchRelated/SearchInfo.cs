using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;

namespace Core.Mvc.Helpers.CustomWrapper.SearchRelated
{
    [Serializable]
    public abstract class SearchInfo
    {
        public string CustomType { get; private set; }
        public string MainPropertyNameOfModel { get; private set; }
        public string NavigationProperty { get; private set; }

        public List<Constant> Constants { get; set; }
        public bool IdReplacement { get; private set; }

        public SearchInfo HasCustomTypeOf(string customType)
        {
            this.CustomType = customType;
            return this;
        }

        public SearchInfo HasMainPropNameOf(string mainPropertyNameOfModel)
        {
            this.MainPropertyNameOfModel = mainPropertyNameOfModel;
            return this;
        }

        //public SearchInfo HasFalseEquivOf(string falseEquiv)
        //{
        //    this.FalseEquivalent = falseEquiv;
        //    return this;
        //}

        //public SearchInfo HasTrueEquivOf(string trueEquiv)
        //{
        //    this.TrueEquivalent = trueEquiv;
        //    return this;
        //}

        public SearchInfo SetIdReplacement(bool idReplacement)
        {
            this.IdReplacement = idReplacement;
            return this;
        }
    }
}
