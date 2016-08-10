using Newtonsoft.Json;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class JsonStringEnumConverterTests
  {
    public enum TestEnum
    {
      SomeValue,
      SomeAnotherValue
    }

    [Fact]
    public void TestReadJson()
    {
      var item = JsonConvert.DeserializeObject<TestItem>("{\"Item\":\"some-value\"}");
      Assert.Equal(TestEnum.SomeValue, item.Item);
    }

    [Fact]
    public void TestReadJson2()
    {
      var item = JsonConvert.DeserializeObject<TestItem>("{\"Item\":\"some-another-value\"}");
      Assert.Equal(TestEnum.SomeAnotherValue, item.Item);
    }

    [Fact]
    public void TestWriteJson()
    {
      var item = new TestItem {Item = TestEnum.SomeValue};
      Assert.Equal("{\"Item\":\"some-value\"}", JsonConvert.SerializeObject(item));
    }

    [Fact]
    public void TestWriteJson2()
    {
      var item = new TestItem {Item = TestEnum.SomeAnotherValue};
      Assert.Equal("{\"Item\":\"some-another-value\"}", JsonConvert.SerializeObject(item));
    }

    public class TestItem
    {
      [JsonConverter(typeof (JsonStringEnumConverter))]
      public TestEnum Item { get; set; }
    }
  }
}
