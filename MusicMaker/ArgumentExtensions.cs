using CommandLineParser.Arguments;

namespace MusicMaker
{
    public static class ArgumentExtensions
    {
        public static T ParseWithDefault<T>(this EnumeratedValueArgument<string> arg, T defaultValue)
            where T : struct, IConvertible, IComparable, IFormattable // T must be an enum, but there is no enum constraint
        {
            if (arg.Parsed && Enum.TryParse<T>(arg.Value, true, out T value))
            {
                return value;
            }

            return defaultValue;
        }
    }
}
