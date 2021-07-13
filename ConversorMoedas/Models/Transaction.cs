using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConversorMoedas.Models
{
    public class Transaction
    {
        [Key]
        public int IdTransaction { get; set; }
        [Required]
        [DisplayName("ID User")]
        public string IdUser { get; set; }
        [Required]
        [DisplayName("Origin Currency")]
        public string OriginCoin { get; set; }
        [Required]
        [DisplayName("Origin Value")]
        [DisplayFormat(DataFormatString = "{0:#,##0.0000}", ApplyFormatInEditMode = true)]
        public decimal OriginValue { get; set; }
        [Required]
        [DisplayName("Destiny Currency")]
        public string DestinyCoin { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:#,##0.0000}", ApplyFormatInEditMode = true)]
        [DisplayName("Conversion Rate")]
        public decimal ConversionRate { get; set; }

        [DisplayName("Date/Time UTC")]
        public DateTime DateTimeUTC { get; set; }
         
        [DisplayFormat(DataFormatString = "{0:#,##0.0000}", ApplyFormatInEditMode = true)]
        [DisplayName("Destiny Value")]
        public decimal DestinyValue {
            get 
            {
                return (OriginValue* ConversionRate);
            }
        }
    }
}