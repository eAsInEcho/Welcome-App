using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace GFSetupWizard.Core
{
    public class StepCompletionManager
    {
        private readonly string _storageFilePath;
        private Dictionary<string, bool> _completionStatus;

        public StepCompletionManager(string storageFilePath = null)
        {
            // If no path is provided, use a default path in the user's AppData folder
            _storageFilePath = storageFilePath ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GlobalFoundries",
                "SetupWizard",
                "completion_status.json");
            
            _completionStatus = new Dictionary<string, bool>();
            
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(_storageFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            // Load existing completion status if available
            LoadCompletionStatus();
        }

        /// <summary>
        /// Marks a step as complete
        /// </summary>
        /// <param name="stepId">The unique identifier for the step</param>
        public void MarkStepComplete(string stepId)
        {
            if (string.IsNullOrWhiteSpace(stepId))
                throw new ArgumentException("Step ID cannot be null or empty", nameof(stepId));
            
            _completionStatus[stepId] = true;
            SaveCompletionStatus();
        }

        /// <summary>
        /// Marks a step as incomplete or skipped
        /// </summary>
        /// <param name="stepId">The unique identifier for the step</param>
        public void MarkStepSkipped(string stepId)
        {
            if (string.IsNullOrWhiteSpace(stepId))
                throw new ArgumentException("Step ID cannot be null or empty", nameof(stepId));
            
            _completionStatus[stepId] = false;
            SaveCompletionStatus();
        }

        /// <summary>
        /// Checks if a step is marked as complete
        /// </summary>
        /// <param name="stepId">The unique identifier for the step</param>
        /// <returns>True if the step is complete, false otherwise</returns>
        public bool IsStepComplete(string stepId)
        {
            if (string.IsNullOrWhiteSpace(stepId))
                throw new ArgumentException("Step ID cannot be null or empty", nameof(stepId));
            
            return _completionStatus.TryGetValue(stepId, out bool isComplete) && isComplete;
        }

        /// <summary>
        /// Saves the current completion status to the storage file
        /// </summary>
        public void SaveCompletionStatus()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(_completionStatus, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                File.WriteAllText(_storageFilePath, jsonString);
            }
            catch (Exception ex)
            {
                // In a real application, you might want to log this error
                Console.WriteLine($"Error saving completion status: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the completion status from the storage file
        /// </summary>
        public void LoadCompletionStatus()
        {
            try
            {
                if (File.Exists(_storageFilePath))
                {
                    string jsonString = File.ReadAllText(_storageFilePath);
                    var loadedStatus = JsonSerializer.Deserialize<Dictionary<string, bool>>(jsonString);
                    
                    if (loadedStatus != null)
                    {
                        _completionStatus = loadedStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                // In a real application, you might want to log this error
                Console.WriteLine($"Error loading completion status: {ex.Message}");
                
                // If there's an error loading, start with a fresh dictionary
                _completionStatus = new Dictionary<string, bool>();
            }
        }

        /// <summary>
        /// Gets all step completion statuses
        /// </summary>
        /// <returns>A dictionary of step IDs and their completion status</returns>
        public Dictionary<string, bool> GetAllCompletionStatuses()
        {
            return new Dictionary<string, bool>(_completionStatus);
        }
    }
}
