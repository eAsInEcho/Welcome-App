using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class VpnSetupStepView : UserControl
    {
        // Event to notify parent containers when the step is completed or skipped
        public event RoutedEventHandler StepCompleted;
        public event RoutedEventHandler StepSkipped;

        public VpnSetupStepView()
        {
            InitializeComponent();
        }

        private void OpenServicePortalButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.OpenServicePortal();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to open the Service Portal automatically. Please navigate to https://serviceportal.globalfoundries.com manually.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the StepCompleted event to notify parent containers
            StepCompleted?.Invoke(this, new RoutedEventArgs());
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            // Confirm that the user wants to skip this step
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to skip requesting an RSA token? You will need this for VPN access when working remotely.",
                "Skip RSA Token Request",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                // Raise the StepSkipped event to notify parent containers
                StepSkipped?.Invoke(this, new RoutedEventArgs());
            }
        }
    }
}
