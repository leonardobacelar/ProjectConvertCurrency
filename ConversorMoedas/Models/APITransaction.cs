using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConversorMoedas.Models
{
    public class APITransaction
    {  
        public bool Success { get; set; }
        public string Timestamp { get; set; }
        public string Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }

     
    

}