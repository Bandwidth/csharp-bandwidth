# CallbackEvent Class
 

Catapult Api callback event


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;Bandwidth.Net.ApiV2.CallbackEvent<br />
**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_ApiV2.md">Bandwidth.Net.ApiV2</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.3

## Syntax

**C#**<br />
``` C#
public class CallbackEvent
```

The CallbackEvent type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href ="M_Bandwidth_Net_ApiV2_CallbackEvent__ctor.md">CallbackEvent</a></td><td>
Initializes a new instance of the CallbackEvent class</td></tr></table>&nbsp;
<a href="#callbackevent-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_ApiV2_CallbackEvent_Description.md">Description</a></td><td>
Event description</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_ApiV2_CallbackEvent_Message.md">Message</a></td><td>
Message data</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_ApiV2_CallbackEvent_ReplyTo.md">ReplyTo</a></td><td>
Phone numbers for answer</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_ApiV2_CallbackEvent_Time.md">Time</a></td><td>
Event time</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_ApiV2_CallbackEvent_To.md">To</a></td><td>
Event target phone number</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_ApiV2_CallbackEvent_Type.md">Type</a></td><td>
Event type</td></tr></table>&nbsp;
<a href="#callbackevent-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href ="M_Bandwidth_Net_ApiV2_CallbackEvent_CreateFromJson.md">CreateFromJson</a></td><td>
Parse callback event data from JSON</td></tr></table>&nbsp;
<a href="#callbackevent-class">Back to Top</a>

## Examples

```
var callbackEvent = CallbackEvent.CreateFromJson("{\"type\": \"message-received\"}");
switch(callbackEvent.Type)
{
  case CallbackEventType.MessageReceived:
    Console.WriteLine($"Sms {callbackEvent.Message.From} -> {string.Join(", ", callbackEvent.Message.To)}: {callbackEvent.Message.Text}");
    break;
}
```


## See Also


#### Reference
<a href ="N_Bandwidth_Net_ApiV2.md">Bandwidth.Net.ApiV2 Namespace</a><br />