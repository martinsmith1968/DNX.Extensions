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
# Update-TagsInFile
#
# Use a dictionary of string pairs to read and replace placeholders in a text file.
#----------------------------------------------------------------------------
function Update-TagsInFile{
    param(
        [string]$FilePath,
        [hashtable]$Tags,
        [string]$Prefix = "#",
        [string]$Suffix = "#"
    )

    if (-Not ( Test-Path $FilePath -PathType Leaf)) {
        Write-Host "File not found: $FilePath"
        return $False
    }

    $content = Get-Content -Path $FilePath -Raw
    $content = Update-TagsInText -Text $content -Tags $Tags -Prefix $Prefix -Suffix $Suffix
    Set-Content -Path $FilePath -Value $content -NoNewline

    return $True
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

    Write-Host "--------------------------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Yellow
    Write-Host "-- $Text"                                                                                                                                     -ForegroundColor Yellow
    Write-Host "--------------------------------------------------------------------------------------------------------------------------------------------" -ForegroundColor Yellow
}

function Write-HostError {
    param(
        [string]$Text
    )

    Write-Host "ERROR: $Text" -ForegroundColor Red
}

function Write-HostWarning {
    param(
        [string]$Text
    )

    Write-Host "WARNING: $Text" -ForegroundColor Cyan
}

#----------------------------------------------------------------------------

$verbose           = $True
$template_wildcard = "*.template"
$data_filename     = "BuildData.ps1"

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
    Write-HostWarning "Template Data file not found: $(Join-Path $template.DirectoryName $data_filename)"
    continue
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
    Write-HostError "Build Data script did not set the output filename template: $data_filename"
    continue
  }

  # Remove previous generated files
  $wildcard = Update-TagsInText -Text $output_filename_template -Tags @{ "Name" = "*" } -Prefix $Tag_Prefix -Suffix $Tag_Suffix
  if ($verbose -eq $True) {
    Write-Host "-- Removing previous generated files matching: $wildcard"
  }
  Remove-Item -Path $wildcard -ErrorAction SilentlyContinue

  $template_content = Get-Content -Path $template.FullName -Raw

  # Iterate each definition
  Write-Host "-- Generating $($build_data.Count) files"
  foreach($definition in $build_data.GetEnumerator())
  {
    $output_filename = Update-TagsInText -Text $output_filename_template -Tags $definition.Value

    Write-Host "-- Building: $($definition.Key) -> ${output_filename}"
    Set-Content -Path $output_filename -Value $template_content -NoNewline
    if ( -Not (Update-TagsInFile -FilePath $output_filename -Tags $definition.Value -Prefix $Tag_Prefix -Suffix $Tag_Suffix) ) {
      Write-HostError "Failed to update tags in file: $output_filename"
    }
  }

  Pop-Location
}

Write-Host
Write-HostBanner "Finished - $($templates.Length) templates processed"
