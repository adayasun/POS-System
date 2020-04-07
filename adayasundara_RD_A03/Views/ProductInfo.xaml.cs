using adayasundara_RD_A03.Models;
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
    /*
     * NAME: Product Info 
     * 
     * PURPOSE: Code behind to control the product information
     * 
     */
    public partial class ProductInfo : UserControl
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["adwally"].ConnectionString;
        public ProductInfo()
        {
            InitializeComponent();
        }

        private void productInfo_Loaded(object sender, RoutedEventArgs e)
        {
            int branch = Branch.ChosenBranchID;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT 
                                        p.Product_Name, p.wPrice, p.Stock
                                    FROM
                                        products as p
                                        Join
                                        branch_product as bp on bp.sku = p.sku
                                        join
                                        branch as b on bp.Branch_ID = b.Branch_ID
                                        where b.Branch_ID = '{branch}';";


                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteReader();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("product");
                    connection.Close();

                    adapter.Fill(dt);
                    productInfo.ItemsSource = dt.DefaultView;
                    adapter.Update(dt);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnNewProd_Click(object sender, RoutedEventArgs e)
        {
            int branch = Branch.ChosenBranchID;
            int SKU = 0;

            string prodName = txtProduct.Text.Trim();
            string prodType = txtType.Text.Trim();
            string stringPrice = txtPrice.Text.Trim();
            string stock = txtStock.Text.Trim();

            decimal wPrice = decimal.Parse(stringPrice);
            int stockNum = Convert.ToInt32(stock);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Insert into products to generate SKU
                    string query = $@"INSERT INTO products(Product_Name, wPrice, Stock, ProdType)
                                        VALUES('{prodName}','{wPrice}','{stockNum}','{prodType}')";
                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();

                    //Get newly created sku
                    query = $@"SELECT * FROM products 
                                WHERE Product_Name = '{prodName}';";
                    createCommand = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = createCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        SKU = ((int)dataReader["SKU"]);
                    }
                    connection.Close();

                    connection.Open();
                    //Enter into Branch to Prod table
                    query = $@"INSERT INTO branch_product(Branch_ID, SKU)
                               VALUES('{branch}','{SKU}');";
                    createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnSubmitchanges_Click(object sender, RoutedEventArgs e)
        {
            //Adjust Inventory prices
            foreach(DataRowView row in selectedProductInfo.ItemsSource)
            {
                string prodName = null;
                int quantity = 0;
                bool discontinued = false;
                string branchName = null;
                int intDisc = 0;
                //Discontinue for one, discontinues for all
                prodName = row[0].ToString();
                quantity = (int)row[1];
                discontinued = (bool)row[2];
                branchName = row[3].ToString();

                if(discontinued)
                {
                    intDisc = 1;
                }
                else
                {
                    intDisc = 0;
                }
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        //Discontinue all products if one is discontinued
                        string query = $@"UPDATE products SET Discontinued='{intDisc}'
                                            WHERE Product_Name ='{prodName}'; ";
                        MySqlCommand createCommand = new MySqlCommand(query, connection);
                        createCommand.ExecuteNonQuery();
                        //Update inventory -- Comeback to this
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void productInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProductInfo.ItemsSource = null;
            DataRowView dataRowView = (DataRowView)productInfo.SelectedItem;
            string name = Convert.ToString(dataRowView.Row[0]);

            //Add to new table all the product levels of all the branches
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@" SELECT p.Product_Name as 'Product Name',
                                         p.Stock as 'Stock', p.Discontinued as 'Discontinued',
                                         b.Branch_Name as 'Branch Name'
                                         FROM products as p 
	                                        join
                                            branch_product as bp on bp.SKU = p.SKU
                                            join
                                            branch as b on bp.Branch_ID = b.Branch_ID
                                         where p.Product_Name = '{name}';";


                    MySqlCommand createCommand = new MySqlCommand(query, connection);
                    createCommand.ExecuteReader();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("product");
                    connection.Close();

                    adapter.Fill(dt);
                    selectedProductInfo.ItemsSource = dt.DefaultView;
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
