using System;
using System.IO;
using Microsoft.Win32;

namespace GFSetupWizard.SystemIntegration
{
    public static class InstallationManager
    {
        /// <summary>
        /// Checks if the registry key for startup is properly set.
        /// </summary>
        /// <returns>True if registry key exists, false otherwise.</returns>
        public static bool RegistryKeyExists()
        {
            try
            {
                string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                // Check in HKCU first (preferred)
                using (var key = Registry.CurrentUser.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object? value = key.GetValue("GFSetupWizard");
                        if (value != null)
                        {
                            return true;
                        }
                    }
                }
                
                // Fall back to checking HKLM for backward compatibility
                using (var key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object? value = key.GetValue("GFSetupWizard");
                        return value != null;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking registry key: {ex.Message}");
                return false;
            }
        }
        
        
        /// <summary>
        /// Sets up the registry entry for application startup.
        /// </summary>
        /// <returns>True if registry entry was created successfully, false otherwise.</returns>
        public static bool SetupRegistryEntry()
        {
            try
            {
                // Get the current executable path
                string? exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
                if (string.IsNullOrEmpty(exePath))
                {
                    Console.WriteLine("Error: Could not determine the current executable path.");
                    return false;
                }
                
                // Create registry key for startup in HKCU (doesn't require admin rights)
                string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                using (var key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("GFSetupWizard", exePath);
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up registry: {ex.Message}");
                return false;
            }
        }
        
        
        /// <summary>
        /// Removes the registry entry for application startup.
        /// </summary>
        /// <returns>True if registry entry was removed successfully, false otherwise.</returns>
        public static bool RemoveRegistryEntry()
        {
            bool success = false;
            try
            {
                // Try to remove from HKCU first (doesn't require admin rights)
                string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                using (var key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                {
                    if (key != null)
                    {
                        try
                        {
                            key.DeleteValue("GFSetupWizard", false);
                            success = true;
                        }
                        catch (ArgumentException)
                        {
                            // Value doesn't exist in HKCU, which is fine
                        }
                    }
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing registry entry: {ex.Message}");
                return false;
            }
        }
    }
}
