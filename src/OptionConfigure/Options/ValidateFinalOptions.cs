using Microsoft.Extensions.Options;

namespace OptionConfigure.Options;

[OptionsValidator]
public partial class ValidateFinalOptions : IValidateOptions<FinalOptions>;