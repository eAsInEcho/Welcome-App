using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace GFSetupWizard.SystemIntegration
{
    public static class SystemApplicationLauncher
    {
        public static bool LaunchOneDrive()
        {
            try
            {
                // Path to OneDrive executable
                string oneDrivePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "OneDrive", "OneDrive.exe");
                
                if (File.Exists(oneDrivePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = oneDrivePath,
                        UseShellExecute = true
                    });
                    return true;
                }
                else
                {
                    Console.WriteLine("OneDrive executable not found at expected location.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching OneDrive: {ex.Message}");
                return false;
            }
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
                // Try to launch Teams from typical installation paths
                string teamsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Teams", "current", "Teams.exe");
                
                if (File.Exists(teamsPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = teamsPath,
                        UseShellExecute = true
                    });
                    return true;
                }
                else
                {
                    // Try alternative method using shell command
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "teams",
                        UseShellExecute = true
                    });
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Teams: {ex.Message}");
                return false;
            }
        }

        public static bool LaunchEdge()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "msedge",
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Edge: {ex.Message}");
                return false;
            }
        }

        public static bool LaunchSoftwareCenter()
        {
            try
            {
                // Path to Software Center executable
                string softwareCenterPath = @"C:\Windows\CCM\SCClient.exe";
                
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Software Center: {ex.Message}");
                return false;
            }
        }

        public static bool OpenServicePortal()
        {
            try
            {
                // Open service portal URL in default browser
                string servicePortalUrl = "https://serviceportal.globalfoundries.com";
                
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
    }
}
