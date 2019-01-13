using System.Windows.Controls;
using ActivityTracker.ViewModels;

namespace ActivityTracker.Views
{
    /// <summary>
    /// Interaction logic for ActivitiesView.xaml
    /// </summary>
    public partial class ActivitiesView : UserControl
    {
        public ActivitiesView()
        {
            InitializeComponent();
            DataContext = new ActivitiesViewModel();
        }
    }
}
