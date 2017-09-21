﻿using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using Bandwidth.Net.Test.Mocks;
using LightMock;

namespace Bandwidth.Net.Test
{
  public static class Helpers
  {
    public static Client GetClient(MockContext<IHttp> context = null)
    {
      return new Client("userId", "apiToken", "apiSecret", "http://localhost",
        context == null ? null : new Http(context));
    }
    private static readonly ResourceManager ResourceManager = new ResourceManager("Bandwidth.Net.Test.Json", typeof(Helpers).GetTypeInfo().Assembly);
    public static string GetJsonResourse(string name)
    {
      return ResourceManager.GetString(name);
    }

    public static JsonContent GetJsonContent(string name)
    {
      return new JsonContent(GetJsonResourse(name));
    }

    public static StringContent GetXmlContent(string name)
    {
      return new StringContent(ResourceManager.GetString(name), Encoding.UTF8, "application/xml");
    }
  }

  public class JsonContent : StringContent
  {
    public JsonContent(string content) : base(content, Encoding.UTF8, "application/json")
    {
    }
  }
}
