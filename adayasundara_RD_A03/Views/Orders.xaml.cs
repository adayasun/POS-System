using adayasundara_RD_A03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace adayasundara_RD_A03.Views
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        public Orders()
        {
            InitializeComponent();

        }

        private void productsAvailable_Loaded(object sender, RoutedEventArgs e)
        {
            productsAvailable.Items.Clear();
            productsAvailable.Text = "-- SELECT A PRODUCT --";
            int branchId = Branch.ChosenBranchID;

            foreach(int id in GetBranchToProdId(branchId))
            {
                foreach(string name in GetProductById(id))
                {
                    productsAvailable.Items.Add(name);
                }
            }
        }

        private int[] GetBranchToProdId(int id)
        {
            return BranchToProduct.branchToProducts.Where(line => line.lBranchId == id).Select(l => (l.lSKU)).ToArray();
        }
        
        private string[] GetProductById(int id)
        {
            return ProductList.products.Where(line => line.SKU == id).Select(l => (l.prodName)).ToArray();
        }
    }
}
