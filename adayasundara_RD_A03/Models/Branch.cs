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
        public static int ChosenBranchID { get; set; }
        public static List<Branch> branches { get; set; }
        public int branchID { get; set; }
        public string branchName { get; set; }

    }
}
