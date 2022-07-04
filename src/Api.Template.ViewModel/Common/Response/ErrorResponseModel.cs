using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Api.Template.ViewModel.Common.Response;

public class ErrorResponseModel
{
    [JsonProperty("succeed")]
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; } = false;

    [JsonProperty("errors")]
    [JsonPropertyName("errors")]
    public dynamic Errors { get; set; }
    
    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public dynamic Data { get; set; }

    public ErrorResponseModel()
    {
    }

    public ErrorResponseModel(object data)
    {
        Data = data;
    }
    
    public ErrorResponseModel(dynamic data, dynamic errors)
    {
        Data = data;
        Errors = errors;
    }
        
    public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
}

public class ErrorResponseModel<T>
{
    [JsonProperty("succeed")]
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; } = false;
    
    [JsonProperty("errors")]
    [JsonPropertyName("errors")]
    public dynamic Errors { get; set; }
    
    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public T Data { get; set; }

    public ErrorResponseModel()
    {
    }

    public ErrorResponseModel(T data)
    {
        Data = data;
    }
    
    public ErrorResponseModel(T data, dynamic errors)
    {
        Data = data;
        Errors = errors;
    }
        
    public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
}