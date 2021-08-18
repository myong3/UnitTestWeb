using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestWeb.Controllers;
using UnitTestWeb.Controllers.TRN;
using UnitTestWeb.Models;

namespace UnitTestWebTests
{
    [TestClass]
    public class TRN1001Tests
    {
        private TRNController _TRNController = null;

        [TestInitialize]
        public void Init()
        {
            _TRNController = new TRNController();
        }

        [TestMethod]
        public void TRN1001_�]Request���e���~_1_�^�ǥ���()
        {
            var req = new TRN1001Req();

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "�Ѽƿ��~:type:'',actNo:'',outActNo:'',txAmt: 0(���o��0)",
                    responseCode = "98",
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_�]Request���e���~_2_�^�ǥ���()
        {
            var req = new TRN1001Req()
            {
                type = "6",
                actNo = "1",
                outActNo = "1",
                txAmt = 1
            };

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "�Ѽƿ��~:type:'6'",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_�]Request���e���~_3_�^�ǥ���()
        {
            var req = new TRN1001Req()
            {
                type = "1",
                actNo = "",
                outActNo = "1",
                txAmt = 1
            };

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "�Ѽƿ��~:actNo:''",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_�]Request���e���~_4_�^�ǥ���()
        {
            var req = new TRN1001Req()
            {
                type = "1",
                actNo = "12345678901234",
                outActNo = "",
                txAmt = 1
            };

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "�Ѽƿ��~:outActNo:''",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_�]Request���e���~_5_�^�ǥ���()
        {
            var req = new TRN1001Req()
            {
                type = "1",
                actNo = "12345678901234",
                outActNo = "12345678901234",
                txAmt = 0
            };

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "�Ѽƿ��~:txAmt: 0(���o��0)",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        [Ignore]
        public void TRN1001_�]Request���e���~_6_�^�ǥ���()
        {
            var req = new TRN1001Req()
            {
                type = "1",
                actNo = "12345678901234",
                outActNo = "12345678901234",
                txAmt = 10
            };

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "API�������ҥ���-����JauthResponses�BfipsPWD�BregionCode",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        [Ignore]
        public void TRN1001_�]Request���e���~_7_�^�ǥ���()
        {
            var req = new TRN1001Req()
            {
                type = "1",
                actNo = "12345678901234",
                outActNo = "12345678901234",
                txAmt = 10,
                authResponses = "xxx"
            };

            var actual = _TRNController.TRN1001(req);

            actual.responseCode.Should().Be("95");
        }

        [TestMethod]
        public void TRN1001_�sï��sï_�^�Ǧ��\()
        {
            var req = new TRN1001Req()
            {
                type = "1",
                actNo = "12345678901234",
                outActNo = "12345678901234",
                txAmt = 10,
                fipsPWD = "test"
            };
            var actual = _TRNController.TRN1001(req);

            //actual.errMsg.Should().BeEmpty();
            //actual.responseCode.Should().Be("0");

            Assert.IsTrue(actual.responseCode == "0");

        }

        [TestMethod]
        public void TRN1001_�sï�๺��_�^�Ǧ��\()
        {
            var req = new TRN1001Req()
            {
                type = "2",
                actNo = "12345678901234",
                outActNo = "12345678",
                txAmt = 10,
                fipsPWD = "test"
            };
            var actual = _TRNController.TRN1001(req);

            actual.errMsg.Should().BeEmpty();
            actual.responseCode.Should().Be("0");
        }

        [TestMethod]
        public void TRN1001_�sï��L��_�^�Ǧ��\()
        {
            var req = new TRN1001Req()
            {
                type = "3",
                actNo = "12345678901234",
                outBankNo = "012",
                outActNo = "1234567890123456",
                txAmt = 10,
                fipsPWD = "test"
            };
            var actual = _TRNController.TRN1001(req);

            actual.errMsg.Should().BeEmpty();
            actual.responseCode.Should().Be("0");
        }

        [TestMethod]
        public void TRN1001_������sï_�^�Ǧ��\()
        {
            var req = new TRN1001Req()
            {
                type = "4",
                actNo = "12345678",
                outActNo = "12345678901234",
                txAmt = 10,
                fipsPWD = "test"
            };
            var actual = _TRNController.TRN1001(req);

            actual.errMsg.Should().BeEmpty();
            actual.responseCode.Should().Be("0");
        }

        [TestMethod]
        public void TRN1001_�����๺��_�^�Ǧ��\()
        {
            var req = new TRN1001Req()
            {
                type = "5",
                actNo = "12345678",
                outActNo = "87654321",
                txAmt = 10,
                fipsPWD = "test"
            };
            var actual = _TRNController.TRN1001(req);

            actual.errMsg.Should().BeEmpty();
            actual.responseCode.Should().Be("0");
        }
    }
}
