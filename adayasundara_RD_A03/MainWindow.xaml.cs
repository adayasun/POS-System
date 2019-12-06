using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using adayasundara_RD_A03.Models;
using adayasundara_RD_A03.Utilities;
using adayasundara_RD_A03.ViewModels;
using MySql.Data.MySqlClient;


namespace adayasundara_RD_A03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;
        List<Branch> branch = new List<Branch>();
        List<CustomerInfo> customer = new List<CustomerInfo>();
        List<BranchToCustomer> linkBranchCust = new List<BranchToCustomer>();
        List<BranchToProduct> linkBranchProd = new List<BranchToProduct>();
        List<ProductList> products = new List<ProductList>();
        public MainWindow()
        {
            InitializeComponent();
            CartUtility.AddToCarts = new ObservableCollection<AddToCart>(); 
        }

        private void branches_Loaded(object sender, RoutedEventArgs e)
        {

            //Retrieve Branch information (Independent)
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string branchQuery = "SELECT * FROM Branch";
                    MySqlCommand createCommand = new MySqlCommand(branchQuery, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        branches.Items.Add(datareader["Branch_Name"]);

                        branch.Add(new Branch()
                        {
                            branchID = ((int)datareader["Branch_ID"]),
                            branchName = datareader["Branch_Name"] as string
                        });
                    }
                    connection.Close();
                    Branch.branches = branch;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //Retrieve Customer information (Dependant on Branch)
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string customerQuery = "SELECT * FROM Customer";
                    MySqlCommand createCommand = new MySqlCommand(customerQuery, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        //customers.Items.Add(datareader["FName" + "LName"]);
                        customer.Add(new CustomerInfo()
                        {
                            custID = ((int)datareader["Cust_ID"]),
                            fName = datareader["FName"] as string,
                            lName = datareader["LName"] as string,
                            phoneNumber = datareader["phoneNumber"] as string
                        });
                    }
                    connection.Close();
                    CustomerInfo.customers = customer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //Retrieve Associative table to match branch to customer
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string customerQuery = "SELECT * FROM branch_cust";
                    MySqlCommand createCommand = new MySqlCommand(customerQuery, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        //customers.Items.Add(datareader["FName" + "LName"]);
                        linkBranchCust.Add(new BranchToCustomer()
                        {
                            lCustId = ((int)datareader["Cust_ID"]),
                            lBranchId=((int)datareader["Branch_ID"])                            
                        });
                    }
                    connection.Close();
                    BranchToCustomer.branchToCustomers = linkBranchCust;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //Retrieve Associative table for products
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string customerQuery = "SELECT * FROM branch_product";
                    MySqlCommand createCommand = new MySqlCommand(customerQuery, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        //customers.Items.Add(datareader["FName" + "LName"]);
                        linkBranchProd.Add(new BranchToProduct()
                        {
                            lSKU = ((int)datareader["SKU"]),
                            lBranchId = ((int)datareader["Branch_ID"])
                        });
                    }
                    connection.Close();
                    BranchToProduct.branchToProducts = linkBranchProd;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //Retrieve Products Table
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
                            wPrice=((decimal)datareader["wPrice"]),
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

        }

        private string[] GetCustomerById(int id)
        {
            return CustomerInfo.customers.Where(line => line.custID == id).Select(l => (l.fName + " " + l.lName)).ToArray();
        }
        private int[] GetBranchToCustId(int id)
        {
            return BranchToCustomer.branchToCustomers.Where(line => line.lBranchId == id).Select(l => (l.lCustId)).ToArray();
        }
        private void branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customers.Items.Clear();
            string branchName = branches.SelectedValue.ToString();
            int bId = (branches.SelectedIndex) + 1;
            Branch.ChosenBranchID = bId;
            foreach(int id in GetBranchToCustId(bId))
            {
                //string name = GetCustomerById(id);
                foreach(string name in GetCustomerById(id))
                {
                    this.customers.Items.Add(name);
                }
            }
           
        }

        private int DbCustomerId(string name)
        {
            return CustomerInfo.customers.Where(line => line.fName == name).Select(l => l.custID).Sum();
        }
        private void customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = customers.SelectedValue.ToString();
            string fName = name.Substring(0, name.IndexOf(' '));
            CustomerInfo.ChosenCustomer = DbCustomerId(fName);
        }

        public void ShowData()
        {
            //cart.ItemsSource = null;
            //cart.ItemsSource = CartUtility.AddToCarts;
        }

        private void cart_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ShowData();
        }

        private void updateCart_Loaded(object sender, RoutedEventArgs e)
        {
            //updateCart.ItemsSource = CartUtility.cartItems.DefaultView;
        }
    }
}
