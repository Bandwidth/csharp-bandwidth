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
      return new Client(new CatapultAuthData {UserId = "userId", ApiToken = "apiToken", ApiSecret = "apiSecret", BaseUrl = "http://localhost/v1"},
        new IrisAuthData { AccountId = "accountId", UserName = "userName", Password = "password", BaseUrl = "http://localhost/v1.0" },
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
  }

  public class JsonContent : StringContent
  {
    public JsonContent(string content) : base(content, Encoding.UTF8, "application/json")
    {
    }
  }
}
