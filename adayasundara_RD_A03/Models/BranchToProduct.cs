/*
*	FILE			:	BranchToProducts.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    Retains the associative for the branch to products
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
     * NAME: Branch To Product
     * 
     * PURPOSE: Products available per each branch
     * 
     */

    public class BranchToProduct
    {
        public static List<BranchToProduct> branchToProducts { get; set; }
        public int lSKU { get; set; }
        public int lBranchId { get; set; }
    }
}
