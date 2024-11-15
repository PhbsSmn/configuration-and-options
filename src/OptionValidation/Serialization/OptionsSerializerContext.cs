using System.Text.Json.Serialization;
using OptionValidation.Options;

namespace OptionValidation.Serialization;

[JsonSerializable(typeof(MyOptions))]
public partial class OptionsSerializerContext : JsonSerializerContext;