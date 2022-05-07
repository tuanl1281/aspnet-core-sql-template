namespace Api.Template.ViewModel.Common;

using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class ResultModel
{
    [JsonProperty("succeed")]
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; }

    [JsonProperty("errorMessages")]
    [JsonPropertyName("errorMessages")]
    public string ErrorMessages { get; set; }

    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public dynamic Data { get; set; }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}

public class ResultModel<T>
{
    [JsonProperty("succeed")]
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; }

    [JsonProperty("errorMessages")]
    [JsonPropertyName("errorMessages")]
    public string ErrorMessages { get; set; }

    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public T Data { get; set; }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}

public class StatusModel 
{
    [JsonProperty("total")]
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonProperty("success")]
    [JsonPropertyName("success")]
    public int Success { get; set; }

    [JsonProperty("failed")]
    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
