using System;
using System.Text;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class AuthDataTests
  {
  
    [Fact]
    public void TestCatapultAuthDataAuthenticationHeader()
    {
      var data = new CatapultAuthData{ApiToken = "token", ApiSecret = "secret"};
      Assert.Equal("Basic", data.AuthenticationHeader.Scheme);
      Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("token:secret")), data.AuthenticationHeader.Parameter);
    }

    [Fact]
    public void TestCatapultAuthDataValidate()
    {
      var data = new CatapultAuthData{UserId = "userId", ApiToken = "token", ApiSecret = "secret"};
      data.Validate();
    }

    [Fact]
    public void TestCatapultAuthDataValidateWithoutUserId()
    {
      var data = new CatapultAuthData{ApiToken = "token", ApiSecret = "secret"};
      Assert.Throws<MissingCredentialsException>(() => data.Validate());
    }

    [Fact]
    public void TestCatapultAuthDataValidateWithoutApiToken()
    {
      var data = new CatapultAuthData{UserId = "userId", ApiSecret = "secret"};
      Assert.Throws<MissingCredentialsException>(() => data.Validate());
    }

    [Fact]
    public void TestCatapultAuthDataValidateWithoutApiSecret()
    {
      var data = new CatapultAuthData{UserId = "userId", ApiToken = "token"};
      Assert.Throws<MissingCredentialsException>(() => data.Validate());
    }

    [Fact]
    public void TestCatapultAuthDataValidateWithoutBaseUrl()
    {
      var data = new CatapultAuthData{UserId = "userId", ApiToken = "token", ApiSecret = "secret", BaseUrl = null};
      Assert.Throws<InvalidBaseUrlException>(() => data.Validate());
    }

    [Fact]
    public void TestIrisAuthDataAuthenticationHeader()
    {
      var data = new IrisAuthData{UserName = "userName", Password = "password"};
      Assert.Equal("Basic", data.AuthenticationHeader.Scheme);
      Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("userName:password")), data.AuthenticationHeader.Parameter);
    }

    [Fact]
    public void TestIrisAuthDataValidate()
    {
      var data = new IrisAuthData{AccountId = "accountId", UserName = "userName", Password = "password"};
      data.Validate();
    }

    [Fact]
    public void TestIrisAuthDataValidateWithoutAccountId()
    {
      var data = new IrisAuthData{UserName = "userName", Password = "password"};
      Assert.Throws<MissingCredentialsException>(() => data.Validate());
    }

    [Fact]
    public void TestIrisAuthDataValidateWithoutUserName()
    {
      var data = new IrisAuthData{AccountId = "accountId", Password = "password"};
      Assert.Throws<MissingCredentialsException>(() => data.Validate());
    }

    [Fact]
    public void TestIrisAuthDataValidateWithoutPassword()
    {
      var data = new IrisAuthData{AccountId = "accountId", UserName = "userName"};
      Assert.Throws<MissingCredentialsException>(() => data.Validate());
    }

    [Fact]
    public void TestIrisAuthDataValidateWithoutBaseUrl()
    {
      var data = new IrisAuthData{AccountId = "accountId", UserName = "userName", Password = "password", BaseUrl = null};
      Assert.Throws<InvalidBaseUrlException>(() => data.Validate());
    }
  }
}
