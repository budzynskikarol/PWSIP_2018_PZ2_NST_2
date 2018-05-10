using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class Kategorie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
    }
}