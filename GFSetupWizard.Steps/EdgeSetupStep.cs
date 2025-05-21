using GFSetupWizard.Core;
using GFSetupWizard.SystemIntegration;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class EdgeSetupStep : BaseSetupStep
    {
        public override string Title => "Microsoft Edge Setup";
        public override string Description => "Configure Microsoft Edge browser settings and enable syncing.";

        public required UserControl View { get; init; }

        // Override the IsComplete property to check if Edge is properly configured
        public override bool IsComplete => base.IsComplete || EdgeProfileManager.HasWorkAccountWithSync();

        // Override the Initialize method to check Edge profile status
        public override void Initialize()
        {
            base.Initialize();
            
            // If Edge already has a work account with sync enabled, mark the step as complete
            if (EdgeProfileManager.HasWorkAccountWithSync())
            {
                MarkAsComplete();
            }
        }

        // Use the base class implementation which uses StepCompletionManager
        public override void MarkAsComplete()
        {
            base.MarkAsComplete();
        }

        // Use the base class implementation which uses StepCompletionManager
        public override void MarkAsSkipped()
        {
            base.MarkAsSkipped();
        }
    }
}
