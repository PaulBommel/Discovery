using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public class TimespanToSecondsConverterAttribute : JsonConverterAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="typeToConvert"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            if (!typeof(TimeSpan).IsAssignableFrom(typeToConvert))
                throw new Exception("TimeSpan");
            return new TimespanToSecondsConverter();
        }
    }

    public class TimespanToSecondsConverter : JsonConverter<TimeSpan>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="typeToConvert"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool CanConvert(Type typeToConvert)
            => typeof(TimeSpan).IsAssignableFrom(typeToConvert);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="reader"><inheritdoc/></param>
        /// <param name="typeToConvert"><inheritdoc/></param>
        /// <param name="options"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.FromSeconds(reader.GetInt64());
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="writer"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <param name="options"><inheritdoc/></param>
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.TotalSeconds);
        }
    }
}
