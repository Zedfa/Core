using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Cmn;
using System.Collections.Generic;
using System.Net.Http;

namespace Core.UnitTesting.Cmn
{
    [TestClass]
    public class HttpHelperTest
    {
        [TestMethod]
        public void TestWebApiGet()
        {
            Assert.AreEqual(HttpHelper.WebApiGet<bool>("http://sharedflights.sepehrsystems.com/api/Login/ChangeSignUsability?sign=EP830/1389A&pid=51733&usable=false").GetType(), typeof(bool));
        }

        [TestMethod]
        public void TestWebApiPost()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("FromDate", DateTime.Now.ToString("yyyy/MM/dd")),
                new KeyValuePair<string, string>("ToDate", DateTime.Now.AddMonths(0).ToString("yyyy/MM/dd")),
                new KeyValuePair<string, string>("WebApiId", "288794a9-7fde-4cb8-901e-52d80757b534"),
                new KeyValuePair<string, string>("UserID", "b52c9937-fa16-417c-8fb4-1729721498e7"),
                new KeyValuePair<string, string>("ApiSiteID", "288794a9-7fde-4cb8-901e-52d80757b534"),
                new KeyValuePair<string, string>("username", "sepehr"),
                new KeyValuePair<string, string>("password", "123321"),
            });
            Assert.AreEqual(HttpHelper.WebApiPost<List<SafarIranFlight>>("http://WebApiSepehr.safariran.ir/api/FlightReservationApi/SearchAllFlightData", content).GetType(), typeof(List<SafarIranFlight>));
        }

        public class SafarIranFlight
        {
            public string FlightInDateID { get; set; }
            public string TitleFlight { get; set; }
            public string FromAirPort { get; set; }
            public string FromCity { get; set; }
            public string FromState { get; set; }
            public string FromCountry { get; set; }
            public string FromAbbreviation { get; set; }
            public string ToAirPort { get; set; }
            public string ToCity { get; set; }
            public string ToState { get; set; }
            public string ToCountry { get; set; }
            public string ToAbbreviation { get; set; }
            public string Date { get; set; }
            public string StartTime { get; set; }
            public string ArrivalTime { get; set; }
            public string Price { get; set; }
            public string Count { get; set; }
            public string AirLineCode { get; set; }
            public string AirLineTitle { get; set; }
            public string AirPlaneTitle { get; set; }
            public string Ch_IsReserve { get; set; }
            public string Ch_Description { get; set; }
            public string SessionID { get; set; }
            public string IsRequestFlag { get; set; }
            public string IsSystemic { get; set; }
            public string CabinType { get; set; }
            public string IsRefundable { get; set; }
        }
    }
}
