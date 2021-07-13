using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConversorMoedas.Models
{
    public class APITransactionError
    {
        public APITransactionErrorMessage Error { get; set; }
    }

    public class APITransactionErrorMessage
    {
        public string code { get; set; }
        public string message { get; set; }
        public string type { get; set; }
    }
}