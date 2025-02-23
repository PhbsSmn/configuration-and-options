using System.ComponentModel.DataAnnotations;

public class NonDiClientOptions
{
    public const string CONFIGURATION_SECTION_PATH = "NonDiClient";

    [Required(AllowEmptyStrings = false)]
    public string Key { get; set; }
}