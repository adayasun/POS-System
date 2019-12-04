using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    public class ProductList
    {
        public static List<ProductList> products { get; set; }
        public int SKU { get; set; }
        public string prodName { get; set; }
        public decimal wPrice { get; set; }
        public int stock { get; set; }
        public string prodType { get; set; }
    }
}
