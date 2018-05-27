using Core.Cmn.Extensions;
using System;
using System.Collections.Generic;
using Tools.Cmn;

namespace Core.Cmn.Trace
{
    [Serializable]
    public class TraceDto : IDisposable
    {
        private static int _id;
        public TraceDto()
        {
            Id = ++_id;
            SystemTime = DateTime.Now;
            Data = new Dictionary<string, string>();
        }
        public int Id { get; private set; }

        public string TraceKey { get; set; }

        public DateTime SystemTime
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public string Writer { get; set; }

        private Dictionary<string, string> _data;
        public Dictionary<string, string> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                //SerializedData = Newtonsoft.Json.JsonConvert.SerializeObject(_data);
            }
        }
        private string _serializedData;
        public string SerializedData
        {
            get
            {
                if (string.IsNullOrEmpty(_serializedData))
                    _serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(Data);

                return _serializedData;
            }
        }

        public void Dispose()
        {

        }
    }
}
