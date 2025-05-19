// BaseSetupStep.cs in GFSetupWizard.Core
namespace GFSetupWizard.Core
{
    public abstract class BaseSetupStep : ISetupStep
    {
        private static StepCompletionManager _completionManager;
        
        // Static constructor to initialize the shared completion manager
        static BaseSetupStep()
        {
            _completionManager = new StepCompletionManager();
        }
        
        public abstract string Title { get; }
        public abstract string Description { get; }
        
        // StepId is used to uniquely identify each step for persistence
        public virtual string StepId => GetType().Name;
        
        // IsComplete now checks the StepCompletionManager
        public virtual bool IsComplete => _completionManager.IsStepComplete(StepId);
        
        public virtual bool CanProceed { get; protected set; } = true;

        public virtual void Initialize() 
        {
            // Load completion status when initializing
            _completionManager.LoadCompletionStatus();
        }

        public virtual bool Validate()
        {
            return true;
        }

        public virtual void Execute()
        {
            MarkAsComplete();
        }

        public virtual void Cleanup() { }
        
        // Method to mark the step as complete
        public virtual void MarkAsComplete()
        {
            _completionManager.MarkStepComplete(StepId);
        }
        
        // Method to mark the step as skipped
        public virtual void MarkAsSkipped()
        {
            _completionManager.MarkStepSkipped(StepId);
        }
        
        // Static method to get the completion manager
        public static StepCompletionManager GetCompletionManager()
        {
            return _completionManager;
        }
    }
}
