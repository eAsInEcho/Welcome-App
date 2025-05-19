using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class TeamsSetupStep : BaseSetupStep
    {
        public override string Title => "Microsoft Teams Setup";
        public override string Description => "Configure Microsoft Teams for communication and collaboration.";

        public required UserControl View { get; init; }

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
