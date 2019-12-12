/*
*	FILE			:	MainWindow.xaml.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This has the code behind for the controls of the main window.
*	                    It will also load the appropriate tables for each of the required 
*	                    models.
*/

#region System
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
#endregion System

namespace adayasundara_RD_A03
{
    /*
     * NAME: Main Window 
     * 
     * PURPOSE: To generate the table information for the code
     * 
     */
    public partial class MainWindow : Window
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;
        List<Branch> branch = new List<Branch>();
        List<CustomerInfo> customer = new List<CustomerInfo>();
        List<DontNeed> linkBranchCust = new List<DontNeed>();
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
                    MessageBox.Show(ex.Message + "MainWindow, Branch loaded");
                }
            }

            //Retrieve Customer information (Independent)
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

                        customer.Add(new CustomerInfo()
                        {
                            custID = ((int)datareader["Cust_ID"]),
                            fName = datareader["FName"] as string,
                            lName = datareader["LName"] as string,
                            phoneNumber = datareader["phoneNumber"] as string
                        });

                        string first = datareader["FName"] as string;
                        string last = datareader["LName"] as string;
                        string fAndL = first + " " + last;
                        customers.Items.Add(fAndL);

                    }
                    connection.Close();
                    CustomerInfo.customers = customer;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "MainWindow: Customer");
                }
            }

            //Retrieve Associative table to match branch to customer
            //using (MySqlConnection connection = new MySqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();
            //        string customerQuery = "SELECT * FROM branch_cust";
            //        MySqlCommand createCommand = new MySqlCommand(customerQuery, connection);
            //        MySqlDataReader datareader = createCommand.ExecuteReader();
            //        while (datareader.Read())
            //        {
            //            linkBranchCust.Add(new BranchToCustomer()
            //            {
            //                lCustId = ((int)datareader["Cust_ID"]),
            //                lBranchId=((int)datareader["Branch_ID"])                            
            //            });
            //        }
            //        connection.Close();
            //        BranchToCustomer.branchToCustomers = linkBranchCust;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}

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

        //private string[] GetCustomerById(int id)
        //{
        //    return CustomerInfo.customers.Where(line => line.custID == id).Select(l => (l.fName + " " + l.lName)).ToArray();
        //}

        //private int[] GetBranchToCustId(int id)
        //{
        //    return DontNeed.branchToCustomers.Where(line => line.lBranchId == id).Select(l => (l.lCustId)).ToArray();
        //}

        private void branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string branchName = null;

            //customers.Items.Clear();
            branchName = branches.SelectedValue.ToString();
            int bId = (branches.SelectedIndex) + 1;
            Branch.ChosenBranchID = bId;
            //foreach (int id in GetBranchToCustId(bId))
            //{
            //    //string name = GetCustomerById(id);
            //    foreach (string name in GetCustomerById(id))
            //    {
            //        this.customers.Items.Add(name);
            //    }
            //}
        }

        private int DbCustomerId(string name)
        {
            return CustomerInfo.customers.Where(line => line.fName == name).Select(l => l.custID).Sum();
        }

        private void customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string name = customers.SelectedValue.ToString();
                string fName = name.Substring(0, name.IndexOf(' '));
                CustomerInfo.ChosenCustomer = DbCustomerId(fName);

            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

    }
}
