using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestWeb.Models
{
    public class TRN1001Req
    { /// <summary>
      /// 類別
      /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 轉出帳號
        /// </summary>
        public string actNo { get; set; }
        /// <summary>
        /// 轉入銀行代碼
        /// </summary>
        public string outBankNo { get; set; }
        /// <summary>
        /// 轉入銀行名稱
        /// </summary>
        public string bnkName { get; set; }
        /// <summary>
        /// 轉入帳號
        /// </summary>
        public string outActNo { get; set; }
        /// <summary>
        /// 轉入戶別名
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 轉帳金額
        /// </summary>
        public decimal txAmt { get; set; }
        /// <summary>
        /// 轉入戶印摺註記
        /// </summary>
        public string txType { get; set; }
        /// <summary>
        /// 轉出戶附言欄(自己備註)
        /// </summary>
        public string commentIn { get; set; }
        /// <summary>
        /// 轉入戶附言欄(對方備註)
        /// </summary>
        public string commentOut { get; set; }
        /// <summary>
        /// authResponses
        /// </summary>
        public string authResponses { get; set; }
        /// <summary>
        /// 交易密碼
        /// </summary>
        public string fipsPWD { get; set; }
    }
}

