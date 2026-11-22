$Tag_Prefix = "#"
$Tag_Suffix = "#"

$output_filename_template = "Maths#Name#Extensions.generated.cs"

$build_data = @{
    "DateTime" = @{ "Name" = "DateTime"; "Type" = "DateTime" }
    "DateTimeOffset" = @{ "Name" = "DateTimeOffset"; "Type" = "DateTimeOffset" }
    "TimeSpan" = @{ "Name" = "TimeSpan"; "Type" = "TimeSpan" }

    "Guid" = @{ "Name" = "Guid"; "Type" = "Guid" }
    "BigInteger" = @{ "Name" = "BigInteger"; "Type" = "System.Numerics.BigInteger" }

    "Byte" = @{ "Name" = "Byte"; "Type" = "byte" }
    "SByte" = @{ "Name" = "SByte"; "Type" = "sbyte" }

    "Int16" = @{ "Name" = "Int16"; "Type" = "short" }
    "UInt16" = @{ "Name" = "UInt16"; "Type" = "ushort" }

    "Int32" = @{ "Name" = "Int32"; "Type" = "int" }
    "UInt32" = @{ "Name" = "UInt32"; "Type" = "uint" }

    "Int64" = @{ "Name" = "Int64"; "Type" = "long" }
    "UInt64" = @{ "Name" = "UInt64"; "Type" = "ulong" }

    "Float" = @{ "Name" = "Float"; "Type" = "float" }
    "Double" = @{ "Name" = "Double"; "Type" = "double" }
    "Decimal" = @{ "Name" = "Decimal"; "Type" = "decimal" }
}
