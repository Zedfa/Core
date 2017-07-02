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

        public string DeletedTimeStamp { get; set; }
        [DataMember]

        public string TableName { get; set; }
        private long _deletedEntityId;
        public long DeletedEntityId
        {
            get
            {
                if (_deletedEntityId == default(long))
                    _deletedEntityId = long.Parse(DeletedTimeStamp);
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
