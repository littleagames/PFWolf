namespace LittleAGames.PFWolf.SDK.Utilities;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IntOrStringConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            // If the token is a string, try to parse it as an integer
            if (int.TryParse(reader.GetString(), out var result))
            {
                return result;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            // If the token is a number, return its value as an integer
            return reader.GetInt32();
        }

        // If it's neither a string nor a number, throw an exception
        throw new JsonException("Invalid value type for integer.");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        // For writing, just write the integer as a number
        writer.WriteNumberValue(value);
    }
}
public class FloatOrStringConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            // If the token is a string, try to parse it as an integer
            if (int.TryParse(reader.GetString(), out var result))
            {
                return result;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            // If the token is a number, return its value as an integer
            return (float)reader.GetDouble();
        }

        // If it's neither a string nor a number, throw an exception
        throw new JsonException("Invalid value type for integer.");
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        // For writing, just write the integer as a number
        writer.WriteNumberValue(value);
    }
}