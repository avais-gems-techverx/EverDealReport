using CrystalDecisions.CrystalReports.Engine;
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
using System.Web.Http;

namespace EverDealReport.Controllers
{
    public class ReportApiController : ApiController
    {
        public async Task<HttpResponseMessage> GetReport()
        {
            

            StringBuilder sb_url = new StringBuilder();
            sb_url.Append("http://localhost:22742/api/services/app/User/GetUsers");
            //sb_url.Append("api/services/app/Company/GetCompanies");

            var request = (HttpWebRequest)WebRequest.Create(sb_url.ToString());

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            // request.Headers.Add("Authorization", "Bearer " + token );
            request.Headers.Add("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6IjliZWVmYWY4LWY0NzUtNGYyOC04YmRmLWY1ZWQ1MTRhMTdmNyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwic3ViIjoiMSIsImp0aSI6IjQ0ZjJkZDNlLWZiZDctNDg2ZS1iNjI4LTk1ZmI2Mjg4MmRiYiIsImlhdCI6MTU0NDYyMzA3NiwibmJmIjoxNTQ0NjIzMDc2LCJleHAiOjE1NDQ3MDk0NzYsImlzcyI6IlRlc3RQcjBqZWN0IiwiYXVkIjoiVGVzdFByMGplY3QifQ.k3PClniez2SwTkri9qdc_MfHEYRbTgXrw4zNio0tRPc");
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
                        //rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport2.rpt"));
                       //// var MappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Reports");
                        ////rd.Load(Path.Combine(MappedPath.ToString(), "CrystalReport2.rpt"));

                        rd.SetDataSource(dt);
                        Stream streamm = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                         
                        HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                        httpResponseMessage.Content = new StreamContent(streamm);
                        httpResponseMessage.Content.Headers.ContentDisposition = 
                            new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        httpResponseMessage.Content.Headers.ContentDisposition.FileName = "sdafdsf.pdf";
                        httpResponseMessage.Content.Headers.ContentType = 
                            new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                        return httpResponseMessage;
                    }
                }
            }

            return null;


        }
    }
}
