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
        public void TRN1001_因Request內容有誤_1_回傳失敗()
        {
            var req = new TRN1001Req();

            var actual = _TRNController.TRN1001(req);

            var expectResult =
                new TRN1001Res
                {
                    errMsg = "參數錯誤:type:'',actNo:'',outActNo:'',txAmt: 0(不得為0)",
                    responseCode = "98",
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_因Request內容有誤_2_回傳失敗()
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
                    errMsg = "參數錯誤:type:'6'",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_因Request內容有誤_3_回傳失敗()
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
                    errMsg = "參數錯誤:actNo:''",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_因Request內容有誤_4_回傳失敗()
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
                    errMsg = "參數錯誤:outActNo:''",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        public void TRN1001_因Request內容有誤_5_回傳失敗()
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
                    errMsg = "參數錯誤:txAmt: 0(不得為0)",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        [Ignore]
        public void TRN1001_因Request內容有誤_6_回傳失敗()
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
                    errMsg = "API執行驗證失敗-未輸入authResponses、fipsPWD、regionCode",
                    responseCode = "98"
                };

            actual.Should().BeEquivalentTo(expectResult);
        }

        [TestMethod]
        [Ignore]
        public void TRN1001_因Request內容有誤_7_回傳失敗()
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
        public void TRN1001_存簿轉存簿_回傳成功()
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
        public void TRN1001_存簿轉劃撥_回傳成功()
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
        public void TRN1001_存簿轉他行_回傳成功()
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
        public void TRN1001_劃撥轉存簿_回傳成功()
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
        public void TRN1001_劃撥轉劃撥_回傳成功()
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
