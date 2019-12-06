using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    [Serializable]
    public class CustomerInfo : ObservableObject
    {
        public static List<CustomerInfo> customers { get; set; }
        public static int ChosenCustomer { get; set; }
        public int custID { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phoneNumber { get; set; }


    }
}
