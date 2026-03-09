using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SqlReportTools
{
    public class FileInfoJsonConverter : JsonConverter<FileInfo>
    {
        public override FileInfo Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => new FileInfo(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            FileInfo file,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(file.FullName);
    }
}
