using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HotelManagement.WebApi.Validators;

public class CountryCodeAttribute : ValidationAttribute
{
    // ISO 3166-1
    private static readonly HashSet<string> ValidCountryCodes =
        CultureInfo.GetCultures( CultureTypes.SpecificCultures )
            .Select( culture =>
            {
                try
                {
                    return new RegionInfo( culture.Name ).ThreeLetterISORegionName;
                }
                catch ( ArgumentException )
                {
                    // Некоторые культуры не имеют связанного региона ("en-US" имеет, а "fr" - нет)
                    return null;
                }
            } )
            .Where( code => !string.IsNullOrEmpty( code ) )
            .Select( code => code!.ToUpperInvariant() )
            .ToHashSet( StringComparer.OrdinalIgnoreCase );

    public CountryCodeAttribute() : base( "The field {0} must be a valid ISO 3166-1 country code." ) { }

    public CountryCodeAttribute( string errorMessage ) : base( errorMessage ) { }

    protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
    {
        if ( value == null )
        {
            return ValidationResult.Success;
        }

        string code = value.ToString()!.Trim().ToUpperInvariant();
        return ValidCountryCodes.Contains( code )
            ? ValidationResult.Success
            : new ValidationResult( FormatErrorMessage( validationContext.DisplayName ) );
    }
}