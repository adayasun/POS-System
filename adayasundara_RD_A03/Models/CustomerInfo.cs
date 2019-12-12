/*
*	FILE			:	CustomerInfo.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    Retains the cusatomers that were add to the database
*/
#region Systems
using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion Systems

namespace adayasundara_RD_A03.Models
{
    [Serializable]
    /*
     * NAME: Customer Info
     * 
     * PURPOSE: To have all of the customer info. As well to set the current customer that is 
     *          being viewed
     * 
     */
    public class CustomerInfo : ObservableObject
    {   
        //Current Customer ID
        public static List<CustomerInfo> customers { get; set; }
       //Current customer selected
        public static int ChosenCustomer { get; set; }
        public int custID { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phoneNumber { get; set; }


    }
}
