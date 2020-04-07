/*
*	FILE			:	Orders.xaml.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This manages the orders behind the code. It implements the orders 
*	                    being processed into the database.
*/
#region Systems
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
#endregion Systems

namespace adayasundara_RD_A03.Views
{
    /*
     * NAME: Orders 
     * 
     * PURPOSE: Behind code for order creation
     * 
     */
    public partial class Orders : UserControl
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;
        List<ProductList> products = new List<ProductList>();

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
            showQuantity.Text = aQuantity.ToString();
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
            string prodType = null;
            int noQuantity = 0;
            try
            {
                insertQuantity = Int32.Parse(selectedQuantity.Text); //Check Quantity
                availableQty = Int32.Parse(showQuantity.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (dateSelected.SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
            }
            else if(insertQuantity <= availableQty || insertQuantity >= noQuantity)
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
            int currentBranch = Branch.ChosenBranchID;
            string dateTime = dateSelected.SelectedDate.Value.ToString("yyyy-MM-dd");
            string purchased;
            string status = "PAID";
            string totalPurchaseLine = "\n";
            string stringOrderId = null;
            decimal subtotal = 0;
            string stringSubtotal = null;
            string stringWithHST = null;
            string stringWithSalesTotal = null;

            //Needed variable
            int orderId = 0;
            int orderLineId = 0;

            //USING 1
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //INSERT INTO ORDER
                    string query = $@"INSERT INTO orders(Cust_ID, Branch_ID, OrderDate)
                                        VALUES('{currentCustomer}','{currentBranch}','{dateTime}')";

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
                    stringOrderId = orderId.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "using 1");
                }
            }

            foreach (DataRowView row in cartList.ItemsSource)
            {
                int insertSku = ProductList.products.Where(line => line.prodName == (row[0].ToString())).Select(l => l.SKU).Sum();
                int insertStock = ProductList.products.Where(line => line.prodName == (row[0].ToString())).Select(l => l.stock).Sum();

                string stringProductName = null;
                string stringProdType = null;
                string stringQuantity = null;
                string stringSPrice = null;
                string stringFinalPrice = null;

                stringProductName = row[0].ToString();
                stringProdType = row[1].ToString();
                stringQuantity = row[2].ToString();
                stringSPrice = row[3].ToString();

                int quantity = Int32.Parse(row[2].ToString());
                decimal price = decimal.Parse(row[3].ToString());
                decimal extended = price * quantity;
                stringFinalPrice = extended.ToString();

                totalPurchaseLine += stringProductName + " (" + stringProdType +") " + stringQuantity + " x $" + stringSPrice + " = $" + stringFinalPrice + "\n";
                subtotal += extended;
                //USING 2
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        //Retireve values
                        //int quantity = Int32.Parse(row[2].ToString());
                        //decimal price = decimal.Parse(row[3].ToString());
                        //decimal extended = price * quantity;
                        //stringFinalPrice = extended.ToString();
                        int adjustedInv = insertStock - quantity;
                        string paid = "PAID";
                        connection.Open();
                        string query = null;

                        //INSERT ORDER_ID, Price, Quantity, extended INTO ORDERLINE
                        query = $@"INSERT INTO order_line(Order_ID, sPrice, Quantity, Extended_Price, PaymentStatus)
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
                        connection.Close();

                        connection.Open();

                        //ADJUST PRODUCT QTY
                        query = $@"UPDATE products 
                                    SET Stock = '{adjustedInv}'
                                    WHERE SKU ='{insertSku}';";
                        createCommand = new MySqlCommand(query, connection);
                        createCommand.ExecuteNonQuery();


                        connection.Close();
                        showQuantity.Text = "";
                        showQuantity.Text = adjustedInv.ToString();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex);
                    }
                }

            }
            decimal withHST = subtotal * 0.13M;
            decimal salesTotal = subtotal + withHST;
            stringWithHST = withHST.ToString();
            stringWithSalesTotal = salesTotal.ToString();
            stringSubtotal = subtotal.ToString();

            UpdateProductList();
            ShowReciept(orderId, totalPurchaseLine, stringOrderId, stringWithSalesTotal, stringWithHST, stringSubtotal);
            cartList.ItemsSource = null;
        }

        private void ShowReciept(int orderId, string totalPurchaseLine, string stringOrderId, string stringWithSalesTotal, string stringWithHST, string stringSubtotal)
        {
            string customerName = null;
            string fName = null;
            string lName = null;
            string orderIdstr = orderId.ToString();
            string branchName = null;
            int branchId = Branch.ChosenBranchID;
            int customerId = CustomerInfo.ChosenCustomer;
            string date = dateSelected.ToString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT * FROM branch WHERE Branch_ID = '{branchId}';";
                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        branchName = datareader["Branch_Name"] as string;
                    }

                    connection.Close();

                    connection.Open();
                    query = $@"SELECT * FROM customer WHERE Cust_ID = '{customerId}';";
                    createCommand = new MySqlCommand(query, connection);
                    datareader = createCommand.ExecuteReader();

                    while (datareader.Read())
                    {
                        fName = datareader["FName"] as string;
                        lName = datareader["LName"] as string;
                    }
                    customerName = fName + " " + lName;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //Need Date
            //List of purchased items
            //Subtotal
            //HST
            //Sales total

            MessageBox.Show("************************" + "\n" + 
                "Thank you for shopping at" + "\n" + 
                "Wally's World " + $"{ branchName}" + "\n" + 
                "On " + date + $" {customerName}" + "\n" + 
                "Order ID: " + stringOrderId + "\n" + 
                totalPurchaseLine + "\n" +
                "Subtotal = $" + stringSubtotal + "\n" +
                "HST (13%) = $" + stringWithHST + "\n" +
                "Sale Total = $" + stringWithSalesTotal + "\n" +
                 "\n" + "Paid - Thank you!");
        }

        private void UpdateProductList()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string customerQuery = "SELECT * FROM products";
                    MySqlCommand createCommand = new MySqlCommand(customerQuery, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        products.Add(new ProductList()
                        {
                            SKU = ((int)datareader["SKU"]),
                            prodName = datareader["Product_Name"] as string,
                            wPrice = ((decimal)datareader["wPrice"]),
                            stock = ((int)datareader["Stock"]),
                            prodType = datareader["ProdType"] as string
                        });

                    }
                    connection.Close();

                    ProductList.products = products;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            int branchId = Branch.ChosenBranchID;

            foreach (int id in GetBranchToProdId(branchId))
            {
                foreach (string name in GetProductById(id))
                {
                    productsAvailable.Items.Add(name);
                }
            }

        }
    }
}
