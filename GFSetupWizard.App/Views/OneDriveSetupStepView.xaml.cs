using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class OneDriveSetupStepView : UserControl
    {
        // Events needed for step navigation
        public event RoutedEventHandler StepCompleted;
        public event RoutedEventHandler StepSkipped;
        
        public OneDriveSetupStepView()
        {
            InitializeComponent();
            
            // Auto-mark step as complete when the view is loaded
            Loaded += (s, e) => StepCompleted?.Invoke(this, new RoutedEventArgs());
        }

        private void LaunchOneDriveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.LaunchOneDriveWithFeedback();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to launch OneDrive automatically. Please open it manually from the Start menu.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
