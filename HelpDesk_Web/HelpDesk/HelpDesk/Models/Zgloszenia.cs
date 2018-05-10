using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class Zgloszenia
    {
        [Key]
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public string Komentarz { get; set; }
        public Statusy Statusy { get; set; }
        public int StatusyId { get; set; }
        public Kategorie Kategorie { get; set; }
        public int KategorieId { get; set; }
        public string Uzytkownik { get; set; }
    }
}