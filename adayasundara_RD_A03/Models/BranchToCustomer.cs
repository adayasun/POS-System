/*
*	FILE			:	BranchToCustomer.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    Retains the associatives of the Branch to customer.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    [Serializable]
    class BranchToCustomer
    {
        public static List<BranchToCustomer> branchToCustomers { get; set; }
        public int lCustId { get; set; }
        public int lBranchId { get; set; }
    }
}
