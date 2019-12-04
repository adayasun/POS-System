using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    [Serializable]
    class Customer
    {
        public static List<Customer> customers { get; set; }
        public int custID { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phoneNumber { get; set; }
    }
}
