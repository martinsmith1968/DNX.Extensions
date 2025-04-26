Write-Host "Executing in ${PSScriptRoot}"

$output_suffix = ".generated.cs"

$template_filename = Join-Path $PSScriptRoot "MathsExtensionsTestsDataSource.cs.template"
$output_filename = "Maths#name#ExtensionsTestsDataSource" + $output_suffix

$hash = @{
    'Byte' = 'byte'
    'SByte' = 'sbyte'

    'DateTime' = 'DateTime'
    'Guid' = 'Guid'

    'Int16' = 'short'
    'UInt16' = 'ushort'

    'Int32' = 'int'
    'UInt32' = 'uint'

    'Int64' = 'long'
    'UInt64' = 'ulong'

    'Float' = 'float'

    'Double' = 'double'
    'Decimal' = 'decimal'
}

function Substitute($text, $type, $name) {
    return $text.replace("#type#", $type).replace("#name#", $name)
}

Remove-Item -Path (Join-Path $PSScriptRoot ("*" + $output_suffix))

foreach($entry in $hash.GetEnumerator())
{
    $target_filename = Join-Path $PSScriptRoot (Substitute $output_filename $entry.Value $entry.Key)

    Write-Host "Building $($entry.Key) -> ${target_filename}"

    $source_code = Get-Content $template_filename -Raw
    $generated_code = Substitute $source_code $entry.Value $entry.Key
    Set-Content -Path $target_filename -Value $generated_code
}

Write-Host
Write-Host "$($hash.Count) definitions generated"
