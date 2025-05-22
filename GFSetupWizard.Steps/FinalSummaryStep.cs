using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class FinalSummaryStep : BaseSetupStep
    {
        public override string Title => "Setup Complete";
        public override string Description => "Congratulations! You have completed the GlobalFoundries laptop setup process.";

        public required UserControl View { get; init; }

        // This step is always considered complete since it's the final summary
        // Override the base implementation to always return true regardless of StepCompletionManager
        public override bool IsComplete => true;
    }
}
