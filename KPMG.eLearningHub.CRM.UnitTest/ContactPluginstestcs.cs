using KPMG.eLearningHub.CRM.Contact.plugin.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMG.eLearningHub.CRM.UnitTest
{
    [TestClass]
    public class ContactPluginstestcs
    {
        [TestMethod]
        public void TestThirdPartyAPI()
        {
            // Arrange
            bool expected = true;
            APIResponse apiResponse= new ContactEmailCheckerLogic().CheckThirdPartyApi("ahmed@test.tes").GetAwaiter().GetResult();
            // Act
            

            // Assert
            bool actual = apiResponse.APICallingResponse;
            Assert.AreEqual(expected, actual);
        }
    }
}
