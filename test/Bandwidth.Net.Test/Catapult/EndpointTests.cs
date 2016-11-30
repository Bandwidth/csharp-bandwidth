﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Catapult;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Catapult
{
  public class EndpointTests
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("Endpoint")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetCatapultApi(context).Endpoint;
      var endpoints = api.List("domainId");
      ValidateEndpoint(endpoints.First());
    }

    [Fact]
    public async void TestCreate()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/id");
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetCatapultApi(context).Endpoint;
      var endpointId = await api.CreateAsync(new CreateEndpointData {DomainId = "domainId", Name = "name"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", endpointId);
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Endpoint")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetCatapultApi(context).Endpoint;
      var endpoint = await api.GetAsync("domainId", "id");
      ValidateEndpoint(endpoint);
    }

    [Fact]
    public async void TestUpdate()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetCatapultApi(context).Endpoint;
      await api.UpdateAsync("domainId","id", new UpdateEndpointData {Enabled = false});
    }

    [Fact]
    public async void TestDelete()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidDeleteRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetCatapultApi(context).Endpoint;
      await api.DeleteAsync("domainId", "id");
    }

    [Fact]
    public async void TestCreateAuthToken()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("EndpointAuthToken")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateAuthTokenRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetCatapultApi(context).Endpoint;
      var token = await api.CreateAuthTokenAsync("domainId", "id");
      Assert.Equal("token", token.Token);
    }

    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/domains/domainId/endpoints";
    }

    public static bool IsValidCreateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/domains/domainId/endpoints" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"name\":\"name\",\"domainId\":\"domainId\"}";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/domains/domainId/endpoints/id";
    }

    public static bool IsValidUpdateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/domains/domainId/endpoints/id" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"enabled\":false}";
    }

    public static bool IsValidDeleteRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Delete && request.RequestUri.PathAndQuery == "/v1/users/userId/domains/domainId/endpoints/id";
    }

    public static bool IsValidCreateAuthTokenRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/domains/domainId/endpoints/id/tokens";
    }


    private static void ValidateEndpoint(Endpoint item)
    {
      Assert.Equal("domainId", item.DomainId);
      Assert.Equal("applicationId", item.ApplicationId);
      Assert.True(item.Enabled);
      Assert.Equal("jsmith-mobile", item.Credentials.UserName);
    }
  }
}
