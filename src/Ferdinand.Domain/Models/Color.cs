using System.Text.RegularExpressions;
using Ferdinand.Domain.Events;
using Ferdinand.Domain.Primitives;
using Throw;

namespace Ferdinand.Domain.Models;

public sealed class Color : AggregateRoot<ColorKey>
{
    private Color(
        Tenant tenant,
        ColorKey key,
        string hexValue,
        string description = "")
    {
        Tenant = tenant;
        Key = key;
        HexValue = hexValue;
        Description = description;
    }
    
    public Tenant Tenant { get; private set; }
    public string HexValue { get; private set; } 
    public string Description { get; private set; }

    public static Color FromHexValue(Tenant tenant, string hexValue, string description = "")
    {
        hexValue.Throw(() => new DomainException())
            .IfEmpty()
            .IfWhiteSpace()
            .IfLengthNotEquals(6)
            .IfNotMatches(new Regex(@"^[0-9a-fA-F]{6}$"));

        var color = new Color(
            tenant,
            ColorKey.CreateUnique(),
            hexValue,
            description);

        color.RaiseEvent(new ColorAdded(color.Tenant.Value, color.HexValue));

        return color;
    }
}
