using adayasundara_RD_A03.Models;
using MySql.Data.MySqlClient;
using System;
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
    /// Interaction logic for Metrics.xaml
    /// </summary>
    public partial class Metrics : UserControl
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;
        public Metrics()
        {
            InitializeComponent();
        }

        private void gridSales_Loaded(object sender, RoutedEventArgs e)
        {
            int branch = Branch.ChosenBranchID;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT 
                                            b.Branch_Name AS 'Branch Name',
                                            SUM(ol.Extended_Price) AS 'Total Sales Revenue'
                                        FROM
                                            orders AS o
                                                JOIN
                                            order_line AS ol ON ol.Order_ID = o.Order_ID 
                                                JOIN
                                            branch AS b ON o.Branch_ID = b.Branch_ID
                                        GROUP BY b.Branch_Name;";



                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteReader();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("product");
                    connection.Close();

                    adapter.Fill(dt);
                    gridSales.ItemsSource = dt.DefaultView;
                    adapter.Update(dt);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void gridProd_Loaded(object sender, RoutedEventArgs e)
        {
            int branch = Branch.ChosenBranchID;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT 
                                            p.Product_Name, SUM(ol.Quantity) AS Sold 
                                        FROM 
                                            order_line AS ol
                                                JOIN
                                            orderline_prod AS op ON ol.Order_Line_ID = op.Order_Line_ID
                                                JOIN
                                            products AS p ON p.SKU = op.SKU
                                                JOIN
                                            branch_product AS bp ON bp.sku = p.sku
                                                JOIN
                                            branch AS b ON bp.Branch_ID = b.Branch_ID
                                        WHERE
                                            b.Branch_ID = '{branch}' 
                                        GROUP BY p.Product_Name
                                        ORDER BY Sold DESC; 
                                        ";


                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteReader();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("product");
                    connection.Close();

                    adapter.Fill(dt);
                    gridProd.ItemsSource = dt.DefaultView;
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
