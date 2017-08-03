using Core.Cmn;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    [Table("DeletedCachedRecords", Schema = "core")]
    [DataContract]
    public class DeletedCachedRecord : EntityBase<DeletedCachedRecord>, IDeletedCachedRecord
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        //ToDo: change DeletedTimeStamp name to IdForDeletedEntity and datatype int
        public string DeletedTimeStamp { get; set; }
        [DataMember]

        public string TableName { get; set; }
        private int _deletedEntityId;
        public int DeletedEntityId
        {
            get
            {
                if (_deletedEntityId == default(int))
                    _deletedEntityId = int.Parse(DeletedTimeStamp);
                return _deletedEntityId;
            }
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        [DataMember]

        public DateTime DeletionDateTime
        {
            get;
            set;
        }
    }
}
