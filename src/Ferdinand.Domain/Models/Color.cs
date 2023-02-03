using System.Text.RegularExpressions;
using Ferdinand.Domain.Primitives;
using Throw;

namespace Ferdinand.Domain.Models;

public sealed class Color : AggregateRoot<ColorId>
{
    private Color() { } 
    
    public Tenant Tenant { get; private set; }
    public string HexValue { get; private set; } 
    public string Description { get; private set; }

    public static Color FromHexValue(Tenant tenant, string hexValue, string description = "")
    {
        hexValue.Throw(() => new DomainException())
            .IfEmpty()
            .IfWhiteSpace()
            .IfLengthNotEquals(6)
            .IfNotMatches(new Regex("[0-9a-fA-F]{6}"));

        return new Color {
            Tenant = tenant,
            Key = ColorId.CreateUnique(),
            HexValue = hexValue,
            Description = description
        };
    }
}
