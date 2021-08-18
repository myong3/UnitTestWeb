using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestWeb.Models
{
    public class TRN1001Res : MessageModel
    {
        /// <summary>
        /// TRACE NO
        /// </summary>
        public string traceNo { get; set; }
        /// <summary>
        /// 跨行交易序號
        /// </summary>
        public string fromAccountNo { get; set; }
        /// <summary>
        /// 主機時間
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 轉帳前存簿結存
        /// </summary>
        public string beforePbaBal { get; set; }
        /// <summary>
        /// 轉帳後存簿結存
        /// </summary>
        public string afterPbaBal { get; set; }
        /// <summary>
        /// 轉帳手續費
        /// </summary>
        public string trfFee { get; set; }
        /// <summary>
        /// 未登摺記號
        /// </summary>
        public string unprintCnt { get; set; }
    }
}

