using System.Text.Json.Serialization;
using OptionConfigure.Options;

namespace OptionConfigure.Serialization;

[JsonSerializable(typeof(FinalOptions))]
[JsonSerializable(typeof(MyOptions))]
[JsonSerializable(typeof(Dictionary<string, MyOptions>))]
public partial class OptionsSerializerContext : JsonSerializerContext;
