﻿/*
*	FILE			:	Customer.xaml.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This manages the customer information for adding a new customer,
*	                    looking at currently selected customer information and adding 
*	                    current customer to branch.
*/

#region Systems
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
#endregion Systems

namespace adayasundara_RD_A03.Views
{

    /*
     * NAME: Customer 
     * 
     * PURPOSE: Behind code for the customer to manage the customer creation
     * 
     */
    public partial class Customer : UserControl
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;
        List<CustomerInfo> customer = new List<CustomerInfo>();

        public Customer()
        {
            InitializeComponent();
        }

        /// <summary>
        ///        Load orders and update into table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ordersInfo_Loaded(object sender, RoutedEventArgs e)
        {
            //cmbBranches.Items.Clear();
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
                                            o.OrderDate as 'Order Date',
                                            ol.Quantity as 'Quantity',
                                            ol.PaymentStatus as 'Payment Status'
                                        FROM
                                            order_line AS ol
                                                JOIN
                                            orderline_prod AS op ON op.Order_Line_ID = ol.Order_Line_ID
                                                JOIN
                                            products AS p ON p.SKU = op.SKU
                                                JOIN
                                            orders AS o ON ol.Order_ID = o.Order_ID
                                                JOIN
                                            customer AS c ON c.Cust_ID = o.Cust_ID
                                        WHERE
                                            c.Cust_ID = '{customer}'; ";


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
            //Quantity 4
            //Date 3
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
                    int orderLineId = 0;
                    DataRowView information = (DataRowView)rowView;

                    int orderId = Int32.Parse(information.Row.ItemArray[0].ToString());
                    string prodName = information.Row.ItemArray[1].ToString();
                    int quantity = Int32.Parse(information.Row.ItemArray[4].ToString());
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
                    connection.Close();

                    connection.Open();
                    //INSERT Paid to refund
                    query = $@"INSERT INTO order_line(Order_ID, Quantity, PaymentStatus)
                                          VALUES ('{orderId}','{quantity}', '{refund}');";
                    createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    //Get new Orderline ID and associate with Product ID
                    query = $@"Select * FROM order_line
                                WHERE Order_ID = '{orderId}' AND
                                      Quantity = '{quantity}' AND
                                      PaymentStatus = '{refund}'";
                    createCommand = new MySqlCommand(query, connection);
                    datareader = createCommand.ExecuteReader();
                    while(datareader.Read())
                    {
                        orderLineId = ((int)datareader["Order_Line_ID"]);
                    }
                    connection.Close();

                    connection.Open();
                    //Insert new association with prod
                    query = $@"INSERT INTO orderline_prod(Order_Line_ID, SKU)
                                VALUES('{orderLineId}','{SKU}');";
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

        /// <summary>
        ///     Enter new customer into database - have to change this to one to many
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    //connection.Open();
                    ////Insert into Branch to Customer (branch_cust)
                    //query = $@"INSERT INTO branch_cust(Cust_ID, Branch_ID)
                    //                    VALUES('{custId}','{branchId}')";

                    //DontNeed.branchToCustomers.Add(new DontNeed()
                    //{
                    //    lBranchId = branchId,
                    //    lCustId = custId
                    //});
                    //createCommand = new MySqlCommand(query, connection);
                    //createCommand.ExecuteNonQuery();


                    //connection.Close();
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


        /// <summary>
        ///     Choose a branches to load the customer to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cmbBranches_Loaded(object sender, RoutedEventArgs e)
        //{
        //    cmbBranches.Items.Clear();
        //    cmbBranches.Text = "--- SELECT A BRANCH ---";

        //    int currentCustomer = CustomerInfo.ChosenCustomer;
        //    int currentBranch = Branch.ChosenBranchID;

        //    //Search for branches customer that is not in
        //    foreach(var branch in Branch.branches)
        //    {
        //        foreach(var branchCust in DontNeed.branchToCustomers)
        //        {
        //           if(branch.branchID != branchCust.lBranchId)
        //           {
        //                if(branchCust.lCustId != currentCustomer)
        //                {
        //                    cmbBranches.Items.Add(branch.branchName);
        //                }
        //           }
        //        }
        //    }
        //}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnNewBranch_Click(object sender, RoutedEventArgs e)
        //{
        //    string branchName = cmbBranches.SelectedItem.ToString();
        //    int currentCustomer = CustomerInfo.ChosenCustomer;
        //    int branchId = 0;
        //    foreach (var branch in Branch.branches)
        //    {
        //        if(branch.branchName == branchName)
        //        {
        //            branchId = branch.branchID;

        //            using (MySqlConnection connection = new MySqlConnection(connectionString))
        //            {
        //                try
        //                {
        //                    connection.Open();
                            
        //                    //Insert into Branch to Customer (branch_cust)
        //                    string query = $@"INSERT INTO branch_cust(Cust_ID, Branch_ID)
        //                                VALUES('{currentCustomer}','{branchId}')";

        //                    MySqlCommand createCommand = new MySqlCommand(query, connection);
        //                    createCommand.ExecuteNonQuery();
        //                    connection.Close();

        //                    DontNeed.branchToCustomers.Add(new DontNeed()
        //                    {
        //                        lBranchId = branchId,
        //                        lCustId = currentCustomer
        //                    });

        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                }
        //            }
        //        }
        //    }
        //}

    }
}

