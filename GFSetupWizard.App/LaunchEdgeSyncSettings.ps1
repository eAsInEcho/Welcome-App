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
