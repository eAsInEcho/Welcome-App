// BaseSetupStep.cs in GFSetupWizard.Core
namespace GFSetupWizard.Core
{
    public abstract class BaseSetupStep : ISetupStep
    {
        public abstract string Title { get; }
        public abstract string Description { get; }
        public virtual bool IsComplete { get; protected set; }
        public virtual bool CanProceed { get; protected set; } = true;

        public virtual void Initialize() { }

        public virtual bool Validate()
        {
            return true;
        }

        public virtual void Execute()
        {
            IsComplete = true;
        }

        public virtual void Cleanup() { }
    }
}