/*
*	FILE			:	WindowViewModel.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This controls the views for each of the window types.
*/

#region System 
using adayasundara_RD_A03.Utilities;
using adayasundara_RD_A03.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion System

namespace adayasundara_RD_A03
{
    /*
     * NAME: Window View Model
     * 
     * PURPOSE: Controls the Window view and manages the commands for the tabs
     * 
     */

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
        //Bound to button click for load order page
        public ICommand LoadOrderCommand { get; private set; }
        //Property of the Order View saved
        private OrdersViewModel _ordersVM;
        public OrdersViewModel OrdersVM
        {
            get { return _ordersVM; }
            set { OnPropertyChanged(ref _ordersVM, value); }
        }
        // --------------- Customer Page ----------------- //
        //Bound to Customer page to button to load the user control
        public ICommand LoadCustomerCommand { get; private set; }
        //Property of the Customer View available
        private CustomerViewModel _customerVM;

        public CustomerViewModel CustomerVM
        {
            get { return _customerVM; }
            set { OnPropertyChanged(ref _customerVM, value); }
        }
        // ------------- Product Information Page ------------ //
        //Bound to Load page to button to load the user control
        public ICommand LoadProductCommand { get; private set; }

        private ProductViewModel _productVM;

        public ProductViewModel ProductVM
        {
            get { return _productVM; }
            set { OnPropertyChanged(ref _productVM, value); }
        }

        // ------------- Metric Information Page ------------ //
        //Bound to Load page to button to load the user control
        public ICommand LoadMetricCommand { get; private set; }

        private MetricsViewModel _metricVM;

        public MetricsViewModel MetricVM
        {
            get { return _metricVM; }
            set { OnPropertyChanged(ref _metricVM, value); }
        }

        // ------------- Window Preparation ----------------//
        /// <summary>
        /// Load a new page for each of the view types
        /// </summary>
        public WindowViewModel()
        {
            OrdersVM = new OrdersViewModel();
            LoadOrderCommand = new BoundingCommand(LoadOrder);

            CustomerVM = new CustomerViewModel();
            LoadCustomerCommand = new BoundingCommand(LoadCustomer);

            ProductVM = new ProductViewModel();
            LoadProductCommand = new BoundingCommand(LoadProduct);

            MetricVM = new MetricsViewModel();
            LoadMetricCommand = new BoundingCommand(LoadMetric);
        }

        private void LoadMetric()
        {
            ThisView = MetricVM;
        }

        /// <summary>
        /// Set the current content view to the Order user control
        /// </summary>
        private void LoadOrder()
        {
            ThisView = OrdersVM;
        }
        /// <summary>
        /// Set the current content view to the Customer user control
        /// </summary>
        private void LoadCustomer()
        {
            ThisView = CustomerVM;
        }
        /// <summary>
        /// Set the current content view to the Product user control
        /// </summary>
        private void LoadProduct()
        {
            ThisView = ProductVM;
        }

    }
}
