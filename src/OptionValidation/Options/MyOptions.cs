using System.ComponentModel.DataAnnotations;

namespace OptionValidation.Options;

public class MyOptions
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(AllowEmptyStrings = false), MinLength(20), MaxLength(200)]
    public string Setting { get; set; } = string.Empty;
}