using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

namespace AppSettingToEnvVar.services;

public class ConvertJsonToEnvVarsService : IConvertJsonToEnvVarsService
{
    private const string Separator = "__";
    private const string KeyValueSeparator = ": ";

    public async Task<string> Convert(string json)
    {
        var expando = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);

        if (expando == null) return string.Empty;
        var result = string.Empty;

        foreach (var (key, value) in expando)
        {
            result += await GetStringValue(value ?? string.Empty, key);
        }

        return result;
    }

    private static async Task<string> GetStringValue(object obj, string key)
    {
        var result = string.Empty;

        switch (obj)
        {
            case ExpandoObject expandoObject:
            {
                foreach (var childObject in expandoObject)
                {
                    var currentKey = key + Separator + childObject.Key;
                    result += await GetStringValue(childObject.Value ?? string.Empty, currentKey);
                }

                break;
            }
            case List<object> expandoObjects:
                for (var i = 0; i < expandoObjects.Count; i++)
                {
                    var currentKey = key + Separator + i;
                    result += await GetStringValue(expandoObjects[i], currentKey);
                }

                break;
            case string:
                result += key + KeyValueSeparator + obj + Environment.NewLine;
                break;
        }

        return result;
    }
}

public interface IConvertJsonToEnvVarsService
{
    Task<string> Convert(string json);
}