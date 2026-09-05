using DNX.Extensions.Strings;
using Shouldly;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace DNX.Extensions.Tests.Strings;

public class ArgumentParserExtensionsTests
{
    [Theory]
    [MemberData(nameof(ParseToIndividualArguments_Data))]
    public void When_called_with_a_valid_simple_string_of_values(string text, string[] expectedResults)
    {
        // Act
        var result = text.ParseToIndividualArguments();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(expectedResults.Length);
        result.ShouldBe(expectedResults);
    }

    public static TheoryData<string, string[]> ParseToIndividualArguments_Data()
    {
        return new TheoryData<string, string[]>()
        {
            {
                "command value1 value2 --option optionValue",
                [
                    "command", "value1", "value2", "--option", "optionValue"
                ]
            },
            {
                "command value1 \"value2\" --option 'optionValue'",
                [
                    "command", "value1", "value2", "--option", "'optionValue'"
                ]
            },
            {
                "command \" value1 has multiple  spaces \" \"value2 contains spaces\" --option 'optionValue'",
                [
                    "command", " value1 has multiple  spaces ", "value2 contains spaces", "--option", "'optionValue'"
                ]
            },
            {
                "--swiftcon \"Server=.\\SQLEXPRESS;Database=Swift;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;\" --swiftsoaphaaddress \"https://127.0.0.1:48200/soapha/\" --swiftencryptedlau mylauwhichshouldbeencrypted --institutionadclientid \"ab988c21-f419-4488-b3d6-a7ffeea63e68\" --institutionadclientsecret \"No8pQsZjBSIGbGMM6KCHf24qPvZ+YnvJKt0cTeQar0g=\" --institutionadtenantname \"cbiuktestinstitution.onmicrosoft.com\" --institutionprincipalcon \"Server=.\\SQLEXPRESS;Database=BankingInstitutionAuthentication;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;\" --institutionprincipalids \"AE261E74-4BDF-470C-9FFD-0227804DD8B9\" \"10246AE7-ED49-41C4-AF25-023521FF3622\"",
                [
                    "--swiftcon", "Server=.\\SQLEXPRESS;Database=Swift;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;", "--swiftsoaphaaddress",
                    "https://127.0.0.1:48200/soapha/", "--swiftencryptedlau", "mylauwhichshouldbeencrypted", "--institutionadclientid", "ab988c21-f419-4488-b3d6-a7ffeea63e68",
                    "--institutionadclientsecret", "No8pQsZjBSIGbGMM6KCHf24qPvZ+YnvJKt0cTeQar0g=", "--institutionadtenantname", "cbiuktestinstitution.onmicrosoft.com",
                    "--institutionprincipalcon",
                    "Server=.\\SQLEXPRESS;Database=BankingInstitutionAuthentication;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;",
                    "--institutionprincipalids", "AE261E74-4BDF-470C-9FFD-0227804DD8B9", "10246AE7-ED49-41C4-AF25-023521FF3622"
                ]
            },
            {
                "Endpoint=sb://#{service_bus_url-prefix}#.servicebus.windows.net/;SharedAccessKeyName=#{servicebus_sas_applications_name}#;SharedAccessKey=#{servicebus_sas_applications_key}#",
                [
                    "Endpoint=sb://#{service_bus_url-prefix}#.servicebus.windows.net/;SharedAccessKeyName=#{servicebus_sas_applications_name}#;SharedAccessKey=#{servicebus_sas_applications_key}#"
                ]
            },
            {
                "\"Server=.;Database=JPMEmulator;Integrated Security=True;MultipleActiveResultSets=True\"",
                [
                    "Server=.;Database=JPMEmulator;Integrated Security=True;MultipleActiveResultSets=True"
                ]
            },
            {
                "bob -c 4 \"-a 1\" \"a note\"",
                [
                    "bob", "-c", "4", "-a 1", "a note"
                ]
            }
        };
    }
}
