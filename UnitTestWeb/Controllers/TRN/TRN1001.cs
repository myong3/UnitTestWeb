using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestWeb.Extensions;
using UnitTestWeb.Models;

namespace UnitTestWeb.Controllers.TRN
{
    public partial class TRNController
    {
        /// <summary>
        /// [TRN1001]立即轉帳(非約定)
        /// type：
        /// 1：存簿轉存簿(非約定轉帳)
        /// 2：存簿轉劃撥(非約定轉帳)
        /// 3：存簿轉他行(非約定轉帳)
        /// 4：劃撥轉存簿(非約定轉帳)
        /// 5：劃撥轉劃撥(非約定轉帳)
        /// </summary>
        /// <param name="myReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TRN1001")]
        public TRN1001Res TRN1001(TRN1001Req myreq)
        {
            var rtn = new TRN1001Res();
            try
            {
                #region Request檢核

                bool checkRequest = true;
                var errMsg = string.Empty;

                if (myreq == null)
                {
                    checkRequest = false;
                    errMsg = "參數錯誤:Request=Null";
                }
                else
                {
                    if (myreq.type.IsNullOrWhiteSpace() || !CheckType_TRN1001(myreq.type))
                    {
                        checkRequest = false;
                        errMsg += errMsg.IsNullOrWhiteSpace() ? $"參數錯誤:type:\'{myreq.type}\'" : $",type:\'{myreq.type}\'";
                    }

                    if (myreq.actNo.IsNullOrWhiteSpace())
                    {
                        checkRequest = false;
                        errMsg += errMsg.IsNullOrWhiteSpace() ? $"參數錯誤:actNo:\'{myreq.actNo}\'" : $",actNo:\'{myreq.actNo}\'";
                    }

                    if (myreq.type == "3" && myreq.outBankNo.IsNullOrWhiteSpace())
                    {
                        checkRequest = false;
                        errMsg += errMsg.IsNullOrWhiteSpace() ? $"參數錯誤:outBankNo:\'{myreq.outBankNo}\'" : $",outBankNo:\'{myreq.outBankNo}\'";
                    }

                    if (myreq.outActNo.IsNullOrWhiteSpace())
                    {
                        checkRequest = false;
                        errMsg += errMsg.IsNullOrWhiteSpace() ? $"參數錯誤:outActNo:\'{myreq.outActNo}\'" : $",outActNo:\'{myreq.outActNo}\'";
                    }

                    if (myreq.txAmt == 0)
                    {
                        checkRequest = false;
                        errMsg += errMsg.IsNullOrWhiteSpace() ? $"參數錯誤:txAmt: {myreq.txAmt}(不得為0)" : $",txAmt: {myreq.txAmt}(不得為0)";
                    }

                    if (myreq.txAmt > 999999999)
                    {
                        checkRequest = false;
                        errMsg += errMsg.IsNullOrWhiteSpace() ? $"參數錯誤:txAmt: {myreq.txAmt}(提款/轉帳超過每次限額)" : $",txAmt: {myreq.txAmt}(提款/轉帳超過每次限額)";
                    }
                }

                #endregion Request檢核

                if (!checkRequest)
                {
                    var message = new MessageModel()
                    {
                        responseCode = "98",
                        errMsg = errMsg
                    };
                    rtn.SetMessage(message);
                    return rtn;
                }
                else
                {
                    #region Excute BlueStar

                    switch (myreq.type)
                    {
                        case "1":
                            rtn = PSToPS_TRN1001(myreq);
                            break;

                        case "2":
                            rtn = PSToGR_TRN1001(myreq);
                            break;

                        case "3":
                            rtn = PSToBank_TRN1001(myreq);
                            break;

                        case "4":
                            rtn = GRToPS_TRN1001(myreq);
                            break;

                        case "5":
                            rtn = GRToGR_TRN1001(myreq);
                            break;

                        default:
                            break;
                    }

                    #endregion
                }

                return rtn;
            }
            catch (Exception ex)
            {
                return rtn;
            }
        }

