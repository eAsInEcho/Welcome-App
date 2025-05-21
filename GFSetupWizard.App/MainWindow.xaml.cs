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
        private readonly Dictionary<ISetupStep, UserControl> _stepViews = new();
        private readonly StepCompletionManager _completionManager;
        
        // UI elements
        private Button BackButton;
        private Button NextButton;
        private ProgressBar StepProgress;
        private ContentControl StepContent;

        public MainWindow()
        {
            // Initialize UI components manually
            InitializeUIComponents();
            
            // Get the shared completion manager
            _completionManager = BaseSetupStep.GetCompletionManager();
            
            InitializeSteps();
            UpdateUI();
        }
        
        private void InitializeUIComponents()
        {
            // Set window properties
            Title = "GlobalFoundries Setup Wizard";
            Width = 800;
            Height = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            // Create main grid
            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            
            // Create header
            var headerBorder = new Border
            {
                Background = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(0, 119, 200)), // #0077C8
                Margin = new Thickness(20, 10, 20, 10)
            };
            Grid.SetRow(headerBorder, 0);
            
            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var headerText = new TextBlock
            {
                Text = "GlobalFoundries Setup Wizard",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White
            };
            headerPanel.Children.Add(headerText);
            headerBorder.Child = headerPanel;
            mainGrid.Children.Add(headerBorder);
            
            // Create content area
            StepContent = new ContentControl { Margin = new Thickness(20) };
            Grid.SetRow(StepContent, 1);
            mainGrid.Children.Add(StepContent);
            
            // Create navigation area
            var navGrid = new Grid
            {
                Background = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(240, 240, 240)), // #F0F0F0
                Margin = new Thickness(20, 10, 20, 10)
            };
            Grid.SetRow(navGrid, 2);
            
            navGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            navGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            navGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            
            StepProgress = new ProgressBar { Height = 20, Margin = new Thickness(0, 0, 20, 0) };
            Grid.SetColumn(StepProgress, 0);
            navGrid.Children.Add(StepProgress);
            
            BackButton = new Button
            {
                Content = "Back",
                Width = 100,
                Margin = new Thickness(0, 0, 10, 0)
            };
            BackButton.Click += BackButton_Click;
            Grid.SetColumn(BackButton, 1);
            navGrid.Children.Add(BackButton);
            
            NextButton = new Button
            {
                Content = "Next",
                Width = 100
            };
            NextButton.Click += NextButton_Click;
            Grid.SetColumn(NextButton, 2);
            navGrid.Children.Add(NextButton);
            
            mainGrid.Children.Add(navGrid);
            
            // Set the content of the window
            Content = mainGrid;
        }
        
        private void InitializeSteps()
        {
            // Create views for each step
            var welcomeView = new WelcomeStepView();
            var oneDriveView = new OneDriveSetupStepView();
            var outlookView = new OutlookSetupStepView();
            var teamsView = new TeamsSetupStepView();
            var edgeView = new EdgeSetupStepView();
            var vpnView = new VpnSetupStepView();
            var softwareView = new SoftwareSetupStepView();
            var finalSummaryView = new FinalSummaryStepView();
            
            // Create steps with their associated views
            var welcomeStep = new WelcomeStep { View = welcomeView };
            var oneDriveStep = new OneDriveSetupStep { View = oneDriveView };
            var outlookStep = new OutlookSetupStep { View = outlookView };
            var teamsStep = new TeamsSetupStep { View = teamsView };
            var edgeStep = new EdgeSetupStep { View = edgeView };
            var vpnStep = new VpnSetupStep { View = vpnView };
            var softwareStep = new SoftwareSetupStep { View = softwareView };
            var finalSummaryStep = new FinalSummaryStep { View = finalSummaryView };
            
            // Set up event handlers for step views
            oneDriveView.StepCompleted += (s, e) => oneDriveStep.MarkAsComplete();
            oneDriveView.StepSkipped += (s, e) => oneDriveStep.MarkAsSkipped();
            outlookView.StepCompleted += (s, e) => outlookStep.MarkAsComplete();
            outlookView.StepSkipped += (s, e) => outlookStep.MarkAsSkipped();
            teamsView.StepCompleted += (s, e) => teamsStep.MarkAsComplete();
            teamsView.StepSkipped += (s, e) => teamsStep.MarkAsSkipped();
            edgeView.StepCompleted += (s, e) => edgeStep.MarkAsComplete();
            edgeView.StepSkipped += (s, e) => edgeStep.MarkAsSkipped();
            vpnView.StepCompleted += (s, e) => vpnStep.MarkAsComplete();
            vpnView.StepSkipped += (s, e) => vpnStep.MarkAsSkipped();
            softwareView.StepCompleted += (s, e) => softwareStep.MarkAsComplete();
            softwareView.StepSkipped += (s, e) => softwareStep.MarkAsSkipped();
            
            // Store step-view mapping
            _stepViews[welcomeStep] = welcomeView;
            _stepViews[oneDriveStep] = oneDriveView;
            _stepViews[outlookStep] = outlookView;
            _stepViews[teamsStep] = teamsView;
            _stepViews[edgeStep] = edgeView;
            _stepViews[vpnStep] = vpnView;
            _stepViews[softwareStep] = softwareView;
            _stepViews[finalSummaryStep] = finalSummaryView;
            
            // Create the step navigator with all steps
            _navigator = new StepNavigator(new List<ISetupStep>
            {
                welcomeStep,
                oneDriveStep,
                outlookStep,
                teamsStep,
                edgeStep,
                vpnStep,
                softwareStep,
                finalSummaryStep
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
