using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace GFSetupWizard.SystemIntegration
{
    /// <summary>
    /// Manages Microsoft Edge profile detection and status checking
    /// </summary>
    public static class EdgeProfileManager
    {
        // Company email domain to identify work accounts
        private const string CompanyDomain = "@globalfoundries.com";
        
        // Edge user data paths
        private static readonly string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string EdgeUserDataPath = Path.Combine(LocalAppData, "Microsoft", "Edge", "User Data");
        private static readonly string LocalStatePath = Path.Combine(EdgeUserDataPath, "Local State");
        
        /// <summary>
        /// Represents an Edge profile with its details
        /// </summary>
        public class EdgeProfile
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public bool IsWorkAccount { get; set; }
            public bool IsSyncEnabled { get; set; }
            public string ProfilePath { get; set; }
        }
        
        /// <summary>
        /// Represents the overall Edge profile status
        /// </summary>
        public class EdgeProfileStatus
        {
            public bool IsEdgeInstalled { get; set; }
            public List<EdgeProfile> Profiles { get; set; } = new List<EdgeProfile>();
            public bool HasWorkAccount => Profiles.Exists(p => p.IsWorkAccount);
            public bool HasWorkAccountWithSync => Profiles.Exists(p => p.IsWorkAccount && p.IsSyncEnabled);
            public string StatusMessage { get; set; }
        }
        
        /// <summary>
        /// Checks if Edge is installed on the system
        /// </summary>
        public static bool IsEdgeInstalled()
        {
            try
            {
                // Check if Edge executable exists in Program Files
                string edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), 
                    "Microsoft", "Edge", "Application", "msedge.exe");
                
                if (File.Exists(edgePath))
                    return true;
                
                // Check if Edge executable exists in Program Files (x64)
                edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), 
                    "Microsoft", "Edge", "Application", "msedge.exe");
                
                if (File.Exists(edgePath))
                    return true;
                
                // Check registry for Edge installation
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe"))
                {
                    return key != null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking Edge installation: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Gets the complete Edge profile status
        /// </summary>
        public static EdgeProfileStatus GetEdgeProfileStatus()
        {
            var status = new EdgeProfileStatus
            {
                IsEdgeInstalled = IsEdgeInstalled()
            };
            
            if (!status.IsEdgeInstalled)
            {
                status.StatusMessage = "Microsoft Edge is not installed on this system.";
                return status;
            }
            
            try
            {
                // Check if Edge user data directory exists
                if (!Directory.Exists(EdgeUserDataPath))
                {
                    status.StatusMessage = "Edge has been installed but not yet launched. No profile data found.";
                    return status;
                }
                
                // Check if Local State file exists
                if (!File.Exists(LocalStatePath))
                {
                    status.StatusMessage = "Edge profile data is incomplete. Try launching Edge first.";
                    return status;
                }
                
                // Parse the Local State file to get profile information
                status.Profiles = ParseLocalStateFile();
                
                // Check each profile directory for additional information
                foreach (var profile in status.Profiles)
                {
                    EnrichProfileWithPreferencesData(profile);
                }
                
                // Set appropriate status message
                if (status.HasWorkAccountWithSync)
                {
                    status.StatusMessage = "Edge is properly configured with a work account and sync is enabled.";
                }
                else if (status.HasWorkAccount)
                {
                    status.StatusMessage = "Edge has a work account but sync may not be enabled.";
                }
                else if (status.Profiles.Count > 0)
                {
                    status.StatusMessage = "Edge has profiles but no work account was detected.";
                }
                else
                {
                    status.StatusMessage = "Edge is installed but no profiles were detected.";
                }
                
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Edge profile status: {ex.Message}");
                status.StatusMessage = $"Error checking Edge profiles: {ex.Message}";
                return status;
            }
        }
        
        /// <summary>
        /// Parses the Local State file to extract profile information
        /// </summary>
        private static List<EdgeProfile> ParseLocalStateFile()
        {
            var profiles = new List<EdgeProfile>();
            
            try
            {
                // Read and parse the Local State file
                string localStateJson = File.ReadAllText(LocalStatePath);
                using (JsonDocument doc = JsonDocument.Parse(localStateJson))
                {
                    // Check if the profile section exists
                    if (doc.RootElement.TryGetProperty("profile", out JsonElement profileElement))
                    {
                        // Check if the info_cache section exists
                        if (profileElement.TryGetProperty("info_cache", out JsonElement infoCache))
                        {
                            // Iterate through each profile in the info_cache
                            foreach (JsonProperty profile in infoCache.EnumerateObject())
                            {
                                string profileName = profile.Name;
                                string profilePath = Path.Combine(EdgeUserDataPath, profileName);
                                
                                // Extract profile information
                                string email = string.Empty;
                                if (profile.Value.TryGetProperty("user_name", out JsonElement userName))
                                {
                                    email = userName.GetString() ?? string.Empty;
                                }
                                
                                // Create profile object
                                var edgeProfile = new EdgeProfile
                                {
                                    Name = profileName,
                                    Email = email,
                                    IsWorkAccount = !string.IsNullOrEmpty(email) && email.EndsWith(CompanyDomain, StringComparison.OrdinalIgnoreCase),
                                    IsSyncEnabled = false, // Will be updated in EnrichProfileWithPreferencesData
                                    ProfilePath = profilePath
                                };
                                
                                profiles.Add(edgeProfile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing Local State file: {ex.Message}");
            }
            
            return profiles;
        }
        
        /// <summary>
        /// Enriches profile information with data from the Preferences file
        /// </summary>
        private static void EnrichProfileWithPreferencesData(EdgeProfile profile)
        {
            try
            {
                string preferencesPath = Path.Combine(profile.ProfilePath, "Preferences");
                
                if (File.Exists(preferencesPath))
                {
                    string preferencesJson = File.ReadAllText(preferencesPath);
                    using (JsonDocument doc = JsonDocument.Parse(preferencesJson))
                    {
                        // Check if sync is enabled
                        if (doc.RootElement.TryGetProperty("sync", out JsonElement syncElement))
                        {
                            if (syncElement.TryGetProperty("sync_promo", out JsonElement syncPromo))
                            {
                                if (syncPromo.TryGetProperty("user_skipped_signin", out JsonElement userSkippedSignin))
                                {
                                    // If user skipped sign-in, sync is not enabled
                                    profile.IsSyncEnabled = !userSkippedSignin.GetBoolean();
                                }
                            }
                            
                            // Check if sync is explicitly enabled
                            if (syncElement.TryGetProperty("sync_everything", out JsonElement syncEverything))
                            {
                                profile.IsSyncEnabled = syncEverything.GetBoolean();
                            }
                        }
                        
                        // Check for account info if email is not already set
                        if (string.IsNullOrEmpty(profile.Email))
                        {
                            if (doc.RootElement.TryGetProperty("account_info", out JsonElement accountInfo))
                            {
                                if (accountInfo.TryGetProperty("email", out JsonElement email))
                                {
                                    profile.Email = email.GetString() ?? string.Empty;
                                    profile.IsWorkAccount = !string.IsNullOrEmpty(profile.Email) && 
                                        profile.Email.EndsWith(CompanyDomain, StringComparison.OrdinalIgnoreCase);
                                }
                            }
                        }
                        
                        // Check for sync tokens which indicate active sync
                        if (doc.RootElement.TryGetProperty("signin", out JsonElement signin))
                        {
                            if (signin.TryGetProperty("signedin_time", out JsonElement _))
                            {
                                // If signedin_time exists, the profile is signed in
                                profile.IsSyncEnabled = true;
                            }
                        }
                    }
                }
                
                // Check for Sync Data directory which indicates sync is or was enabled
                string syncDataPath = Path.Combine(profile.ProfilePath, "Sync Data");
                if (Directory.Exists(syncDataPath) && Directory.GetFiles(syncDataPath).Length > 0)
                {
                    profile.IsSyncEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enriching profile data: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Checks if Edge has any work accounts configured
        /// </summary>
        public static bool HasWorkAccount()
        {
            var status = GetEdgeProfileStatus();
            return status.HasWorkAccount;
        }
        
        /// <summary>
        /// Checks if Edge has any work accounts with sync enabled
        /// </summary>
        public static bool HasWorkAccountWithSync()
        {
            var status = GetEdgeProfileStatus();
            return status.HasWorkAccountWithSync;
        }
    }
}