        /// <summary>
        /// 執行存簿轉存簿(非約定轉帳)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private TRN1001Res PSToPS_TRN1001(TRN1001Req data)
        {
            var result = new TRN1001Res();

            var msgId = "P0E460";

            if (data.outActNo.PadLeft(14, '0').Length != 14)
            {
                return result;
            }

            result = new TRN1001Res()
            {
                traceNo = "",
                fromAccountNo = "",
                time = "",
                beforePbaBal = "",
                afterPbaBal = "",
                trfFee = "",
                unprintCnt = "",
                responseCode = "0",
                errMsg = string.Empty
            };

            #region Send Email
#if !DEBUG
            var nickName = string.IsNullOrWhiteSpace(data.nickName) ? string.Empty : "/" + data.nickName.NickNameCover();
            var mailContentModel = new BuildEmailContentModel()
            {
                type = "存簿轉存簿(非約定)",
                accountOut = data.actNo.AccountCover(),
                bankName = "中華郵政股份有限公司",
                accountIn = data.outActNo.AccountCover() + nickName,
                traceNo = tRACE_NO,
                commentIn = data.commentIn,
                commentOut = data.commentOut,
                finishTime = MailFinishTimeformat_TRN1001(result.time)
            };

            data.txAmt.ToString().Decimal_TryThousandsSeparator(0, out string mailTxAmt);
            tRF_FEE.Decimal_TryThousandsSeparator(0, out string mailFee);

            mailContentModel.amt = mailTxAmt;
            mailContentModel.fee = mailFee;

            var mailContent = BuildEmailContent(mailContentModel, true);
            var receivers = new List<string>();
            receivers.Add(req._EMAIL.ToString());
            await _emailService.SendEmailAsync(receivers, _projectChineseName + "交易通知", mailContent);
#endif
            #endregion

            return result;
        }

        /// <summary>
        /// 執行存簿轉劃撥(非約定轉帳)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private TRN1001Res PSToGR_TRN1001(TRN1001Req data)
        {
            var result = new TRN1001Res();

            var msgId = "P0E461";

            result = new TRN1001Res()
            {
                traceNo = "",
                fromAccountNo = "",
                time = "",
                beforePbaBal = "",
                afterPbaBal = "",
                trfFee = "",
                unprintCnt = "",
                responseCode = "0",
                errMsg = string.Empty
            };

            #region Send Email
#if !DEBUG
            var nickName = string.IsNullOrWhiteSpace(data.nickName) ? string.Empty : "/" + data.nickName.NickNameCover();
            var mailContentModel = new BuildEmailContentModel()
            {
                type = "存簿轉劃撥(非約定)",
                accountOut = data.actNo.AccountCover(),
                bankName = "中華郵政股份有限公司",
                accountIn = data.outActNo.AccountCover() + nickName,
                traceNo = tRACE_NO,
                commentIn = data.commentIn,
                commentOut = data.commentOut,
                finishTime = MailFinishTimeformat_TRN1001(result.time)
            };

            data.txAmt.ToString().Decimal_TryThousandsSeparator(0, out string mailTxAmt);
            tRF_FEE.Decimal_TryThousandsSeparator(0, out string mailFee);

            mailContentModel.amt = mailTxAmt;
            mailContentModel.fee = mailFee;

            var mailContent = BuildEmailContent(mailContentModel, true);
            var receivers = new List<string>();
            receivers.Add(req._EMAIL.ToString());
            await _emailService.SendEmailAsync(receivers, _projectChineseName + "交易通知", mailContent);
#endif
            #endregion

            return result;
        }

        /// <summary>
        /// 執行存簿轉他行(非約定轉帳)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private TRN1001Res PSToBank_TRN1001(TRN1001Req data)
        {
            var result = new TRN1001Res();

            var msgId = "YCB471A";


            var cALL_CENT_ACT_NO = string.Empty;
            var nICKNAME_12 = string.Empty;
            if (data.commentIn?.Length > 5)
            {
                cALL_CENT_ACT_NO = data.commentIn.Substring(0, 5);
                nICKNAME_12 = data.commentIn.Substring(5, data.commentIn.Length - 5);
            }
            else if (data.commentIn?.Length <= 5)
            {
                cALL_CENT_ACT_NO = data.commentIn;
            }




            result = new TRN1001Res()
            {
                traceNo = "",
                fromAccountNo = "",
                time = "",
                beforePbaBal = "",
                afterPbaBal = "",
                trfFee = "",
                unprintCnt = "",
                responseCode = "0",
                errMsg = string.Empty
            };


            #region Send Email
#if !DEBUG
            var nickName = string.IsNullOrWhiteSpace(data.nickName) ? string.Empty : "/" + data.nickName.NickNameCover();
            var bnkName = string.IsNullOrWhiteSpace(data.bnkName) ? string.Empty : "/" + data.bnkName;

            var mailContentModel = new BuildEmailContentModel()
            {
                type = "存簿跨行(非約定)",
                accountOut = data.actNo.AccountCover(),
                bankName = data.outBankNo + bnkName,
                accountIn = data.outActNo.AccountCover() + nickName,
                traceNo = slip_From_AccountNo,
                finishTime = MailFinishTimeformat_TRN1002(result.time)
            };

            slip_TxAmount.Decimal_TryThousandsSeparator(0, out string mailTxAmt);
            tRF_FEE.Decimal_TryThousandsSeparator(0, out string mailFee);

            mailContentModel.amt = mailTxAmt;
            mailContentModel.fee = mailFee;

            var mailContent = BuildEmailContent(mailContentModel, true, true);
            var receivers = new List<string>();
            receivers.Add(req._EMAIL.ToString());

            await _emailService.SendEmailAsync(receivers, _projectChineseName + "交易通知", mailContent);
#endif
            #endregion

            return result;
        }

