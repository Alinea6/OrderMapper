using Newtonsoft.Json.Linq;

namespace Mapper;

public class OrderMapper
{
    public JObject Map(JArray inputMapping, JObject inputData)
    {
        JArray processedMappings = new JArray();
        var output = new JObject();

        var input = ConvertInputData(inputData);
        
        
        foreach (var mapping in inputMapping)
        {
            processedMappings = new JArray(processedMappings.Union(mapping["map"] as JArray));
        }
        
        foreach (var mapping in processedMappings)
        {
            // checks for constant values in input mapping and adds it to the output
            if (mapping.SelectToken("input.type")!=null)
            {
                output = AddToJObject(output, mapping.SelectToken("target")?.ToString(),
                    mapping.SelectToken("input.value")?.ToString());
            }
            
            // maps values from input data to output based on the input mapping properties
            if (!mapping.SelectToken("input").Children<JObject>().Properties().Any() 
                && input.Keys.Any(x => x == mapping.SelectToken("input")?.ToString()))
            {
                output = AddToJObject(output, mapping.SelectToken("target")?.ToString(),
                    input[mapping.SelectToken("input")?.ToString()]);
            }
        }
        return output;
    }

    /// <summary>
    /// Adds new entries to output json object based on property format
    /// </summary>
    /// <returns>input jObject with appended field</returns>
    private JObject AddToJObject(JObject jObject, string property, string value)
    {
        if (property.Contains('.'))
        {
            var properties = property.Split(".");
            jObject.Add(properties[0], new JObject{ [properties[1]] = value });
            return jObject;
        }
        jObject.Add(property, value);
        return jObject;
    }

    /// <summary>
    /// Converted input data to dictionary with keys in the form of `a` or `a.b` and their values being target strings.
    /// </summary>
    private Dictionary<string, string> ConvertInputData(JObject inputData)
    {
        var inputProperties = inputData.Properties();
        var inputDictionary = new Dictionary<string, string>();
        foreach (var property in inputProperties)
        {
            var children = property.Children<JObject>().Properties();
            if (children.Any())
            {
                foreach (var childrenProperty in children)
                {
                    inputDictionary.Add(string.Join(".", [property.Name, childrenProperty.Name]), childrenProperty.Value.ToString());
                }
            }
            else
            {
                inputDictionary.Add(property.Name, property.Value.ToString());
            }
        }

        return inputDictionary;
    }
}