# Define the path to your assembly file
$filePath = "C:\Users\XM\Documents\GitHub\ColossalCheatMenu\ColossalCheatMenuV2\bin\Release\ColossalCheatMenuV2.dll"  # Change this to your actual file path

# Check if the file exists
if (Test-Path $filePath) {
    # Calculate the hash
    $hash = Get-FileHash -Path $filePath -Algorithm SHA256

    # Output the hash
    Write-Host "Assembly Hash: $($hash.Hash)"
} else {
    Write-Host "File not found: $filePath"
}
