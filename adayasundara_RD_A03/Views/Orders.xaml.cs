using adayasundara_RD_A03.Models;
using adayasundara_RD_A03.Utilities;
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


        private void productsAvailable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decimal wPrice = 0;
            double markUp = 1.40;
            decimal sPrice = 0;
            int aQuantity = 0;
            string productType = null;

            //Setup Price per unit
            string product = productsAvailable.SelectedItem.ToString();
            wPrice = ProductList.products.Where(line => line.prodName == product).Select(l => l.wPrice).Sum();
            sPrice = wPrice * (decimal)markUp;
            price.Text = sPrice.ToString("0.00");

            //Setup Product Type CHECK WHY IT DOES THAT ENUM BITCH THING
            productType = ProductList.products.Where(line => line.prodName == product).Select(l => l.prodType).ToString();
            pType.Text = productType.ToString();
            //Setup available quantity
            aQuantity = ProductList.products.Where(line => line.prodName == product).Select(l => l.stock).Sum();
            quantity.Text = aQuantity.ToString();
        }

        private void selectedQuantity_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            int insertSku = 0;
            string insertName = null;
            decimal insertSPrice = 0;
            int insertQuantity = 0;
            string insertProdType = null;
            decimal wPrice = 0;
            double markUp = 1.40;

            insertName = productsAvailable.SelectedItem.ToString();
            insertSku = ProductList.products.Where(line => line.prodName == insertName).Select(l => l.SKU).Sum();
            wPrice = ProductList.products.Where(line => line.prodName == insertName).Select(l => l.wPrice).Sum();
            insertSPrice = wPrice * (decimal)markUp;
            insertQuantity = Int32.Parse(selectedQuantity.Text);
            insertProdType = ProductList.products.Where(line => line.prodName == insertName).Select(l => l.prodType).ToString();

            AddToCart newItem = new AddToCart(insertSku, insertName, insertSPrice, insertQuantity, insertProdType);
            CartUtility.AddToCarts.Add(newItem);

            
            ClearQuantity();
        }

        private void ClearQuantity()
        {
            selectedQuantity.Text = "";
        }
    }
}
