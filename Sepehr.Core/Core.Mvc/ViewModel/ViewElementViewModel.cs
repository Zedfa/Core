using Core.Mvc.ViewModel;
using Core.Entity;
using Core.Entity.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel
{
    public class ViewElementViewModel : ViewModelBase<ViewElement>
    {

        public ViewElementViewModel()
            : base()
        {

        }
        public ViewElementViewModel(ViewElement model)
            : base(model)
        {

        }

        [DataMember]
        public bool HasChildren { get; set; }
        [DataMember(Name="id")]
        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; }

        }
        [DataMember]

        //[Display(Name = "Role", ResourceType = typeof(Resource.Resource))]
        [Display(Name = "نقش")]

        public int? ViewElementParentId
        {
            get { return Model.ParentId; }
            set { Model.ParentId = value; }

        }

        [DataMember]

        //[Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resource.Resource))]
        //[Display(Name = "title", ResourceType = typeof(Resource.Resource))]
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        [Display(Name = "عنوان")]

        public string Title
        {
            get { return Model.Title; }

            set { Model.Title = value; }
        }

        [DataMember]

        //[Required(ErrorMessageResourceName = "ViewElementUniqueNameRequired", ErrorMessageResourceType = typeof(Resource.Resource))]
        //[Display(Name = "ViewElementUniqueName", ResourceType = typeof(Resource.Resource))]
        [Required(ErrorMessage = "نام غیر تکراری را وارد کنید")]
        [Display(Name = "نام یکتا")]

        public string UniqueName
        {
            get { return Model.UniqueName; }

            set { Model.UniqueName = value; }
        }

        [DataMember]
        //[Display(Name = "ViewElementElementType", ResourceType = typeof(Resource.Resource))]
        [Display(Name = "نوع المنت")]

        public ElementType ElementType
        {
            get { return Model.ElementType; }

            set { Model.ElementType = value; }
        }

        [DataMember]
       // [Display(Name = "PicturePath", ResourceType = typeof(Resource.Resource))]
        [Display(Name = "مسیر عکس")]

        public string XMLViewData
        {
            get { return Model.XMLViewData; }

            set { Model.XMLViewData = value; }
        }

        [DataMember]
      //  [Display(Name = "ViewElementsHidden", ResourceType = typeof(Resource.Resource))]
        [Display(Name = "زیر شاخه غیر قابل نمایش")]

        public bool IsHidden
        {
            get { return Model.IsHidden; }

            set { Model.IsHidden = value; }
        }

        [DataMember]
       // [Display(Name = "ViewElementInVisible", ResourceType = typeof(Resource.Resource))]
        [Display(Name = "غیر قابل نمایش")]

        public bool Invisible
        {
            get { return (bool)Model.InVisible; }

            set { Model.InVisible = value; }
        }

        [DataMember]
        [Display(Name = "ترتیب")]
        public int SortOrder
        {
            get { return Model.SortOrder; }

            set { Model.SortOrder = value; }
        }


    }

}