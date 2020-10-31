using System.Globalization;
using System.Text;
using Xamarin.Forms.Internals;

namespace Actividad.Extensions
{
    public static class StringExtensions
    {
        public static string RemoverDiacriticos(this string textoConDiacriticos)
        {
            string palabra = textoConDiacriticos.Normalize(NormalizationForm.FormD);
            StringBuilder textoSinDiacriticos = new StringBuilder();

            palabra.ForEach(letra =>
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letra) != UnicodeCategory.NonSpacingMark) textoSinDiacriticos.Append(letra);
            });

            return textoSinDiacriticos.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
