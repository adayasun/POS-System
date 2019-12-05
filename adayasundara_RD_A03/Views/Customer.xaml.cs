using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data;
using adayasundara_RD_A03.Models;

namespace adayasundara_RD_A03.Views
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : UserControl
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;

        public Customer()
        {
            InitializeComponent();
        }

        private void ordersInfo_Loaded(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string customer = CustomerInfo.ChosenCustomer.ToString();
                    connection.Open();
                    string query = $@"SELECT 
                                                    o.Order_ID as 'Order ID', 
                                                    p.Product_Name as 'Product Name', 
                                                    ol.Quantity as 'Quantity', 
                                                    o.OrderDate as 'Order Date', 
                                                    o.PaymentStatus as 'Payment Status' 
                                            FROM order_line as ol 
                                                join orderline_prod as op on op.Order_Line_ID = ol.Order_Line_ID 
                                                join products as p on p.SKU = op.SKU 
                                                join orders as o on ol.Order_ID = o.Order_ID 
                                                join customer as c on c.Cust_ID = o.Cust_ID 
                                            WHERE c.Cust_ID = '{customer}'; ";

                    //string infoQuery = string.Format(query, customer);

                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteReader();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("order_line");
                    connection.Close();

                    adapter.Fill(dt);
                    ordersInfo.ItemsSource = dt.DefaultView;
                    adapter.Update(dt);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}

