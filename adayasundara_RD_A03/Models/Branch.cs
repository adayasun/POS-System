/*
*	FILE			:	Branch.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    Controls the branches list information
*/

using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    [Serializable]
    public class Branch : ObservableObject
    {
        //Current Branch ID
        public static int ChosenBranchID { get; set; }
        public static List<Branch> branches { get; set; }
        public int branchID { get; set; }
        public string branchName { get; set; }

    }
}
