/*
*	FILE			:	AddToCart.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                   This class controls the cart information and will hold until the user is ready to check out
*/


using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    /*
    * NAME: AddToCart 
    * 
    * PURPOSE: Add to the cart section to be later used
    * 
    */
    public class AddToCart : ObservableObject
    {
        /// <summary>
        ///     Adds information of the cart 
        /// </summary>
        /// <param name="SKU"> Product SKU </param>
        /// <param name="name"> Product Name </param>
        /// <param name="sPrice"> Sales Price </param>
        /// <param name="quantity"> Quantity of the Product </param>
        /// <param name="prodType"> Type of Products </param>
        public AddToCart(int SKU, string name, decimal sPrice, int quantity, string prodType)
        {
            set(SKU, name, sPrice, quantity, prodType);
        }

        public int SKU { get; set; }
        public string name { get; set; }
        public decimal sPrice { get; set; }
        public int quantity { get; set; }
        public string prodType { get; set; }

        /// <summary>
        ///     Sets the cart list 
        /// </summary>
        /// <param name="setSKU"></param>
        /// <param name="setName"></param>
        /// <param name="setPrice"></param>
        /// <param name="setQuantity"></param>
        /// <param name="setProdType"></param>
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
