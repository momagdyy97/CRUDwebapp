using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDwebapp.Models
{
    public class itemsDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // 2015-11-17
        public DateTime Date { get; set; }
        public string String { get; set; }
        public string ImageBase64 { get; set; }
    }
}
