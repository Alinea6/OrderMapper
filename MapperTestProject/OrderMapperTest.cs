using FluentAssertions.Json;
using Mapper;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace MapperTestProject;

public class OrderMapperTest
{
    private OrderMapper _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new OrderMapper();
    }

    [Test]
    public void OrderMapper_should_add_const_values_to_output()
    {
        var mapping = JArray.Parse("""
                                              [
                                              {
                                                  "map": [
                                                      {
                                                          "target": "smerpConnection.context",
                                                          "input": {
                                                              "type": "const",
                                                              "value": "oms"
                                                          }
                                                      }
                                                  ]
                                              }

                                              ]
                                              """);

        var inputData = JObject.Parse("""
                                      {
                                      }
                                      """);
        var outputData = JObject.Parse("""
                                        {
                                           "smerpConnection" : {
                                               "context" : "oms"
                                           }
                                       }
                                       """);

        var result = _sut.Map(mapping, inputData);

        result.Should().BeEquivalentTo(outputData);
    }

    [Test]
    public void OrderMapper_should_map_values_from_input_to_output()
    {
        var inputMapping = JArray.Parse("""
                                        [
                                        {
                                            "map": [
                                                {
                                                    "target": "platformOrderId",
                                                    "input": "source.platformOrderId"
                                                }
                                            ]
                                        }
                                        ]
                                        """);
        var inputData = JObject.Parse("""
                                      {
                                          "firstName": "test",
                                          "source" : {
                                              "platformOrderId" : "abc"
                                          }
                                      }
                                      """);
        var outputData = JObject.Parse("""
                                       {
                                           "platformOrderId" : "abc"
                                       }
                                       """);

        var result = _sut.Map(inputMapping, inputData);
        
        result.Should().BeEquivalentTo(outputData);
    }
}
                                                                              
                                                                  
                                                                  
                                                             