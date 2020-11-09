using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KPMG.eLearningHub.CRM.Contact.plugin.Logic
{
 
    public class ContactEmailCheckerLogic
    {
        public Entity Contact;
        public IOrganizationService CrmService;
        private IOrganizationService service;
        private ITracingService tracingService;
        private Entity entity;

        public ContactEmailCheckerLogic(IOrganizationService Service, Entity ContactEntity)
        {
            this.CrmService = Service;
            this.Contact = ContactEntity;
        }

        public ContactEmailCheckerLogic(IOrganizationService service, ITracingService tracingService, Entity entity)
        {
            this.CrmService = service;
            this.tracingService = tracingService;
            this.Contact = entity;
        }

        public ContactEmailCheckerLogic()
        {
        }

        public void StartLogic()
        {
            tracingService.Trace("StartLogic");
            tracingService.Trace(Contact.Id.ToString());
            string conatctEmail =  GetTargetContactEmail(Contact.Id);
            tracingService.Trace("conatctEmail");
            if (string.IsNullOrWhiteSpace(conatctEmail)) return;
            APIResponse repsonse = CheckThirdPartyApi(conatctEmail).GetAwaiter().GetResult();
            // Update The Contact 
            Entity contacttoUpdate = new Entity(Contact.LogicalName, Contact.Id);
            if (!repsonse.APICallingResponse)
            {
                contacttoUpdate["kpmg_duplicatecheck"] = new OptionSetValue(9);
            }
            else
            {
              
                if (repsonse.EamilCheckResult)
                    contacttoUpdate["kpmg_duplicatecheck"] = new OptionSetValue(1);

                else if (!repsonse.EamilCheckResult)
                    contacttoUpdate["kpmg_duplicatecheck"] = new OptionSetValue(2);
            }

            CrmService.Update(contacttoUpdate);
        }

        private string GetTargetContactEmail(Guid contactId)
        {

            tracingService.Trace("GetTargetContactEmail");
            // Instantiate QueryExpression QEcontact
            var QEcontact = new QueryExpression("contact");

            // Add columns to QEcontact.ColumnSet
            QEcontact.ColumnSet.AddColumns("adx_identity_emailaddress1confirmed", "emailaddress1");

            // Define filter QEcontact.Criteria
            QEcontact.Criteria.AddCondition("contactid", ConditionOperator.Equal, contactId.ToString());
            var reslult = CrmService.RetrieveMultiple(QEcontact).Entities.ToList();
            if (reslult.Count > 0)
            {
                return reslult.FirstOrDefault().GetAttributeValue<string>("emailaddress1");
            }
            return "";

        }

        public async Task<APIResponse> CheckThirdPartyApi(string contactEmail)
        {
            tracingService?.Trace("CheckThirdPartyApi");
            APIResponse resultObj = new APIResponse();
            var client = new HttpClient();

            string url = $"{ GetAPiUrl()}{contactEmail}";
            var result = await client.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                bool _flag = bool.Parse(result.Content.ReadAsStringAsync().Result);
                resultObj.APICallingResponse = true;
                resultObj.APICallingResponse = _flag;
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
