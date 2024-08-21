using DNX.Extensions.Strings;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.Strings;

public class ArgumentParserExtensionsTests
{
    [Theory]
    [InlineData("command value1 value2 --option optionValue", 5, "command|value1|value2|--option|optionValue")]
    [InlineData("command value1 \"value2\" --option 'optionValue'", 5, "command|value1|value2|--option|'optionValue'")]
    [InlineData("command \" value1 has multiple  spaces \" \"value2 contains spaces\" --option 'optionValue'", 5, "command| value1 has multiple  spaces |value2 contains spaces|--option|'optionValue'")]
    [InlineData("--swiftcon \"Server=.\\SQLEXPRESS;Database=Swift;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;\" --swiftsoaphaaddress \"https://127.0.0.1:48200/soapha/\" --swiftencryptedlau mylauwhichshouldbeencrypted --institutionadclientid \"ab988c21-f419-4488-b3d6-a7ffeea63e68\" --institutionadclientsecret \"No8pQsZjBSIGbGMM6KCHf24qPvZ+YnvJKt0cTeQar0g=\" --institutionadtenantname \"cbiuktestinstitution.onmicrosoft.com\" --institutionprincipalcon \"Server=.\\SQLEXPRESS;Database=BankingInstitutionAuthentication;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;\" --institutionprincipalids \"AE261E74-4BDF-470C-9FFD-0227804DD8B9\" \"10246AE7-ED49-41C4-AF25-023521FF3622\"", 17, "--swiftcon|Server=.\\SQLEXPRESS;Database=Swift;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;|--swiftsoaphaaddress|https://127.0.0.1:48200/soapha/|--swiftencryptedlau|mylauwhichshouldbeencrypted|--institutionadclientid|ab988c21-f419-4488-b3d6-a7ffeea63e68|--institutionadclientsecret|No8pQsZjBSIGbGMM6KCHf24qPvZ+YnvJKt0cTeQar0g=|--institutionadtenantname|cbiuktestinstitution.onmicrosoft.com|--institutionprincipalcon|Server=.\\SQLEXPRESS;Database=BankingInstitutionAuthentication;Trusted_Connection=True;ConnectRetryCount=6;ConnectRetryInterval=10;Connection Timeout=30;|--institutionprincipalids|AE261E74-4BDF-470C-9FFD-0227804DD8B9|10246AE7-ED49-41C4-AF25-023521FF3622")]
    [InlineData("Endpoint=sb://#{service_bus_url-prefix}#.servicebus.windows.net/;SharedAccessKeyName=#{servicebus_sas_applications_name}#;SharedAccessKey=#{servicebus_sas_applications_key}#", 1, "Endpoint=sb://#{service_bus_url-prefix}#.servicebus.windows.net/;SharedAccessKeyName=#{servicebus_sas_applications_name}#;SharedAccessKey=#{servicebus_sas_applications_key}#")]
    [InlineData("\"Server=.;Database=JPMEmulator;Integrated Security=True;MultipleActiveResultSets=True\"", 1, "Server=.;Database=JPMEmulator;Integrated Security=True;MultipleActiveResultSets=True")]
    public void When_called_with_a_valid_simple_string_of_values(string text, int parameterCount, string resultsByPipe)
    {
        // Act
        var result = text.ParseArguments();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(parameterCount);

        var parameters = resultsByPipe.Split("|".ToCharArray());
        parameters.Length.Should().Be(parameterCount);

        var parameterPosition = 0;
        foreach (var parameter in parameters)
        {
            result[parameterPosition].Should().Be(parameter);

            ++parameterPosition;
        }
    }
}
