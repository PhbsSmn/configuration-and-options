using System.ComponentModel.DataAnnotations;

namespace OptionConfigure.Options;

public class MyOptions
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(AllowEmptyStrings = false), MinLength(5), MaxLength(200)]
    public string Setting { get; set; } = string.Empty;

    public List<string> Order { get; set; } = [];
}
