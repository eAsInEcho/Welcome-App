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
        
        // UI elements
        private Button _backButton;
        private Button _nextButton;
        private ProgressBar _stepProgress;
        private ContentControl _stepContent;

        public MainWindow()
        {
            // Initialize UI components manually
            InitializeUIComponents();
            
            InitializeSteps();
            UpdateUI();
        }
        
        private void InitializeUIComponents()
        {
            // Set window properties
            Title = "GF Setup Wizard";
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
                Background = Application.Current.Resources["GFOrangeBrush"] as System.Windows.Media.SolidColorBrush,
                Margin = new Thickness(20, 10, 20, 10)
            };
            Grid.SetRow(headerBorder, 0);
            
            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            
            // GF Logo
            var logoImage = new Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(
                    new System.Uri("/GFSetupWizard.App;component/Resources/Images/New _GF_icons_Globe_ylw.png", System.UriKind.Relative)),
                Width = 40,
                Height = 40,
                Margin = new Thickness(10, 5, 10, 5)
            };
            Grid.SetColumn(logoImage, 0);
            headerGrid.Children.Add(logoImage);
            
            // Title with colon graphic element
            var titlePanel = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetColumn(titlePanel, 1);
            
            var gfText = new TextBlock
            {
                Text = "GF",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White
            };
            titlePanel.Children.Add(gfText);
            
            var colonGrid = new Grid { Margin = new Thickness(10, 0, 0, 0) };
            colonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            colonGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            
            var topColon = new Border
            {
                Width = 10,
                Height = 10,
                Background = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 0, 0, 2)
            };
            Grid.SetRow(topColon, 0);
            colonGrid.Children.Add(topColon);
            
            var bottomColon = new Border
            {
                Width = 10,
                Height = 10,
                Background = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 2, 0, 0)
            };
            Grid.SetRow(bottomColon, 1);
            colonGrid.Children.Add(bottomColon);
            
            titlePanel.Children.Add(colonGrid);
            
            var setupWizardText = new TextBlock
            {
                Text = "Setup Wizard",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(10, 0, 0, 0)
            };
            titlePanel.Children.Add(setupWizardText);
            
            headerGrid.Children.Add(titlePanel);
            headerBorder.Child = headerGrid;
            mainGrid.Children.Add(headerBorder);
            
            // Create content area
            _stepContent = new ContentControl { Margin = new Thickness(20) };
            Grid.SetRow(_stepContent, 1);
            mainGrid.Children.Add(_stepContent);
            
            // Create navigation area
            var navGrid = new Grid
            {
                Background = Application.Current.Resources["GFLightGreyBrush"] as System.Windows.Media.SolidColorBrush,
                Margin = new Thickness(20, 10, 20, 10)
            };
            Grid.SetRow(navGrid, 2);
            
            navGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            navGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            navGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            
            _stepProgress = new ProgressBar
            {
                Height = 20,
                Margin = new Thickness(0, 0, 20, 0),
                Foreground = Application.Current.Resources["GFPurpleBrush"] as System.Windows.Media.SolidColorBrush
            };
            Grid.SetColumn(_stepProgress, 0);
            navGrid.Children.Add(_stepProgress);
            
            _backButton = new Button
            {
                Content = "Back",
                Width = 100,
                Margin = new Thickness(0, 0, 10, 0),
                Background = System.Windows.Media.Brushes.White,
                BorderBrush = Application.Current.Resources["GFOrangeBrush"] as System.Windows.Media.SolidColorBrush,
                Foreground = Application.Current.Resources["GFOrangeBrush"] as System.Windows.Media.SolidColorBrush
            };
            _backButton.Click += BackButton_Click;
            Grid.SetColumn(_backButton, 1);
            navGrid.Children.Add(_backButton);
            
            _nextButton = new Button
            {
                Content = "Next",
                Width = 100,
                Background = Application.Current.Resources["GFOrangeBrush"] as System.Windows.Media.SolidColorBrush,
                Foreground = System.Windows.Media.Brushes.White
            };
            _nextButton.Click += NextButton_Click;
            Grid.SetColumn(_nextButton, 2);
            navGrid.Children.Add(_nextButton);
            
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
            
            // Step completion events removed as they're no longer needed
            
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
            _backButton.IsEnabled = _navigator.CanMovePrevious;
            _nextButton.Content = _navigator.CanMoveNext ? "Next" : "Finish";
            
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
            
            _stepProgress.Minimum = 0;
            _stepProgress.Maximum = totalSteps;
            _stepProgress.Value = currentStepIndex + 1;
            
            // Update content area with the current step's view
            if (_navigator.CurrentStep != null && _stepViews.TryGetValue(_navigator.CurrentStep, out var view))
            {
                _stepContent.Content = view;
            }
        }
    }
}
