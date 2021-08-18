using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestWeb.Controllers.TRN
{
    public partial class TRNController
    {
        [HttpPost]
        [Route("TRN1002")]
        public string TRN1002(bool isTrue)
        {
            var result = string.Empty;
            if (isTrue)
            {
                result = "TRN1002";
            }
            else
            {
                result = "trn1002";
            }
            return result;
        }
    }
}
