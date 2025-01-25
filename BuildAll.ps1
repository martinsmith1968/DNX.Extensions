$scripts = get-childitem -Path $PSScriptRoot -Include Build.ps1 -Recurse

foreach ($script in $scripts) {
  Write-Host
  Write-Host "--------------------------------------------------------------------------------------------------------------------------------------------"
  Write-Host "-- Executing: $($script.FullName)"
  Write-Host "--------------------------------------------------------------------------------------------------------------------------------------------"
  . $script.FullName
}

Write-Host
Write-Host "$($scripts.Length) scripts executed"
