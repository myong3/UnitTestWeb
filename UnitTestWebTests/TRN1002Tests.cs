using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestWeb.Controllers;
using UnitTestWeb.Controllers.TRN;

namespace UnitTestWebTests
{
    [TestClass]
    public class TRN1002Tests
    {
        [TestMethod]
        public void True()
        {

            var expected = "TRN1002";
            TRNController account = new TRNController();

            // Act
            var actual = account.TRN1002(true);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        //[TestMethod]
        //public void False()
        //{

        //    var expected = "trn1002";
        //    TRNController account = new TRNController();

        //    // Act
        //    var actual = account.TRN1002(false);

        //    // Assert
        //    Assert.AreEqual(expected, actual);
        //}
    }
}