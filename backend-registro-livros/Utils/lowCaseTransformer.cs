using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace backend_registro_livros.Utils
{
    public class LowerCaseTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            // Transforma o valor em letras minúsculas
            return value == null ? null : Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }

}
