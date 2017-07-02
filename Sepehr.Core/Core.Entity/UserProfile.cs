using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Cmn;
using System.Runtime.Serialization;

namespace Core.Entity
{
    [Table("UserProfiles", Schema = "core")]
    [DataContract]
    public class UserProfile : EntityBase<UserProfile>, IUserProfile
    {
        [DataMember]
        public int Id { get; set; }
        [ForeignKey("Id")]
        [DataMember]
        public virtual User User { get; set; }
        [Required]
        [MaxLength(200)]
        [DataMember]
        public string UserName { get; set; }
        [Required]
        [DataMember]
        public string Password { get; set; }

        //private string _dcName;
        //[NotMapped]
        //public string DCName
        //{
        //    get
        //    {
        //        if (UserName.Contains("@"))
        //            _dcName = UserName.Split('@')[1];
        //        else if (UserName.Contains("\\"))
        //            _dcName = UserName.Split('\\')[0];

        //        return _dcName;
        //    }

        //}
        //[NotMapped]
        //public string DCPassword { get; set; }

        //[NotMapped]
        //public bool IsDCUser
        //{
        //    get
        //    {
        //        return !string.IsNullOrEmpty(DCName);
        //    }
        //}
    }
}
