using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API
{
   
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool didUserUpdate { get; set; }
        public async Task Login(string xUserName,string xPassword)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = "http://localhost:3000/login";
                var parameters = new Dictionary<string, string> { };
                parameters.Add("username", xUserName);
                parameters.Add("password", xPassword);
 
                var encodedContent = new FormUrlEncodedContent(parameters);

                //JSON WAY
                // var jsonData = new { id=1,name="smit",location="patel"};
                //string json = JsonConvert.SerializeObject(jsonData);
                //StringContent jsonData1 = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, encodedContent);
                var message = await response.Content.ReadAsStringAsync();
              
                var jsonObject = JObject.Parse(message);
                var IsValid = jsonObject["IsValid"].ToString();
                Debug.WriteLine(jsonObject["UserID"]);
                if(IsValid == "True")
                {
                    UserID = Convert.ToInt32(jsonObject["UserID"]);
                    UserName = xUserName;
                    didUserUpdate = true;
                }
                else
                {
                   didUserUpdate=false;
                }

            }
        }
    }
}
