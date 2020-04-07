using adayasundara_RD_A03.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adayasundara_RD_A03.ViewModels
{
    public class MetricsViewModel : ObservableObject
    {
        private MetricsViewModel _metricsVModel;

        public MetricsViewModel MetricsVModel
        {
            get { return _metricsVModel; }
            set { OnPropertyChanged(ref _metricsVModel, value); }
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

        public MetricsViewModel()
        {
            ThisView = MetricsVModel;
        }
    }
}
