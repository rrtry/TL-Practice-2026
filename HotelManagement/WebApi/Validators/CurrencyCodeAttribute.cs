using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HotelManagement.WebApi.Validators;

public class CurrencyCodeAttribute : ValidationAttribute
{
    // ISO 4217
    private static readonly HashSet<string> ValidCurrencyCodes =
        CultureInfo.GetCultures( CultureTypes.SpecificCultures )
            .Select( culture =>
            {
                try
                {
                    return new RegionInfo( culture.Name ).ISOCurrencySymbol;
                }
                catch ( ArgumentException )
                {
                    return null;
                }
            } )
            .Where( code => !string.IsNullOrEmpty( code ) )
            .Select( code => code!.ToUpperInvariant() )
            .ToHashSet( StringComparer.OrdinalIgnoreCase );

    public CurrencyCodeAttribute() : base( "The field {0} must be a valid ISO 4217 currency code." ) { }

    public CurrencyCodeAttribute( string errorMessage ) : base( errorMessage ) { }

    protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
    {
        if ( value == null )
        {
            return ValidationResult.Success;
        }

        string code = value.ToString()!.Trim().ToUpperInvariant();
        return ValidCurrencyCodes.Contains( code )
            ? ValidationResult.Success
            : new ValidationResult( FormatErrorMessage( validationContext.DisplayName ) );
    }
}