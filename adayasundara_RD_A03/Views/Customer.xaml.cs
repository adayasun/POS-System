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
        List<CustomerInfo> customer = new List<CustomerInfo>();

        public Customer()
        {
            InitializeComponent();
        }

        private void ordersInfo_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        /// <summary>
        ///     Update the table information
        /// </summary>
        private void UpdateTable()
        {
            ordersInfo.ItemsSource = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string customer = CustomerInfo.ChosenCustomer.ToString();
                    connection.Open();
                    string query = $@"SELECT 
                                            o.Order_ID as 'Order ID', 
                                            p.Product_Name as 'Product Name',
                                            p.ProdType as 'Product Type',
                                            ol.Quantity as 'Quantity', 
                                            o.OrderDate as 'Order Date', 
                                            ol.PaymentStatus as 'Payment Status' 
                                    FROM order_line as ol 
                                        join orderline_prod as op on op.Order_Line_ID = ol.Order_Line_ID 
                                        join products as p on p.SKU = op.SKU 
                                        join orders as o on ol.Order_ID = o.Order_ID 
                                        join customer as c on c.Cust_ID = o.Cust_ID 
                                    WHERE c.Cust_ID = '{customer}'; ";


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

        /// <summary>
        ///     Refund the customers purchased product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {
            //OrderId 0
            //Product Name 1
            //Product Type 2
            //Quantity 3
            //Date 4
            //Payment Status 5


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string refund = "RFND";
                    string today = DateTime.Now.ToString("yyyy-mm-dd");
                    var rowView = ordersInfo.SelectedItem;
                    int SKU = 0;
                    int prodQuant = 0;
                    int updatedQuant = 0;
                    int currentCustomer = CustomerInfo.ChosenCustomer;

                    DataRowView information = (DataRowView)rowView;

                    int orderId = Int32.Parse(information.Row.ItemArray[0].ToString());
                    string prodName = information.Row.ItemArray[1].ToString();
                    int quantity = Int32.Parse(information.Row.ItemArray[3].ToString());
                    string prodType = information.Row.ItemArray[2].ToString();

                    connection.Open();
                    //Get SKU number
                    string query = $@"SELECT * 
                                            FROM products
                                            WHERE Product_Name = '{prodName}'
                                            AND ProdType = '{prodType}';";
                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while(datareader.Read())
                    {
                        SKU = ((int)datareader["SKU"]);
                        prodQuant = ((int)datareader["Stock"]);
                    }

                    updatedQuant = prodQuant + quantity;

                    connection.Close();

                    connection.Open();
                    //Update Product Quantity
                    query = $@"UPDATE products 
                                    SET Stock = '{updatedQuant}'
                                    WHERE SKU ='{SKU}';";
                    createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();

                    //INSERT Paid to refund
                    query = $@"INSERT INTO order_line(Order_ID, Quantity, PaymentStatus)
                                          VALUES ('{orderId}','{quantity}', '{refund}');";
                    createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();

                    connection.Close();
                    UpdateTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        private void btnNewCust_Click(object sender, RoutedEventArgs e)
        {
            string fName = txtFirst.Text.Trim();
            string lName = txtLast.Text.Trim();
            string phoneNumb = txtPhone.Text.Trim();
            int branchId = Branch.ChosenBranchID;
            int custId = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Insert into customer (customer)
                    //INSERT INTO ORDER
                    string query = $@"INSERT INTO customer(FName, LName, PhoneNumber)
                                        VALUES('{fName}','{lName}','{phoneNumb}')";

                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();

                    //Get customer id
                    query = $@"SELECT * 
                                    FROM customer 
                                    WHERE FName = '{fName}'
                                    AND LName ='{lName}';";
                    createCommand = new MySqlCommand(query, connection);
                    MySqlDataReader datareader = createCommand.ExecuteReader();
                    while (datareader.Read())
                    {
                        custId = ((int)datareader["Cust_ID"]);

                        CustomerInfo.customers.Add(new CustomerInfo()
                        {
                            custID = ((int)datareader["Cust_ID"]),
                            fName = datareader["FName"] as string,
                            lName = datareader["LName"] as string,
                            phoneNumber = datareader["phoneNumber"] as string
                        });

                    }
                    connection.Close();

                    connection.Open();
                    //Insert into Branch to Customer (branch_cust)
                    query = $@"INSERT INTO branch_cust(Cust_ID, Branch_ID)
                                        VALUES('{custId}','{branchId}')";

                    BranchToCustomer.branchToCustomers.Add(new BranchToCustomer()
                    {
                        lBranchId = branchId,
                        lCustId = custId
                    });
                    createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();


                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            txtFirst.Text = "";
            txtLast.Text = "";
            txtPhone.Text = "";
        }


        //COME BACK TO THIS
        private void cmbBranches_Loaded(object sender, RoutedEventArgs e)
        {
            cmbBranches.Items.Clear();
            cmbBranches.Text = "--- SELECT A BRANCH ---";

            int currentCustomer = CustomerInfo.ChosenCustomer;
            int currentBranch = Branch.ChosenBranchID;

            //Search for branches customer that is not in
            foreach(var branch in Branch.branches)
            {
                foreach(var branchCust in BranchToCustomer.branchToCustomers)
                {
                   if(branch.branchID != branchCust.lBranchId)
                   {
                        if(branchCust.lCustId != currentCustomer)
                        {
                            cmbBranches.Items.Add(branch.branchName);
                        }
                   }
                }
            }
        }

        private void btnNewBranch_Click(object sender, RoutedEventArgs e)
        {
            string branchName = cmbBranches.SelectedItem.ToString();
            int currentCustomer = CustomerInfo.ChosenCustomer;
            int branchId = 0;
            foreach (var branch in Branch.branches)
            {
                if(branch.branchName == branchName)
                {
                    branchId = branch.branchID;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            
                            //Insert into Branch to Customer (branch_cust)
                            string query = $@"INSERT INTO branch_cust(Cust_ID, Branch_ID)
                                        VALUES('{currentCustomer}','{branchId}')";

                            MySqlCommand createCommand = new MySqlCommand(query, connection);
                            createCommand.ExecuteNonQuery();
                            connection.Close();

                            BranchToCustomer.branchToCustomers.Add(new BranchToCustomer()
                            {
                                lBranchId = branchId,
                                lCustId = currentCustomer
                            });

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}

