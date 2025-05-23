using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace GFSetupWizard.SystemIntegration
{
    public static class SystemApplicationLauncher
    {
        /// <summary>
        /// Checks if OneDrive is configured on the system.
        /// </summary>
        /// <returns>True if OneDrive is configured, false otherwise.</returns>
        public static bool IsOneDriveConfigured()
        {
            try
            {
                // Check if OneDrive folder exists in user profile
                string oneDrivePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "OneDrive - GlobalFoundries");
                
                // Also check for the OneDrive process running
                bool processRunning = Process.GetProcessesByName("OneDrive").Length > 0;
                
                // Consider OneDrive configured if both the folder exists and the process is running
                return Directory.Exists(oneDrivePath) && processRunning;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking OneDrive configuration: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Launches OneDrive and shows appropriate feedback based on whether OneDrive is configured or not.
        /// </summary>
        /// <returns>True if OneDrive was launched successfully, false otherwise.</returns>
        public static bool LaunchOneDriveWithFeedback()
        {
            // Check if OneDrive is already configured
            bool isConfigured = IsOneDriveConfigured();
            
            // Launch OneDrive
            bool success = LaunchOneDrive();
            
            if (success)
            {
                // Show different messages based on whether OneDrive is configured or not
                if (isConfigured)
                {
                    MessageBox.Show(
                        "OneDrive has been launched. The OneDrive application should now be running.",
                        "OneDrive Launched",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "OneDrive setup has been initiated. Please follow the on-screen instructions to configure OneDrive.",
                        "OneDrive Setup",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Unable to launch OneDrive automatically. Please open it manually from the Start menu.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            
            return success;
        }

        public static bool LaunchOneDrive()
        {
            // Check if OneDrive is already configured
            bool isConfigured = IsOneDriveConfigured();
            
            try
            {
                if (isConfigured)
                {
                    // If OneDrive is configured, launch the OneDrive application directly
                    try
                    {
                        // Method 1: Try to launch OneDrive directly using the executable path
                        string oneDrivePath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "Microsoft", "OneDrive", "OneDrive.exe");
                        
                        if (File.Exists(oneDrivePath))
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = oneDrivePath,
                                UseShellExecute = true
                            });
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error launching OneDrive executable: {ex.Message}");
                    }
                    
                    // Method 2: Try to launch OneDrive using the shell command
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = "shell:AppsFolder\\Microsoft.SkyDrive_8wekyb3d8bbwe!Microsoft.SkyDrive.Desktop",
                            UseShellExecute = true
                        };
                        
                        Process.Start(startInfo);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error launching OneDrive via shell: {ex.Message}");
                    }
                    
                    // Method 3: Try to launch OneDrive using the protocol handler
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/c start onedrive:",
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        
                        Process.Start(startInfo);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error launching OneDrive via protocol: {ex.Message}");
                        return false;
                    }
                }
                else
                {
                    // If OneDrive is not configured, launch the OneDrive setup process
                    // Based on testing, only Method 1 (direct executable path from Program Files) works reliably
                    // for initiating the OneDrive setup process on unconfigured systems
                    
                    // Method 1: Launch OneDrive setup using the executable path in Program Files
                    try
                    {
                        string oneDriveSetupPath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                            "Microsoft OneDrive", "OneDrive.exe");
                        
                        if (File.Exists(oneDriveSetupPath))
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = oneDriveSetupPath,
                                UseShellExecute = true
                            });
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("OneDrive executable not found in Program Files.");
                            
                            // Fallback to Method 2 only if Method 1 fails due to file not found
                            string oneDriveSetupPathX86 = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                "Microsoft OneDrive", "OneDrive.exe");
                            
                            if (File.Exists(oneDriveSetupPathX86))
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = oneDriveSetupPathX86,
                                    UseShellExecute = true
                                });
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("OneDrive executable not found in Program Files (x86) either.");
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error launching OneDrive setup from Program Files: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LaunchOneDrive: {ex.Message}");
                return false;
            }
            
            // If we reach here, all methods failed
            return false;
        }

        public static bool LaunchOutlook()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "outlook",
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Outlook: {ex.Message}");
                return false;
            }
        }

        public static bool LaunchTeams()
        {
            try
            {
                // Use the shell command approach that works for Outlook
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = "shell:AppsFolder\\MSTeams_8wekyb3d8bbwe!MSTeams",
                    UseShellExecute = true
                };
                
                Process.Start(startInfo);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Teams: {ex.Message}");
                
                try
                {
                    // Try alternative method using protocol handler
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c start msteams:",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    
                    Process.Start(startInfo);
                    return true;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Error launching Teams (alternative method): {ex2.Message}");
                    return false;
                }
            }
        }

        public static bool LaunchSoftwareCenter()
        {
            try
            {
                // Path to Software Center executable - specifically using the SCClient.exe in ClientUX folder
                // as requested to ensure the correct version is launched
                string softwareCenterPath = @"C:\Windows\CCM\ClientUX\SCClient.exe";
                
                if (File.Exists(softwareCenterPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = softwareCenterPath,
                        UseShellExecute = true
                    });
                    return true;
                }
                else
                {
                    Console.WriteLine("Software Center executable not found at expected location.");
                    
                    // Try alternative methods to launch Software Center
                    try
                    {
                        // Try using the shell command
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/c start softwarecenter:",
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        
                        Process.Start(startInfo);
                        return true;
                    }
                    catch (Exception altEx)
                    {
                        Console.WriteLine($"Error launching Software Center (alternative method): {altEx.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Software Center: {ex.Message}");
                
                // Try alternative methods to launch Software Center
                try
                {
                    // Try using the shell command
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c start softwarecenter:",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    
                    Process.Start(startInfo);
                    return true;
                }
                catch (Exception altEx)
                {
                    Console.WriteLine($"Error launching Software Center (alternative method): {altEx.Message}");
                    return false;
                }
            }
        }

        public static bool OpenServicePortal()
        {
            try
            {
                // Open service portal URL in default browser
                string servicePortalUrl = "https://globalfoundries.service-now.com/esc";
                
                Process.Start(new ProcessStartInfo
                {
                    FileName = servicePortalUrl,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening Service Portal: {ex.Message}");
                return false;
            }
        }
        
        public static bool OpenRsaTokenRequestForVpn()
        {
            try
            {
                // Open RSA token request URL in default browser
                string rsaTokenRequestUrl = "https://globalfoundries.service-now.com/esc?id=sc_cat_item&sys_id=879725cf2b7920002c83a15069da15bf&table=sc_cat_item&searchTerm=secur";
                
                Process.Start(new ProcessStartInfo
                {
                    FileName = rsaTokenRequestUrl,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening RSA token request: {ex.Message}");
                return false;
            }
        }
        
        public static bool OpenSoftwareRequestPortal()
        {
            try
            {
                // Open software request URL in default browser
                string softwareRequestUrl = "https://globalfoundries.service-now.com/esc?id=sc_cat_item&sys_id=b6a36416242301005f424364fd5576e3&table=sc_cat_item&searchTerm=software";
                
                Process.Start(new ProcessStartInfo
                {
                    FileName = softwareRequestUrl,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening software request portal: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Launches Edge and uses InputSimulator to automatically navigate to the sync settings page.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool LaunchEdgeWithInputSimulator()
        {
            try
            {
                // Launch Edge browser
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "msedge",
                    UseShellExecute = true
                });
                
                if (process == null)
                {
                    Console.WriteLine("Failed to start Edge process.");
                    return false;
                }
                
                // Create a new InputSimulator instance
                var simulator = new InputSimulator();
                
                // Wait longer for Edge to initialize (3 seconds instead of 2)
                Task.Delay(3000).Wait();
                
                // Press Ctrl+L to focus the address bar
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_L);
                
                // Small delay after focusing
                Task.Delay(200).Wait();
                
                // Type the sync settings URL and press Enter
                simulator.Keyboard.TextEntry("edge://settings/profiles/sync");
                Task.Delay(200).Wait();
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LaunchEdgeWithInputSimulator: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Copies the Edge sync settings URL to the clipboard.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool CopyEdgeSyncUrlToClipboard()
        {
            try
            {
                // Set the URL to clipboard
                string syncUrl = "edge://settings/profiles/sync";
                Clipboard.SetText(syncUrl);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying Edge sync URL to clipboard: {ex.Message}");
                return false;
            }
        }
    }
}
