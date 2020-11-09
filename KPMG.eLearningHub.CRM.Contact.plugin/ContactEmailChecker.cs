using KPMG.eLearningHub.CRM.Contact.plugin.Logic;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMG.eLearningHub.CRM.Contact.plugin
{
    public class ContactEmailChecker : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            #region must to have

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Create service with context of current user
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            //create tracing service
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingService.Trace(context.Depth.ToString());
            #endregion
            if (context.Depth > 1) return;

                #region Contact Email Checker Step
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {

                //create entity context
                Entity entity = (Entity)context.InputParameters["Target"];
                tracingService.Trace("contact Entity");
                tracingService.Trace(entity.LogicalName);
                if (entity.LogicalName != "contact")
                    return;
                tracingService.Trace("Start Logic");
                new ContactEmailCheckerLogic(service, tracingService, entity).StartLogic();
            }
        }

        #endregion
    }

}

