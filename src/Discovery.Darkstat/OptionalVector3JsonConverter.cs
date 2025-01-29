using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    public class OptionalVector3JsonConverterAttribute : JsonConverterAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="typeToConvert"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            if (!typeof(Vector3?).IsAssignableFrom(typeToConvert))
                throw new Exception("Vector3");
            return new OptionalVector3JsonConverter();
        }
    }

    public class OptionalVector3JsonConverter : JsonConverter<Vector3?>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="typeToConvert"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool CanConvert(Type typeToConvert)
            => typeof(Vector3?).IsAssignableFrom(typeToConvert);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="reader"><inheritdoc/></param>
        /// <param name="typeToConvert"><inheritdoc/></param>
        /// <param name="options"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override Vector3? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonDocument.TryParseValue(ref reader, out var doc))
            {
                if (doc.RootElement.TryGetProperty(nameof(Vector3.X), out var x) 
                 && doc.RootElement.TryGetProperty(nameof(Vector3.Y), out var y)
                 && doc.RootElement.TryGetProperty(nameof(Vector3.Z), out var z))
                    return new Vector3(x.GetSingle(), y.GetSingle(), z.GetSingle());
            }
            return null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="writer"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <param name="options"><inheritdoc/></param>
        public override void Write(Utf8JsonWriter writer, Vector3? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStartObject();
                writer.WriteNumber(nameof(Vector3.X), value.Value.X);
                writer.WriteNumber(nameof(Vector3.Y), value.Value.Y);
                writer.WriteNumber(nameof(Vector3.Z), value.Value.Z);
                writer.WriteEndObject();
            }
            else writer.WriteNullValue();
        }
    }
}
