using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Models
{
    public class BranchToProduct
    {
        public static List<BranchToProduct> branchToProducts { get; set; }
        public int lSKU { get; set; }
        public int lBranchId { get; set; }
    }
}
