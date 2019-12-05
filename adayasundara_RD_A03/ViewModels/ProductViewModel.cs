using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.ViewModels
{
    public class ProductViewModel : ObservableObject
    {
        private ProductViewModel _productVModel;
        public ProductViewModel ProductVModel
        {
            get { return _productVModel; }
            set { OnPropertyChanged(ref _productVModel, value); }
        }

        private object _thisView;
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
