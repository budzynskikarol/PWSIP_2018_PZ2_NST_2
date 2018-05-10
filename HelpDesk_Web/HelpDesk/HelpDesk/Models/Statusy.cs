using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class Statusy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
    }
}