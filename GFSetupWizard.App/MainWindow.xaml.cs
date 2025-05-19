using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.App.Views;
using GFSetupWizard.Core;
using GFSetupWizard.Steps;

namespace GFSetupWizard.App
{
    public partial class MainWindow : Window
    {
        private IStepNavigator _navigator;
        private Dictionary<ISetupStep, UserControl> _stepViews = new Dictionary<ISetupStep, UserControl>();
        
        public MainWindow()
        {
            InitializeComponent();
            
            InitializeSteps();
            UpdateUI();
        }
        
        private void InitializeSteps()
        {
            // Create views for each step
            var welcomeView = new WelcomeStepView();
            
            // Create steps with their associated views
            var welcomeStep = new WelcomeStep(welcomeView);
            
            // Store step-view mapping
            _stepViews[welcomeStep] = welcomeView;
            
            // Create the step navigator with all steps
            _navigator = new StepNavigator(new List<ISetupStep>
            {
                welcomeStep,
                // Add more steps here as you create them
            });
            
            // Initialize the first step
            _navigator.CurrentStep?.Initialize();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navigator.CanMovePrevious)
            {
                _navigator.MovePrevious();
                UpdateUI();
            }
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navigator.CanMoveNext)
            {
                _navigator.MoveNext();
                UpdateUI();
            }
            else if (_navigator.CurrentStep?.IsComplete == true)
            {
                // This is the final step and it's complete
                // Close the wizard or show completion message
                MessageBox.Show("Setup completed successfully!", "Setup Complete", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
        
        private void UpdateUI()
        {
            // Update navigation buttons
            BackButton.IsEnabled = _navigator.CanMovePrevious;
            NextButton.Content = _navigator.CanMoveNext ? "Next" : "Finish";
            
            // Update progress bar
            // Assuming all steps have equal weight
            int totalSteps = _stepViews.Count;
            int currentStepIndex = 0;
            
            foreach (var step in _stepViews.Keys)
            {
                if (step == _navigator.CurrentStep)
                    break;
                currentStepIndex++;
            }
            
            StepProgress.Minimum = 0;
            StepProgress.Maximum = totalSteps;
            StepProgress.Value = currentStepIndex + 1;
            
            // Update content area with the current step's view
            if (_navigator.CurrentStep != null && _stepViews.TryGetValue(_navigator.CurrentStep, out var view))
            {
                StepContent.Content = view;
            }
        }
    }
}