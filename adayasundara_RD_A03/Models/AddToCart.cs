using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    public class AddToCart : ObservableObject
    {
        public AddToCart(int SKU, string name, decimal sPrice, int quantity, string prodType)
        {
            set(SKU, name, sPrice, quantity, prodType);
        }

        public int SKU { get; set; }
        public string name { get; set; }
        public decimal sPrice { get; set; }
        public int quantity { get; set; }
        public string prodType { get; set; }

        public void set(int setSKU, string setName, decimal setPrice, int setQuantity, string setProdType)
        {
            SKU = setSKU;
            name = setName;
            sPrice = setPrice;
            quantity = setQuantity;
            prodType = setProdType;
        }
    }
}
