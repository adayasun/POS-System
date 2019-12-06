using adayasundara_RD_A03.Models;
using adayasundara_RD_A03.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;

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

            //Setup Product Type
            productType = ProductList.products.Where(line => line.prodName == product).Select(l => l.prodType).First();
            pType.Text = productType.ToString();
            //Setup available quantity
            aQuantity = ProductList.products.Where(line => line.prodName == product).Select(l => l.stock).Sum();
            quantity.Text = aQuantity.ToString();
        }


        public void Button_Click(object sender, RoutedEventArgs e)
        {
            int insertSku = 0;
            int availableQty = 0;
            string insertName = null;
            decimal insertSPrice = 0;
            int insertQuantity = 0;
            string insertProdType = null;
            decimal wPrice = 0;
            double markUp = 1.40;

            insertQuantity = Int32.Parse(selectedQuantity.Text); //Check Quantity
            availableQty = Int32.Parse(quantity.Text);

            if(dateSelected.SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
            }
            else if(insertQuantity <= availableQty)
            {
                insertName = productsAvailable.SelectedItem.ToString();
                insertSku = ProductList.products.Where(line => line.prodName == insertName).Select(l => l.SKU).Sum();
                wPrice = ProductList.products.Where(line => line.prodName == insertName).Select(l => l.wPrice).Sum();
                insertSPrice = wPrice * (decimal)markUp;
                insertProdType = ProductList.products.Where(line => line.prodName == insertName).Select(l => l.prodType).First();

                AddToCart newItem = new AddToCart(insertSku, insertName, insertSPrice, insertQuantity, insertProdType);
                CartUtility.AddToCarts.Add(newItem);

                CartUtility.StartExport();
                
                cartList.ItemsSource = CartUtility.cartItems.DefaultView;
            }
            else
            {
                MessageBox.Show("Please enter an quantity less than or equal to the available quantity.");
            }
            selectedQuantity.Text = "";
        }

        private void checkout_Click(object sender, RoutedEventArgs e)
        {
            int currentCustomer = CustomerInfo.ChosenCustomer;
            string dateTime = dateSelected.SelectedDate.Value.ToString("yyyy-MM-dd");

            string status = "PAID";

            //Needed variable
            int orderId = 0;
            int orderLineId = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //INSERT INTO ORDER
                    string query = $@"INSERT INTO orders(Cust_ID, OrderDate)
                                        VALUES('{currentCustomer}','{dateTime}')";

                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();

                    //RETIREVE ORDER_ID

                    query = $@"SELECT * 
                                    FROM orders 
                                    WHERE Cust_ID = '{currentCustomer}'
                                    AND OrderDate ='{dateTime}';";
                    createCommand = new MySqlCommand(query, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        orderId = ((int)datareader["Order_ID"]);
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            foreach (DataRowView row in cartList.ItemsSource)
            {
                int insertSku = ProductList.products.Where(line => line.prodName == (row[0].ToString())).Select(l => l.SKU).Sum();
                int insertStock = ProductList.products.Where(line => line.prodName == (row[0].ToString())).Select(l => l.stock).Sum();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        //Retireve values
                        int quantity = Int32.Parse(row[1].ToString());
                        decimal price = decimal.Parse(row[2].ToString());
                        decimal extended = price * quantity;
                        int adjustedInv = insertStock - quantity;
                        string paid = "PAID";
                        connection.Open();
                        //INSERT ORDER_ID, Price, Quantity, extended INTO ORDERLINE
                        string query = $@"INSERT INTO order_line(Order_ID, sPrice, Quantity, Extended_Price, PaymentStatus)
                                          VALUES ('{orderId}','{price}','{quantity}','{extended}', '{paid}');";

                        MySqlCommand createCommand = new MySqlCommand(query, connection);
                        createCommand.ExecuteNonQuery();
                        //RETRIEVE ORDERLINE_ID where ORDER_ID, Price, Quantity, extended
                        query = $@"SELECT * FROM order_line
                                   WHERE Order_ID = '{orderId}' 
                                   AND sPrice = '{price}'
                                   AND Quantity = '{quantity}'
                                   AND Extended_Price= '{extended}';";
                        createCommand = new MySqlCommand(query, connection);
                        MySqlDataReader datareader = createCommand.ExecuteReader();
                        while (datareader.Read())
                        {
                            orderLineId = ((int)datareader["Order_Line_ID"]);
                        }
                        connection.Close();

                        connection.Open();
                        //ORDERLINE_ID TO SKU
                        query = $@"INSERT INTO orderline_prod(Order_Line_ID, SKU)
                                    VALUES('{orderLineId}','{insertSku}');";
                        createCommand = new MySqlCommand(query, connection);
                        createCommand.ExecuteNonQuery();

                        //ADJUST PRODUCT QTY
                        query = $@"UPDATE products 
                                    SET Stock = '{adjustedInv}'
                                    WHERE SKU ='{insertSku}';";
                        createCommand = new MySqlCommand(query, connection);
                        createCommand.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                
            }

            cartList.ItemsSource = null;
        }

    }
}