        /// <summary>
        /// 執行劃撥轉存簿(非約定轉帳)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private TRN1001Res GRToPS_TRN1001(TRN1001Req data)
        {
            var result = new TRN1001Res();

            var msgId = "G00475";

            #region 準備BlueStar參數
            if (data.outActNo.PadLeft(14, '0').Length != 14)
            {
                return result;
            }



            result = new TRN1001Res()
            {
                traceNo = "",
                fromAccountNo = "",
                time = "",
                beforePbaBal = "",
                afterPbaBal = "",
                trfFee = "",
                unprintCnt = "",
                responseCode = "0",
                errMsg = string.Empty
            };
            #endregion

            #region Send Email
#if !DEBUG
            var nickName = string.IsNullOrWhiteSpace(data.nickName) ? string.Empty : "/" + data.nickName.NickNameCover();
            var mailContentModel = new BuildEmailContentModel()
            {
                type = "劃撥轉存簿(非約定)",
                accountOut = fullGRActNo.Substring(6, 8).AccountCover(),
                bankName = "中華郵政股份有限公司",
                accountIn = data.outActNo.AccountCover() + nickName,
                traceNo = tRACE_NO,
                commentIn = data.commentIn,
                commentOut = data.commentOut,
                finishTime = MailFinishTimeformat_TRN1001(result.time)
            };

            data.txAmt.ToString().Decimal_TryThousandsSeparator(0, out string mailTxAmt);
            cHRG.Decimal_TryThousandsSeparator(0, out string mailFee);

            mailContentModel.amt = mailTxAmt;
            mailContentModel.fee = mailFee;

            var mailContent = BuildEmailContent(mailContentModel, false);
            var receivers = new List<string>();
            receivers.Add(req._EMAIL.ToString());

            await _emailService.SendEmailAsync(receivers, _projectChineseName + "交易通知", mailContent);
#endif
            #endregion

            return result;
        }

        /// <summary>
        /// 執行劃撥轉劃撥(非約定轉帳)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private TRN1001Res GRToGR_TRN1001(TRN1001Req data)
        {
            var result = new TRN1001Res();

            var msgId = "G00476";



            result = new TRN1001Res()
            {
                traceNo = "",
                fromAccountNo = "",
                time = "",
                beforePbaBal = "",
                afterPbaBal = "",
                trfFee = "",
                unprintCnt = "",
                responseCode = "0",
                errMsg = string.Empty
            };

            #region Send Email
#if !DEBUG
            var nickName = string.IsNullOrWhiteSpace(data.nickName) ? string.Empty : "/" + data.nickName.NickNameCover();
            var mailContentModel = new BuildEmailContentModel()
            {
                type = "劃撥轉劃撥(非約定)",
                accountOut = fullGRActNo.Substring(6, 8).AccountCover(),
                bankName = "中華郵政股份有限公司",
                accountIn = data.outActNo.AccountCover() + nickName,
                traceNo = tRACE_NO,
                finishTime = MailFinishTimeformat_TRN1001(result.time)
            };

            data.txAmt.ToString().Decimal_TryThousandsSeparator(0, out string mailTxAmt);
            cHRG.Decimal_TryThousandsSeparator(0, out string mailFee);

            mailContentModel.amt = mailTxAmt;
            mailContentModel.fee = mailFee;

            var mailContent = BuildEmailContent(mailContentModel, false);
            var receivers = new List<string>();
            receivers.Add(req._EMAIL.ToString());

            await _emailService.SendEmailAsync(receivers, _projectChineseName + "交易通知", mailContent);
#endif
            #endregion

            return result;
        }

        /// <summary>
        /// 驗證request.type是否符合規定
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        private bool CheckType_TRN1001(string type)
        {
            var result = false;
            if (type == "1" || type == "2" || type == "3" || type == "4" || type == "5")
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 修改日期時間格式
        /// 110/06/17 18:01:12 -> 110年06月17日18時01分12秒
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private string MailFinishTimeformat_TRN1001(string datetime)
        {
            var result = string.Empty;
            var year = datetime.Substring(0, 3);
            var month = datetime.Substring(4, 2);
            var date = datetime.Substring(7, 2);
            var hour = datetime.Substring(10, 2);
            var minute = datetime.Substring(13, 2);
            var second = datetime.Substring(16, 2);

            result = $"{year}年{month}月{date}日{hour}時{minute}分{second}秒";

            return result;
        }
    }
}
