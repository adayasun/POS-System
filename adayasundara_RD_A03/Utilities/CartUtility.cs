using adayasundara_RD_A03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.Utilities
{
    public class CartUtility : ObservableObject
    {
        public static List<AddToCart> AddToCarts { get; set; }
    }
}
