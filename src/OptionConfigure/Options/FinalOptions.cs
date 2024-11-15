using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace OptionConfigure.Options;

public class FinalOptions
{
    public const string CONFIGURATION_SECTION_PATH = "Final";

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, Length(1, 20), ValidateEnumeratedItems]
    public List<MyOptions> Options { get; set; } = [];
}