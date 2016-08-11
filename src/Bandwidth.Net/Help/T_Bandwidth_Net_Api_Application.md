# Application Class
 

Application information


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;Bandwidth.Net.Api.Application<br />
**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
public class Application
```

The Application type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href ="M_Bandwidth_Net_Api_Application__ctor.md">Application</a></td><td>
Initializes a new instance of the Application class</td></tr></table>&nbsp;
<a href="#application-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_AutoAnswer.md">AutoAnswer</a></td><td>
Determines whether or not an incoming call should be automatically answered.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_CallbackHttpMethod.md">CallbackHttpMethod</a></td><td>
Determine if the callback event should be sent via HTTP GET or HTTP POST</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_Id.md">Id</a></td><td>
The unique identifier for the application.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_IncomingCallFallbackUrl.md">IncomingCallFallbackUrl</a></td><td>
The URL used to send the callback event if the request to incomingCallUrl fails.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_IncomingCallUrl.md">IncomingCallUrl</a></td><td>
A URL where call events will be sent for an inbound call. This is the endpoint where the Application Platform will send all call events. Either incomingCallUrl or incomingMessageUrl is required.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_IncomingCallUrlCallbackTimeout.md">IncomingCallUrlCallbackTimeout</a></td><td>
Determine how long should the platform wait for incomingCallUrl's response before timing out in milliseconds.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_IncomingMessageFallbackUrl.md">IncomingMessageFallbackUrl</a></td><td>
The URL used to send the callback event if the request to incomingMessageUrl fails.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_IncomingMessageUrl.md">IncomingMessageUrl</a></td><td>
A URL where message events will be sent for an inbound message.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_IncomingMessageUrlCallbackTimeout.md">IncomingMessageUrlCallbackTimeout</a></td><td>
Determine how long should the platform wait for incomingMessageUrl's response before timing out in milliseconds.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_Application_Name.md">Name</a></td><td>
A name you choose for this application.</td></tr></table>&nbsp;
<a href="#application-class">Back to Top</a>

## See Also


#### Reference
<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />