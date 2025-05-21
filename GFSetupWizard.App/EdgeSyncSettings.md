# Edge Sync Settings Launcher

This document explains the different methods available for launching Microsoft Edge with the sync settings URL.

## Overview

The GF Setup Wizard provides multiple methods for launching Microsoft Edge with the sync settings URL (`edge://settings/profiles/sync`). These methods are designed to be robust and provide fallback options if one method fails.

## Available Methods

### 1. Direct Launch

The most straightforward method is to launch Edge directly with the sync settings URL. This is attempted first and works in most cases.

```csharp
Process.Start(new ProcessStartInfo
{
    FileName = "msedge",
    Arguments = "edge://settings/profiles/sync",
    UseShellExecute = true
});
```

### 2. Protocol Handler (gfedgesync://)

A custom protocol handler (`gfedgesync://`) is registered in the Windows Registry. This allows launching Edge with the sync settings URL from anywhere in the system, including other applications.

To set up the protocol handler, click the "Setup Edge Sync Shortcut" button in the Edge Setup Step view. This will create the necessary registry entries.

Once set up, you can use the "Launch Edge Sync Settings" button to launch Edge with the sync settings URL using the protocol handler.

### 3. Batch File

A batch file (`EdgeSyncShortcut.bat`) is provided that launches Edge with the sync settings URL. This is a simple script that can be run from the command line or by double-clicking on it in File Explorer.

```batch
@echo off
echo Launching Microsoft Edge with sync settings...
start msedge edge://settings/profiles/sync
```

### 4. PowerShell Script

A PowerShell script (`LaunchEdgeSyncSettings.ps1`) is provided that launches Edge with the sync settings URL. This script includes error handling and fallback options.

```powershell
# Launch Microsoft Edge with sync settings
Write-Host "Launching Microsoft Edge with sync settings..."

# Try to launch Edge directly with the sync settings URL
try {
    Start-Process "msedge" -ArgumentList "edge://settings/profiles/sync"
}
catch {
    Write-Host "Error launching Edge directly: $_"
    
    # Try using the protocol handler if it's registered
    try {
        Start-Process "gfedgesync://"
    }
    catch {
        Write-Host "Error launching Edge via protocol handler: $_"
        
        # Last resort fallback: Try using cmd to start Edge with the sync URL
        try {
            Start-Process "cmd.exe" -ArgumentList "/c start msedge edge://settings/profiles/sync"
        }
        catch {
            Write-Host "Error launching Edge via cmd: $_"
            Write-Host "All launch methods failed. Please open Edge manually and navigate to edge://settings/profiles/sync"
        }
    }
}
```

### 5. HTML Redirect

An HTML file (`EdgeSyncRedirect.html`) is provided that attempts to redirect to the Edge sync settings URL. This can be opened in any web browser, and it will attempt to redirect to the Edge sync settings URL.

If the automatic redirection doesn't work, the HTML file includes a button that can be clicked to try again, as well as manual instructions for navigating to the sync settings in Edge.

## Troubleshooting

If you're having trouble launching Edge with the sync settings URL, try the following:

1. Make sure Edge is installed and up to date.
2. Try using a different method from the list above.
3. If all methods fail, open Edge manually and navigate to `edge://settings/profiles/sync` in the address bar.
4. If you're still having trouble, contact your IT support team for assistance.

## Technical Details

The GF Setup Wizard uses a fallback approach when launching Edge with the sync settings URL. It tries each method in sequence until one succeeds. If all methods fail, it falls back to launching Edge without a specific URL.

The `SystemApplicationLauncher.LaunchEdgeSyncSettings()` method in the `GFSetupWizard.SystemIntegration` namespace handles this fallback logic.
