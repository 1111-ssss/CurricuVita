using System.Globalization;
using System.Resources;

namespace Web.Resources;

public class SharedResource
{
    private static readonly ResourceManager ResourceManager =
        new(typeof(SharedResource).FullName!, typeof(SharedResource).Assembly);

    public static string Validation_Required => Get(nameof(Validation_Required));
    public static string Validation_Email => Get(nameof(Validation_Email));

    private static string Get(string name) =>
        ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? name;
}
