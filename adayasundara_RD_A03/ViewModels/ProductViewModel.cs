/*
*	FILE			:	ProductViewModel.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This manages the product views when an event is instantiated
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
     * NAME: Product View Model 
     * 
     * PURPOSE: Controls the view for the product
     * 
     */

    public class ProductViewModel : ObservableObject
    {
        private ProductViewModel _productVModel;
        /// <summary>
        ///     Sustain product view model
        /// </summary>
        public ProductViewModel ProductVModel
        {
            get { return _productVModel; }
            set { OnPropertyChanged(ref _productVModel, value); }
        }

        private object _thisView;
        /// <summary>
        ///        Load the current view that is established by the ICommand 
        /// </summary>
        public object ThisView
        {
            get { return _thisView; }
            set { OnPropertyChanged(ref _thisView, value); }
        }

        public ProductViewModel()
        {
            ThisView = ProductVModel;
        }
    }
}
