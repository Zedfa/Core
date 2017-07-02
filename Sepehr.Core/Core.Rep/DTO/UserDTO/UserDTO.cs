using Core.Cmn.EntityBase;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rep.DTO
{
    [DataContract]
    public class UserDTO : DtoBase<User>
    {

        [DataMember(IsRequired = true)]
        public int SelectedRoleId { get; set; }

        [DataMember]
        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; }

        }



        [DataMember]
        public string FName
        {
            get { return Model.FName; }

            set { Model.FName = value; }
        }

        [DataMember]
        public string LName
        {
            get { return Model.LName; }
            set { Model.LName = value; }

        }

        [DataMember]
        public string UserFullName
        {
            get { return Model.FName + " " + Model.LName; }

        }

        [DataMember]
        private string _userName;
        [DataMember]
        public string UserName
        {

            get
            {
                if (string.IsNullOrEmpty(_userName))
                    return Model.UserProfile != null ? Model.UserProfile.UserName : null;
                return _userName;
            }
            set
            {

                _userName = value;
            }
        }

        [DataMember]
        private string _password;
        [DataMember]
        //[StrengthChecker]
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _password;
            }
            set
            {
                _password = value;
            }
        }


        [DataMember]
        private string _comparePassword;
        [DataMember]
        public string ComparePassword
        {
            get
            {
                if (string.IsNullOrEmpty(_comparePassword))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _comparePassword;
            }
            set
            {
                _comparePassword = value;
            }
        }



        [DataMember]
        private string _confirmPassword;
        [DataMember]
        public string ConfirmPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_confirmPassword))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
            }
        }


        [DataMember]
        public int? CompanyChartId
        {
            get { return Model.CompanyChartId; }

            set
            {

                Model.CompanyChartId = value;



            }
        }


        [DataMember]
        private string _companyName;
        [DataMember]
        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(_companyName))
                    return Model.CompanyChart != null ? Model.CompanyChart.Title : null;
                return _companyName;
            }
            set
            {
                _companyName = value;
            }
        }




        [DataMember]
        public string CompanyOfHeadUser { get; set; }


        [DataMember]
        public bool Active
        {
            get
            {
                //if (Model.Id == 0)
                //    return true;
                return Model.Active;
            }

            set
            {
                Model.Active = value;
            }
        }


        [DataMember]
        public string ActiveStatus
        {
            get { return Active == false ? "غیر فعال" : "فعال"; }

        }


        [DataMember]
        private string _roleChoosInHeadUser;
        [DataMember]

        public string RoleChoosInHeadUser
        {
            get
            {
                if (string.IsNullOrEmpty(_roleChoosInHeadUser))
                {
                    return (Model.UserRoles != null) ? ((Model.UserRoles.Count > 0) ? (Model.UserRoles.Where(x => x.UserId == Model.Id).LastOrDefault().Role.Name) : "") : "";
                }
                return _roleChoosInHeadUser;
            }
            set
            {
                _roleChoosInHeadUser = value;
            }
        }


        [DataMember]
        public int _roleIdForHead;
        [DataMember]
        public int RoleIdForHead
        {
            get
            {
                if (Model.UserRoles != null)
                {
                    //tdo:felan farz bar in hast ke user admin sazman faghat 1 role darad...
                    return (Model.UserRoles != null) ? ((Model.UserRoles.Count > 0) ? (Model.UserRoles.Where(x => x.UserId == Model.Id).Last().RoleID) : 0) : 0;
                }
                return _roleIdForHead;
            }
            set
            {
                _roleIdForHead = value;
            }
        }
    }
}