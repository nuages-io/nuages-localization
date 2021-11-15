#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

#endregion

namespace Nuages.Localization.Storage.Config.Sources.FromSDK;

[ExcludeFromCodeCoverage]
public class JsonConfigurationFileParser
{
    private readonly Stack<string> _context = new();

    private readonly IDictionary<string, string> _data =
        new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    private string? _currentPath;
    private string? _prefix;

    private JsonConfigurationFileParser()
    {
    }

    public static IDictionary<string, string> Parse(Stream input, string? prefix = null)
    {
        return new JsonConfigurationFileParser().ParseStream(input, prefix);
    }

    private IDictionary<string, string> ParseStream(Stream input, string? prefix = null)
    {
        _prefix = prefix;

        _data.Clear();

        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        using var reader = new StreamReader(input);
        using var doc = JsonDocument.Parse(reader.ReadToEnd(), jsonDocumentOptions);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
            throw new FormatException(
                "UnsupportedJSONToken" );
        VisitElement(doc.RootElement);

        return _data;
    }

    private void VisitElement(JsonElement element)
    {
        foreach (var property in element.EnumerateObject())
        {
            EnterContext(property.Name);
            VisitValue(property.Value);
            ExitContext();
        }
    }

    private void VisitValue(JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                VisitElement(value);
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var arrayElement in value.EnumerateArray())
                {
                    EnterContext(index.ToString());
                    VisitValue(arrayElement);
                    ExitContext();
                    index++;
                }

                break;

            case JsonValueKind.Number:
            case JsonValueKind.String:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
                var key = (!string.IsNullOrEmpty(_prefix) ?  _prefix  + ":" : "") + _currentPath;
                if (_data.ContainsKey(key))
                    throw new FormatException("KeyIsDuplicated");
                _data[key] = value.ToString() ;
                break;

            case JsonValueKind.Undefined:
                throw new FormatException(
                    "UnsupportedJSONToken");
            default:
                throw new FormatException(
                    "UnsupportedJSONToken");
        }
    }

    private void EnterContext(string context)
    {
        _context.Push(context);
        _currentPath = ConfigurationPath.Combine(_context.Reverse());
    }

    private void ExitContext()
    {
        _context.Pop();
        _currentPath = ConfigurationPath.Combine(_context.Reverse());
    }
}