$ErrorActionPreference = "Stop"
$sleep_time = 100

#----------------------------------------------------------------------------
# Update-TagsInText
#
# Use a dictionary of string pairs to replace placeholders in a text string.
#----------------------------------------------------------------------------
function Update-TagsInText{
    param(
        [string]$Text,
        [hashtable]$Tags,
        [string]$Prefix = "#",
        [string]$Suffix = "#"
    )

    foreach ($tag in $Tags.GetEnumerator()) {
        $Text = $Text -replace [regex]::Escape($Prefix + $tag.Key + $Suffix), $tag.Value
    }
    return $Text
}

#----------------------------------------------------------------------------
# Write-HostBanner
#
# Write a head banner to the console with a given text.
#----------------------------------------------------------------------------
function Write-HostBanner {
    param(
        [string]$Text
    )

    $length = (($Text.Length + 4), 70 | Measure -Max).Maximum
    $line = "-" * $length

    Write-Host $line      -ForegroundColor Yellow
    Write-Host "-- $Text" -ForegroundColor Yellow
    Write-Host $line      -ForegroundColor Yellow
}

#----------------------------------------------------------------------------

$verbose           = $True
$template_wildcard = "*.template"
$data_filename     = "BuildData.ps1"

Set-Location -Path $PSScriptRoot

$templates = Get-Childitem -Path $PSScriptRoot -Include $template_wildcard -Recurse
Write-Host "$($templates.Length) templates found"

  if ($verbose -eq $True) {
    Write-HostBanner "Templates"

    $index = 0
    foreach ($template in $templates) {
      ++$index
      Write-Host "${index}: Template: $($template.FullName)"
    }
  }


foreach ($template in $templates) {
  Write-Host
  Write-HostBanner "Processing: $($template.FullName)"

  Push-Location -Path $template.DirectoryName

  if ( -Not (Test-Path -Path $data_filename)) {
    throw "Template data file not found: $(Join-Path $template.DirectoryName $data_filename)"
  }

  # Preset variables to be populated
  $output_filename_template = "";
  $build_data = @{ }
  $Tag_Prefix = "#"
  $Tag_Suffix = "#"

  Write-Host "-- Loading template data from: $data_filename"
  . "./$data_filename"

  # Validate variables were set
  if ($output_filename_template -eq "") {
    throw "Build data script did not set the output filename template: $data_filename"
  }

  if ($build_data.Count -eq 0) {
    throw "Build data script did not define any output files: $data_filename"
  }

  # Remove previous generated files
  $wildcard = Update-TagsInText -Text $output_filename_template -Tags @{ "Name" = "*" } -Prefix $Tag_Prefix -Suffix $Tag_Suffix
  if ($verbose -eq $True) {
    Write-Host "-- Removing previous generated files matching: $wildcard"
  }
  Remove-Item -Path (Join-Path $template.DirectoryName $wildcard) -ErrorAction SilentlyContinue

  $template_content = (Get-Content -Path $template.FullName -Raw)
  Start-Sleep -Milliseconds $sleep_time

  # Iterate each definition
  Write-Host "-- Generating $($build_data.Count) files"
  foreach($definition in $build_data.GetEnumerator())
  {
    $output_filename = Update-TagsInText -Text $output_filename_template -Tags $definition.Value

    Write-Host "-- Building: $($definition.Key) -> ${output_filename}"
    $output_content = Update-TagsInText -Text $template_content -Tags $definition.Value -Prefix $Tag_Prefix -Suffix $Tag_Suffix
    $output_path = Join-Path $template.DirectoryName $output_filename
    [System.IO.File]::WriteAllText($output_path, $output_content, [System.Text.UTF8Encoding]::new($false))
    Start-Sleep -Milliseconds $sleep_time
  }

  Pop-Location
}

Write-Host
Write-HostBanner "Finished - $($templates.Length) templates processed"
