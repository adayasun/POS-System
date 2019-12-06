/*
*	FILE			:	OrdersViewModel.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This processes the orders view to reflect property changes.
*/
#region Systems
using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion Systems

namespace adayasundara_RD_A03.ViewModels
{
    public class OrdersViewModel : ObservableObject
    {
        private OrdersViewModel _ordersVModel;
        public OrdersViewModel OrdersVModel
        {
            get { return _ordersVModel; }
            set { OnPropertyChanged(ref _ordersVModel, value); }
        }

        private object _thisView;
        public object ThisView
        {
            get { return _thisView; }
            set { OnPropertyChanged(ref _thisView, value); }
        }

        public OrdersViewModel()
        {
            ThisView = OrdersVModel;
        }
    }
}
