using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class OneDriveSetupStepView : UserControl
    {
        // Event to notify parent containers when the step is completed or skipped
        public event RoutedEventHandler StepCompleted;
        public event RoutedEventHandler StepSkipped;

        public OneDriveSetupStepView()
        {
            InitializeComponent();
        }

        private void LaunchOneDriveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.LaunchOneDrive();
            
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

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the StepCompleted event to notify parent containers
            StepCompleted?.Invoke(this, new RoutedEventArgs());
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            // Confirm that the user wants to skip this step
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to skip OneDrive setup? You can always set it up later.",
                "Skip OneDrive Setup",
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
