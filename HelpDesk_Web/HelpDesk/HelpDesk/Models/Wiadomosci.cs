using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class Wiadomosci
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Tresc { get; set; }
        public string Nadawca { get; set; }
        public Zgloszenia Zgloszenia { get; set; }
        public int ZgloszeniaId { get; set; }
        public DateTime DataDodania { get; set; }
    }
}