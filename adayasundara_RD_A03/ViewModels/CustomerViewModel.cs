/*
*	FILE			:	CustomerViewModel.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    Controls the value of the customer view model
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
    /*
    * NAME: Customer View Model
    * 
    * PURPOSE: Controls the view for the customer tab
    * 
    */

    public class CustomerViewModel : ObservableObject
    {
        private CustomerViewModel _customerVModel;

        public CustomerViewModel CustomerVModel
        {
            get { return _customerVModel; }
            set { OnPropertyChanged( ref _customerVModel, value); }
        }

        private object _thisView;

        public object ThisView
        {
            get { return _thisView; }
            set { OnPropertyChanged(ref _thisView, value); }
        }

        public CustomerViewModel()
        {
            ThisView = CustomerVModel;
        }
    }
}
