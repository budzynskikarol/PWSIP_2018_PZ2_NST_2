using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class ZgloszeniaViewModels
    {
        public Zgloszenia Zgloszenia { get; set; }
        public List<Wiadomosci> Wiadomosci { get; set; }

        public string Wiad { get; set; }
        public int idwiad { get; set; }
        public string nad { get; set; }
    }
}