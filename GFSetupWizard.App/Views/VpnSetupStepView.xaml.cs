using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class VpnSetupStepView : UserControl
    {
        // Events needed for step navigation
        public event RoutedEventHandler StepCompleted;
        public event RoutedEventHandler StepSkipped;
        
        public VpnSetupStepView()
        {
            InitializeComponent();
            
            // Auto-mark step as complete when the view is loaded
            Loaded += (s, e) => StepCompleted?.Invoke(this, new RoutedEventArgs());
        }

        private void OpenServicePortalButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.OpenRsaTokenRequestForVpn();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to open the service portal. Please contact IT support for assistance.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
