using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class OutlookSetupStep : BaseSetupStep
    {
        public override string Title => "Outlook Setup";
        public override string Description => "Configure Microsoft Outlook for your email and calendar";

        public required UserControl View { get; init; }
    }
}
