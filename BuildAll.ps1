$scripts = get-childitem -Path $PSScriptRoot -Include Build.ps1 -Recurse

foreach ($script in $scripts) {
  Write-Host
  Write-Host "--------------------------------------------------------------------------------"
  Write-Host "-- Executing: $($script.FullName)"
  . $script.FullName
}
