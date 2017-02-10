using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class XmlHelpersTests
  {

    [Fact]
    public async void TestSetXmlContent()
    {
      using (var request = new HttpRequestMessage())
      {
        request.SetXmlContent(new TestItem { Field1 = "test"});
        Assert.Equal("application/xml", request.Content.Headers.ContentType.MediaType);
        var xml = await request.Content.ReadAsStringAsync();
        Assert.Contains("<Field1>test</Field1>", xml);
      }
    }

    [Fact]
    public async void TestReadAsXmlAsync()
    {
      var content = new StringContent("<TestItem><Field1>100</Field1></TestItem>", Encoding.UTF8, "application/xml");
      var item = await content.ReadAsXmlAsync<TestItem>();
      Assert.NotNull(item);
      Assert.Equal("100", item.Field1);
    }

    [Fact]
    public async void TestReadAsXmlAsyncForNonXmlContent()
    {
      var content = new StringContent("text", Encoding.UTF8, "plain/text");
      var item = await content.ReadAsXmlAsync<TestItem>();
      Assert.Null(item);
    }

    [Fact]
    public async void TestCheckResponseForSuccess()
    {
      using (var response = new HttpResponseMessage(HttpStatusCode.OK))
      {
        response.Content = new StringContent("<Field1>100</Field1>", Encoding.UTF8, "application/xml");
        await response.CheckXmlResponseAsync();
      }
    }

    [Fact]
    public async void TestCheckResponseForErrorWithPayload1()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("<Result><ErrorCode>Code</ErrorCode><Description>Description</Description></Result>", Encoding.UTF8,
            "application/xml");
          return response.CheckXmlResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
      Assert.Equal("Description", ex.Message);
      Assert.Equal("Code", ex.AdditionalData["Code"]);
    }

    [Fact]
    public async void TestCheckResponseForErrorWithPayload2()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("<ErrorData><resultCode>Code</resultCode><resultMessage>Description</resultMessage></ErrorData>", Encoding.UTF8,
            "application/xml");
          return response.CheckXmlResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
      Assert.Equal("Description", ex.Message);
      Assert.Equal("Code", ex.AdditionalData["Code"]);
    }

    [Fact]
    public async void TestCheckResponseForErrorWithPayload3()
    {
      var ex = await Assert.ThrowsAsync<AggregateException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("<Errors><Code>Code</Code><Description>Description</Description></Errors>", Encoding.UTF8,
            "application/xml");
          return response.CheckXmlResponseAsync();
        }
      });
      Assert.Equal(1, ex.InnerExceptions.Count);
      var e = ex.InnerExceptions.First() as BandwidthException;
      Assert.NotNull(e);
      Assert.Equal(HttpStatusCode.BadRequest, e.Code);
      Assert.Equal("Description", e.Message);
      Assert.Equal("Code", e.AdditionalData["Code"]);
    }

    [Fact]
    public async void TestCheckResponseForErrorWithPayload4()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("<Error><Code>Code</Code><Description>Description</Description></Error>", Encoding.UTF8,
            "application/xml");
          return response.CheckXmlResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
      Assert.Equal("Description", ex.Message);
      Assert.Equal("Code", ex.AdditionalData["Code"]);
    }


    [Fact]
    public async void TestCheckResponseWithInvalidXml()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("<Code 100", Encoding.UTF8, "application/xml");
          return response.CheckJsonResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
    }

    public class TestItem
    {
      public string Field1 { get; set; }
    }
  }
}
