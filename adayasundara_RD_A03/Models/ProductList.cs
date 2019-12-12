/*
*	FILE			:	ProductList.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    Retains the product list
*/
#region Systems
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion Systems

namespace adayasundara_RD_A03.Models
{
    /*
    * NAME: Product List
    * 
    * PURPOSE: Retrieve and set the product list
    * 
    */

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
