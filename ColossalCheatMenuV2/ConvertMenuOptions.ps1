
# convertMenuOptions.ps1

# Get script directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

# Input and output files
$inputFile = Join-Path $scriptDir "MenuOptions.json"
$outputFile = Join-Path $scriptDir "SS MenuOptions.txt"
$debugLog = Join-Path $scriptDir "parser_debug.log"

# Check if input file exists
if (-not (Test-Path $inputFile)) {
    Write-Error "MenuOptions.json not found in $scriptDir"
    exit 1
}

# Initialize debug log
Set-Content -Path $debugLog -Value "Parsing started: $(Get-Date)"

# Read input file
$jsonContent = Get-Content $inputFile -Raw | ConvertFrom-Json

# Format KeyAuth strings with headers
$outputLines = @()
foreach ($menuName in ($jsonContent.Menus.PSObject.Properties.Name | Sort-Object)) {
    $options = $jsonContent.Menus.$menuName
    $keyAuthOptions = @()

    Add-Content -Path $debugLog -Value "Processing menu: $menuName"

    foreach ($option in $options) {
        # Skip if no Type or Type is none
        if (-not $option.Type -or $option.Type -eq "none") {
            Add-Content -Path $debugLog -Value "Skipped option in $menuName`: $($option.DisplayName) (Type: $($option.Type))"
            continue
        }

        $type = $option.Type
        $displayName = $option.DisplayName

        Add-Content -Path $debugLog -Value "Parsing option: $displayName, Type: $type"

        $optionStr = ""
        if ($type -eq "submenuthingy") {
            $associatedString = $option.AssociatedString
            $optionStr = "$displayName,submenuthingy,$associatedString"
            Add-Content -Path $debugLog -Value "Output submenuthingy: $displayName,$associatedString"
        }
        elseif ($type -eq "togglethingy") {
            $associatedBool = $option.AssociatedBool
            $optionStr = "$displayName,togglethingy,$associatedBool"
            Add-Content -Path $debugLog -Value "Output togglethingy: $displayName,$associatedBool"
        }
        elseif ($type -eq "sliderthingy") {
            $stringArray = $option.StringArray
            $arrayStr = if ($stringArray -and $stringArray.Count -gt 0) { "[" + ($stringArray -join "|") + "]" } else { "[new]" }
            $optionStr = "$displayName,sliderthingy,$arrayStr"
            Add-Content -Path $debugLog -Value "Output sliderthingy: $displayName,$arrayStr"
        }
        elseif ($type -eq "buttonthingy") {
            $associatedString = $option.AssociatedString
            $optionStr = "$displayName,buttonthingy,$associatedString"
            Add-Content -Path $debugLog -Value "Output buttonthingy: $displayName,$associatedString"
        }

        # Append extra if present
        if ($option.Extra -and $option.Extra -ne "") {
            $optionStr += ",$($option.Extra)"
            Add-Content -Path $debugLog -Value "Appended extra: $($option.Extra)"
        }

        $keyAuthOptions += $optionStr
    }

    if ($keyAuthOptions) {
        $keyAuthStr = $keyAuthOptions -join ";"
        $outputLines += "---$($menuName.ToUpper())---"
        $outputLines += $keyAuthStr
        $outputLines += ""
        Add-Content -Path $debugLog -Value "Output menu: $menuName"
    }
}

# Write output file
Set-Content -Path $outputFile -Value $outputLines
Add-Content -Path $debugLog -Value "Parsing completed: $(Get-Date)"
Write-Host "Generated $outputFile"
Write-Host "Debug log written to $debugLog"