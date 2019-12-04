using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.ViewModels
{
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
