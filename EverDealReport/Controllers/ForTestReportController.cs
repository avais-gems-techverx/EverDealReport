using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Web.Http.Cors;
using System.Configuration;
using EverDealObjects;

namespace EverDealReport.Controllers
{




    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ForTestReportController : Controller
    {
         

        // GET: ForTestReport
        public   ActionResult Index()
        {

            return View();
       }
        

        public async Task<ActionResult> GetFundingDeficiency(string k)
        {

            try
            {


                //ASCIIEncoding encoder = new ASCIIEncoding();
                //byte[] data = encoder.GetBytes(serializedObject); // a json object, or xml, whatever...

                //HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //request.Method = "POST";
                //request.ContentType = "application/json";
                //request.ContentLength = data.Length;
                //request.Expect = "application/json";

                //request.GetRequestStream().Write(data, 0, data.Length);

                //HttpWebResponse response = request.GetResponse() as HttpWebResponse;




                string ApiUrl = ConfigurationSettings.AppSettings.Get("Api_URL");
                int Api_Timeout = Convert.ToInt32( ConfigurationSettings.AppSettings.Get("Api_Timeout"));
                StringBuilder sb_url = new StringBuilder();
               // sb_url.Append("http://localhost:22742/api/services/app/Deal/Funding_Deficiency");
                sb_url.Append(ApiUrl+ "/api/services/app/Deal/Funding_Deficiency");

                var request = (HttpWebRequest)WebRequest.Create(sb_url.ToString());

                request.Method = "POST";
                request.Timeout = Api_Timeout; //120000;  // 2 minutes
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("Authorization", "Bearer " + k);
                request.ContentLength= 0;;
                var content = string.Empty;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (
                            var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();

                            JObject rss = JObject.Parse(content);

                            var postTitles =
                                from p in rss["result"]["items"]
                                select p;

                            DataTable dt = new DataTable("Funding_Deficiency");
                            dt.Columns.Add("DeliveryDate");
                            dt.Columns.Add("Age");
                            dt.Columns.Add("Type");
                            dt.Columns.Add("StockNumber");
                            dt.Columns.Add("VehicleDescription");
                            dt.Columns.Add("CustomerName");
                            dt.Columns.Add("SalesAssociate1");
                            dt.Columns.Add("SalesAssociate2");
                            dt.Columns.Add("FinanceManager");
                            dt.Columns.Add("SalesManager");
                            dt.Columns.Add("FrontGross");
                            dt.Columns.Add("BackGross");
                            dt.Columns.Add("TotalGross");
                            

                            foreach (var item in postTitles)
                            {
                                DataRow dr = dt.NewRow();
                                dr["DeliveryDate"] = Convert.ToDateTime(item["deliveryDateTime"].ToString()).ToString("dd/MMM/yy")
                                   ;
                                dr["Age"] = "12";
                                dr["Type"] = item["dealTypeName"].ToString();
                                dr["StockNumber"] = item["stockNumber"].ToString();
                                dr["VehicleDescription"] = item["stockModel"].ToString();
                                dr["CustomerName"] = string.Concat( item["customerFirstName"].ToString() ," ", item["customerLastName"].ToString());
                                dr["SalesAssociate1"] = item["salesPerson1Name"].ToString();
                                dr["SalesAssociate2"] = item["salesPerson2Name"].ToString();
                                dr["FinanceManager"] = item["businessManagerName"].ToString();   
                                dr["SalesManager"] = item["salesManagerName"].ToString();
                                dr["FrontGross"] = item["frontGross"].ToString();
                                dr["BackGross"] = "0";
                                dr["TotalGross"] = item["totalGross"].ToString();
                                

                                dt.Rows.Add(dr);
                            }

                            ReportDocument rd = new ReportDocument();
                            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FundingDeficiency.rpt"));

                            rd.SetDataSource(dt);
                            Stream streamm = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                            Response.Buffer = false;
                            Response.ClearContent();
                            Response.ClearHeaders();


                            streamm.Seek(0, SeekOrigin.Begin);
                            return File(streamm, "application/pdf");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ex = ex.Message;
            }
            //return content;
            return View();


        }
         
        public async Task<ActionResult> GetDealListManagerAccess( string k ,string fromDate, string toDate,int reportId,long dealType = 0, long storeId = 0, long departmentId = 0, long companyId = 0, string leadSource = "", string trackingCode = "", long teamId = 0, long status = 0, string inv = "")   // ReportParameter ReportParameterDTO)
        {

            try
            { 


                string ApiUrl = ConfigurationSettings.AppSettings.Get("Api_URL");
                int Api_Timeout = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("Api_Timeout"));
                StringBuilder sb_url = new StringBuilder();
                // sb_url.Append("http://localhost:22742/api/services/app/Deal/Funding_Deficiency");
                sb_url.Append(ApiUrl + "/api/services/app/Deal/Funding_Deficiency?fromDate=" + fromDate  + "&toDate="+ toDate  + "&dealType="+ dealType + "&storeId=" + storeId+ "&departmentId="+ departmentId+ "&companyId="+ companyId + "&leadSource=" + leadSource + "&trackingCode="+ trackingCode + "&teamId=" + teamId + "&status="+ status + "&inv="+ inv);

                var request = (HttpWebRequest)WebRequest.Create(sb_url.ToString());

                request.Method = "POST";
                request.Timeout = Api_Timeout; //120000;  // 2 minutes
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("Authorization", "Bearer " + k);
                request.ContentLength = 0; ;
                var content = string.Empty;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (
                            var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();

                            JObject rss = JObject.Parse(content);

                            var postTitles =
                                from p in rss["result"]["items"]
                                select p;

                            DataTable dt = new DataTable("Funding_Deficiency");
                            dt.Columns.Add("DeliveryDate");
                            dt.Columns.Add("Age");
                            dt.Columns.Add("Type");
                            dt.Columns.Add("StockNumber");
                            dt.Columns.Add("VehicleDescription");
                            dt.Columns.Add("CustomerName");
                            dt.Columns.Add("SalesAssociate1");
                            dt.Columns.Add("SalesAssociate2");
                            dt.Columns.Add("FinanceManager");
                            dt.Columns.Add("SalesManager");
                            dt.Columns.Add("FrontGross");
                            dt.Columns.Add("BackGross");
                            dt.Columns.Add("TotalGross");


                            foreach (var item in postTitles)
                            {
                                DataRow dr = dt.NewRow();
                                dr["DeliveryDate"] = Convert.ToDateTime(item["deliveryDateTime"].ToString()).ToString("dd/MMM/yy")
                                   ;
                                dr["Age"] = "12";
                                dr["Type"] = item["type"].ToString();
                                dr["StockNumber"] = item["stockNumber"].ToString();
                                dr["VehicleDescription"] = item["stockModel"].ToString();
                                dr["CustomerName"] = string.Concat(item["customerFirstName"].ToString(), " ", item["customerLastName"].ToString());
                                dr["SalesAssociate1"] = item["salesPerson1Name"].ToString();
                                dr["SalesAssociate2"] = item["salesPerson2Name"].ToString();
                                dr["FinanceManager"] = item["businessManagerName"].ToString();
                                dr["SalesManager"] = item["salesManagerName"].ToString();
                                dr["FrontGross"] = item["frontGross"].ToString();
                                dr["BackGross"] = "0";
                                dr["TotalGross"] = item["totalGross"].ToString();


                                dt.Rows.Add(dr);
                            }

                            ReportDocument rd = new ReportDocument();
                            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FundingDeficiency.rpt"));

                            rd.SetDataSource(dt);
                            Stream streamm = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                            Response.Buffer = false;
                            Response.ClearContent();
                            Response.ClearHeaders();


                            streamm.Seek(0, SeekOrigin.Begin);
                            return File(streamm, "application/pdf", "DealListManagerAccess");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ex = ex.Message;
            }
            //return content;
            return View();


        }

        public async Task<ActionResult> GetReport(string k)
        {

            try
            {
                StringBuilder sb_url = new StringBuilder();
                sb_url.Append("https://everdealdev.azurewebsites.net/api/services/app/User/GetUsers");
                //sb_url.Append("api/services/app/Company/GetCompanies");

                var request = (HttpWebRequest)WebRequest.Create(sb_url.ToString());

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("Authorization", "Bearer " + k);

                var content = string.Empty;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (
                            var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();

                            JObject rss = JObject.Parse(content);

                            var postTitles =
                                from p in rss["result"]["items"]
                                select p;

                            DataTable dt = new DataTable();
                            dt.Columns.Add("name");
                            dt.Columns.Add("surname");

                            foreach (var item in postTitles)
                            {
                                DataRow dr = dt.NewRow();
                                dr["name"] = item["name"].ToString();
                                dr["surname"] = item["surname"].ToString();
                                dt.Rows.Add(dr);
                            }

                            ReportDocument rd = new ReportDocument();
                            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport2.rpt"));

                            rd.SetDataSource(dt);
                            Stream streamm = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                            Response.Buffer = false;
                            Response.ClearContent();
                            Response.ClearHeaders();


                            streamm.Seek(0, SeekOrigin.Begin);
                            return File(streamm, "application/pdf");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ex = ex.Message;
            }
            //return content;
            return View();


        }
        public async Task<ActionResult> GetReport_withtoken(string token)
        {
            
            // string jss = @"{'result':{'totalCount':1,'items':[{'name':'admin','surname':'admin','userName':'admin','emailAddress':'admin @aspnetzero.com','phoneNumber':null,'profilePictureId':null,'isEmailConfirmed':true,'roles':[{'roleId':1,'roleName':'Admin'}],'lastLoginTime':'2018 - 12 - 11T11: 46:12.9853501Z','isActive':true,'creationTime':'2018 - 12 - 04T09: 47:51.2578196Z','id':1}]},'targetUrl':null,'success':true,'error':null,'unAuthorizedRequest':false,'__abp':true}";

            //JObject rss = JObject.Parse(jss);

            //var postTitles =
            //    from p in rss["result"]["items"]
            //    select p ;

            //DataTable dt = new DataTable();
            //dt.Columns.Add("name");
            //dt.Columns.Add("surname");





            //foreach (var item in postTitles)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["name"] = item["name"].ToString();
            //    dr["surname"] = item["surname"].ToString();
            //    dt.Rows.Add(dr);
            //}

            //ReportDocument rd = new ReportDocument();
            //rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport2.rpt"));

            //rd.SetDataSource(dt);

            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();


            //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //stream.Seek(0, SeekOrigin.Begin);
            //return File(stream, "application/pdf", "CustomerList.pdf");

            //        string json = @"{
            //  'channel': {
            //    'title': 'James Newton-King',
            //    'link': 'http://james.newtonking.com',
            //    'description': 'James Newton-King\'s blog.',
            //    'item': [
            //      {
            //        'title': 'Json.NET 1.3 + New license + Now on CodePlex',
            //        'description': 'Announcing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
            //        'link': 'http://james.newtonking.com/projects/json-net.aspx',
            //        'categories': [
            //          'Json.NET',
            //          'CodePlex'
            //        ]
            //      },
            //      {
            //        'title': 'LINQ to JSON beta',
            //        'description': 'Announcing LINQ to JSON',
            //        'link': 'http://james.newtonking.com/projects/json-net.aspx',
            //        'categories': [
            //          'Json.NET',
            //          'LINQ'
            //        ]
            //      }
            //    ]
            //  }
            //}";

            //JObject rss = JObject.Parse(json);

            //var postTitles =
            //    from p in rss["channel"]["item"]
            //    select (string)p["title"];

            //foreach (var item in postTitles)
            //{
            //    string dd = item.ToString();
            //}




            StringBuilder sb_url = new StringBuilder();
            sb_url.Append("http://localhost:22742/api/services/app/User/GetUsers");
            //sb_url.Append("api/services/app/Company/GetCompanies");

            var request = (HttpWebRequest)WebRequest.Create(sb_url.ToString());

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token);
            //  request.Headers.Add("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6IjliZWVmYWY4LWY0NzUtNGYyOC04YmRmLWY1ZWQ1MTRhMTdmNyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwic3ViIjoiMSIsImp0aSI6ImMwODkxN2NiLTcxOGUtNDVjMi1iYjZkLWNkZmJlOTc4MzlhOSIsImlhdCI6MTU0NDUyODc3MywibmJmIjoxNTQ0NTI4NzczLCJleHAiOjE1NDQ2MTUxNzMsImlzcyI6IlRlc3RQcjBqZWN0IiwiYXVkIjoiVGVzdFByMGplY3QifQ._bKr8GfnsQYIiTlvRf4n4srmXkK6SbjdMMbv3WH5wl8");
            // request.UserAgent = RequestConstants.UserAgentValue;
            //  request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (
                        var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();

                        JObject rss = JObject.Parse(content);

                        var postTitles =
                            from p in rss["result"]["items"]
                            select p;

                        DataTable dt = new DataTable();
                        dt.Columns.Add("name");
                        dt.Columns.Add("surname");

                        foreach (var item in postTitles)
                        {
                            DataRow dr = dt.NewRow();
                            dr["name"] = item["name"].ToString();
                            dr["surname"] = item["surname"].ToString();
                            dt.Rows.Add(dr);
                        }

                        ReportDocument rd = new ReportDocument();
                        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport2.rpt"));

                        rd.SetDataSource(dt);

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();


                        Stream streamm = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        streamm.Seek(0, SeekOrigin.Begin);
                        return File(streamm, "application/pdf", "CustomerList.pdf");

                        //XmlDocument xd1 = new XmlDocument();
                        //xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(content);
                        //DataSet jsonDataSet = new DataSet();
                        //jsonDataSet.ReadXml(new XmlNodeReader(xd1));

                        //// DataSet ds = JObject.Parse(content)["items"].ToObject<DataSet>();
                        //DataSet myDataSet =
                        //    JsonConvert.DeserializeObject<DataSet>(content);
                    }
                }
            }

            //return content;
            return View();


        }
    }
}