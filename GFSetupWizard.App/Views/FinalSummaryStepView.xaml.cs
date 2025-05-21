using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class FinalSummaryStepView : UserControl
    {
        public FinalSummaryStepView()
        {
            InitializeComponent();
        }

        private void OpenServicePortalButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.OpenServicePortal();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to open the Service Portal automatically. Please navigate to https://globalfoundries.service-now.com/esc manually.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent window and close it
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
