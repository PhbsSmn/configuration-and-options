using Microsoft.Extensions.Options;

namespace OptionConfigure.Options;

[OptionsValidator]
public partial class ValidateMyOptions : IValidateOptions<MyOptions>;