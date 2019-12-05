using adayasundara_RD_A03.Utilities;
using adayasundara_RD_A03.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace adayasundara_RD_A03
{
    public class WindowViewModel : ObservableObject
    {
        //Setup the current view of the model
        private object _thisView;
        public object ThisView
        {
            get { return _thisView; }
            set { OnPropertyChanged(ref _thisView, value); }
        }

        // --------------- Orders Page ------------------- //
        public ICommand LoadOrderCommand { get; private set; }
        //Property of the Order View saved
        private OrdersViewModel _ordersVM;
        public OrdersViewModel OrdersVM
        {
            get { return _ordersVM; }
            set { OnPropertyChanged(ref _ordersVM, value); }
        }
        // --------------- Customer Page ----------------- //
        public ICommand LoadCustomerCommand { get; private set; }
        //Property of the Customer View available
        private CustomerViewModel _customerVM;

        public CustomerViewModel CustomerVM
        {
            get { return _customerVM; }
            set { OnPropertyChanged(ref _customerVM, value); }
        }
        // ------------- Product Information Page ------------ //

        public ICommand LoadProductCommand { get; private set; }

        private ProductViewModel _productVM;

        public ProductViewModel ProductVM
        {
            get { return _productVM; }
            set { OnPropertyChanged(ref _productVM, value); }
        }
        // ------------- Window Preparation ----------------//
        public WindowViewModel()
        {
            OrdersVM = new OrdersViewModel();
            LoadOrderCommand = new BoundingCommand(LoadOrder);

            CustomerVM = new CustomerViewModel();
            LoadCustomerCommand = new BoundingCommand(LoadCustomer);

            ProductVM = new ProductViewModel();
            LoadProductCommand = new BoundingCommand(LoadProduct);
        }

        private void LoadOrder()
        {
            ThisView = OrdersVM;
        }

        private void LoadCustomer()
        {
            ThisView = CustomerVM;
        }

        private void LoadProduct()
        {
            ThisView = ProductVM;
        }
    }
}
