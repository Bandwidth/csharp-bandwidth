# Bandwidth.Net

> Deprecation Notice
> This project is deprecated. Please go to https://github.com/Bandwidth/csharp-sdk

A .Net client library for the [Bandwidth Application Platform](http://bandwidth.com/products/application-platform?utm_medium=social&utm_source=github&utm_campaign=dtolb&utm_content=_)

The current version is v3.0, released 1 February, 2017. Version 2.15 is available  [here](https://github.com/bandwidthcom/csharp-bandwidth/tree/v2.15).

Notice: As of April 2019, versions of csharp-bandwidth less than 4.0.0 will not be compatible with Bandwidth's V2 Messaging. If you are using Bandwidth's V2 Messaging, you will need to update your csharp-bandwidth package version to 4.0.0 or above. If you are not using Bandwidth's V2 Messaging, you do not need to update. 

[![Build on .Net 4.5 (Windows)](https://ci.appveyor.com/api/projects/status/bhv8hs3fx9k6c33i?svg=true)](https://ci.appveyor.com/project/Bandwidth/csharp-bandwidth)
[![Build on .Net Core (Linux)](https://travis-ci.org/Bandwidth/csharp-bandwidth.svg)](https://travis-ci.org/Bandwidth/csharp-bandwidth)
[![Coverage Status](https://coveralls.io/repos/github/Bandwidth/csharp-bandwidth/badge.svg)](https://coveralls.io/github/Bandwidth/csharp-bandwidth)

[Full API Reference](src/Bandwidth.Net/Help/Home.md)

## Release Notes
| Version | Notes |
|:---|:---|
| 4.x.x | In order to use versions 4.x.x for Bandwidth's V2 messaging, the following line of code must be included before sending a message `System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;` |

## Installing the SDK

`Bandwidth.Net` is available on Nuget (Nuget 3.0+ is required):

    Install-Package Bandwidth.Net -Pre

## Supported Versions
`Bandwidth.Net` should work on all levels of .Net Framework 4.5+.

| Version | Support Level |
|---------|---------------|
| <4.5 | Unsupported |
| 4.5 | Supported |
| netstandard1.6 (.net 4.6+, .net core 1.0+, etc)  | Supported |
| netstandard2.0 (.net core 2.0+, etc)  | Supported |

## Client initialization

All interaction with the API is done through a class `Client`. The `Client` constructor takes an next options:

| Argument  | Description           | Default value                       | Required |
|-------------|-----------------------|-------------------------------------|----------|
| `userId`    | Your user ID | none                         | Yes      |
| `apiToken`  | Your API token        | none                         | Yes      |
| `apiSecret` | Your API secret       | none                         | Yes      |
| `baseUrl`   | The Bandwidth API URL  | `https://api.catapult.inetwork.com` | No       |

To initialize the `Client` instance, provide your API credentials which can be found on your account page in [the portal](https://catapult.inetwork.com/pages/catapult.jsf).

```csharp
using Bandwidth.Net;

var client = new Client(
    "YOUR_USER_ID", // <-- note, this is not the same as the username you used to login to the portal
    "YOUR_API_TOKEN",
    "YOUR_API_SECRET"
);
```

Your `client` object is now ready to use the API.

### Lazy evalutions

This library uses lazy evalutions in next cases:
    
- Object creation,
    
- Get list of objects

#### Object Creation

When you create a bridge, call, message, etc. you will receive instance of `ILazyInstance<>` as result. It allow you to get `Id` of created object and created object on demand via property `Instance`.

```csharp
var applicationId = await client.Application.CreateAsync(new CreateApplicationData {Name = "MyFirstApp"});

```

#### Get list of objects

Executing of methods which returns collections of objects will not execute Catapult API request immediately. This request will be executed only when you try enumerate items of the collection.

```csharp
var calls = client.Call.List(); // will not execute any requests to Catapult API here

foreach(var call in calls) // a request to Catapult API will be executed here
{
    Console.WriteLine(call.From);
}

// or

var list = calls.ToList(); // a request to Catapult API will be executed here

```

**Note:** All methods `List()` used async request execution and locking current thread for waiting for results. Calling `List()` in main UI thread of application (WinForm, WPF, ASP.Net) can be cause of "freezes".
Use `List()` only inside non-main threads. For example in another async task like in next demo
```csharp
await Task.Run(() => {
    var calls = client.Call.List();
    // rest code here
})
```



### Examples

Send a SMS

```csharp
var messageId = await client.Message.SendAsync(new MessageData {
    From = "+12345678901", // This must be a Bandwidth number on your account
    To   = "+12345678902",
    Text = "Hello world."
});
Console.WriteLine($"Message Id is {messageId}");
```

#### Messaging 2.0

```csharp
// Using Message API v2

// Create a messaging application
var dashboardAuthData = new IrisAuthData
{
    AccountId = "AccountId",
    UserName = "UserName",
    Password = "Password",
    SubaccountId = "SubaccountId"
};
var messagingApplication = await api.CreateMessagingApplicationAsync(dashboardAuthData, new CreateMessagingApplicationData
{
    Name = "My Messaging App",
    CallbackUrl = "http://my-callback",
    LocationName = "My Location",
    SmsOptions = new SmsOptions
    {
        TollFreeEnabled = true
    },
    MmsOptions = new MmsOptions
    {
        Enabled = true
    }
});

// Reserve a phone number for messaging
var numbers = await api.SearchAndOrderNumbersAsync(dashboardAuthData, messagingApplication, new AreaCodeSearchAndOrderNumbersQuery
{
    AreaCode = "910",
    Quantity = 1
});

// Now you can send meessages via API v2
var message = await client.V2.Message.SendAsync(new MessageData{ 
    From = numbers[0],  //use only numbers reserved by SearchAndOrderNumbersAsync()
    To = new[] {"+12345678902"},
    Text = "Hello world",
    ApplicationId = messagingApplication.ApplicationId
});

```

Make a call

```csharp
var callId = await client.Call.CreateAsync(new CreateCallData {
    From = "+12345678901", // This must be a Bandwidth number on your account
    To   = "+12345678902"
});
Console.WriteLine($"Call Id is {call.Id}");
```

Extracting callback event data from string

```csharp
var callbackEvent = CallbackEvent.CreateFromJson("{\"eventType\": \"sms\"}");
```

Extracting callback event data from http content (useful for ASP.Net and other web applications)

```csharp
// In ASP.Net action
var callbackEvent = await Request.Content.ReadAsCallbackEventAsync();

// anywhere
var callbackEvent = await content.ReadAsCallbackEventAsync(); // content is instance of HttpContent

```

Using BXMLv2

```csharp
using Bandwidth.Net.XmlV2.Verbs;
using Bandwidth.Net.Xml;

var response = new Response(new SpeakSentence{
	Gender = "female",
	Locale = "en_UK",
	Sentence = "Hello",
	Voice = "kate"
});

var xml = response.ToXml(); // will build string <?xml version="1.0" encoding="utf-8"?><Response><SpeakSentence gender="female" locale="en_UK" voice="kate">Hello</SpeakSentence></Response>
```

## Providing feedback

Please open an [issue](https://github.com/Bandwidth/csharp-bandwidth/issues) on this repository and we'll take a look. Thanks!

