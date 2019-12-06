using adayasundara_RD_A03.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace adayasundara_RD_A03.Utilities
{
    public class CartUtility : ObservableObject
    {
        public static ObservableCollection<AddToCart> AddToCarts { get; set; }
        public static DataTable cartItems { get; set; }
        public static void StartExport()
        {
            try
            {
                var dataSet = new DataSet();
                var dataTable = new DataTable();
                dataSet.Tables.Add(dataTable);

                // we assume that the properties of DataSourceVM are the columns of the table
                // you can also provide the type via the second parameter
                dataTable.Columns.Add("Product Name");
                dataTable.Columns.Add("Quantity");
                dataTable.Columns.Add("Price");

                foreach (var element in AddToCarts)
                {
                    var newRow = dataTable.NewRow();

                    // fill the properties into the cells
                    newRow["Product Name"] = element.name;
                    newRow["Quantity"] = element.quantity;
                    newRow["Price"] = element.sPrice;
                    dataTable.Rows.Add(newRow);
                }

                cartItems = dataTable;
                // Do excel export
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error");
            }
        }

    }
}
