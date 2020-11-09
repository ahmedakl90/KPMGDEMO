

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KPMG.eLearningHub.CRM.TestConsole
{
    class Program
    {
        static async Task MainAsync(string[] args)
        {
//             var client = new HttpClient();

//            var result = await client.GetAsync("http://mbshandson.azure-api.net/test?email=Crmadmin@mbskpmg.onmicrosoft.com");
//             if(result.StatusCode == HttpStatusCode.OK) { 
//            bool _flag = bool.Parse(result.Content.ReadAsStringAsync().Result);
//            Console.WriteLine(result.Content.ReadAsStringAsync().Result);
//Console.WriteLine(result.StatusCode);
//            }
            //var response = client.GetAsync("http://mbshandson.azure-api.net/test?email=Crmadmin@mbskpmg.onmicrosoft.com").Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    var responseContent = response.Content;

            //    // by calling .Result you are synchronously reading the result
            //    string responseString = responseContent.ReadAsStringAsync().Result;

            //    Console.WriteLine(responseString);
            //}
        }
        static  void Main(string[] args)
        {
            //var client = new RestClient("http://mbshandson.azure-api.net/test?email=Crmadmin@mbskpmg.onmicrosoft.com");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);


            //var accountJson =  RetrieveAccounts();
            //Console.WriteLine(accountJson);

            //MainAsync(args).GetAwaiter().GetResult();
          var obj =  new ContactEmailCheckerLogic().checkThirdPartyApi("Crmadmin@mbskpmg.onmicrosoft.com").GetAwaiter().GetResult();
        }


    }
    public class ContactEmailCheckerLogic
    {
        public async Task<APIResponse> checkThirdPartyApi(string contactEmail)
        {
            APIResponse resultObj = new APIResponse();
            var client = new HttpClient();

            string url = $"{ GetAPiUrl()}{contactEmail}";
            var result = await client.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                bool _flag = bool.Parse(result.Content.ReadAsStringAsync().Result);
                resultObj.APICallingResponse = true;
                resultObj.EamilCheckResult = _flag;
            }
            return resultObj;
        }
        public string GetAPiUrl()
        {

            return "http://mbshandson.azure-api.net/test?email=";
        }
    }
    public class APIResponse
    {

        public bool APICallingResponse { get; set; }
        public bool EamilCheckResult { get; set; }
    }
}
