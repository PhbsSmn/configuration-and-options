using Microsoft.Extensions.Options;

namespace OptionValidation.Options;

[OptionsValidator]
public partial class ValidateMyOptions : IValidateOptions<MyOptions>;