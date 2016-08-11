# Client Constructor 
 

Constructor

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net.md">Bandwidth.Net</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
public Client(
	string userId,
	string apiToken,
	string apiSecret,
	string baseUrl = "https://api.catapult.inetwork.com",
	IHttp http = null
)
```


#### Parameters
&nbsp;<dl><dt>userId</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Id of user on Catapult API</dd><dt>apiToken</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Authorization token of Catapult API</dd><dt>apiSecret</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Authorization secret of Catapult API</dd><dt>baseUrl (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Base url of Catapult API server</dd><dt>http (Optional)</dt><dd>Type: <a href ="T_Bandwidth_Net_IHttp.md">Bandwidth.Net.IHttp</a><br />Optional processor of http requests. Use it to owerwrite default http request processing (useful for test, logs, etc)</dd></dl>

## Examples
Regular usage 
```
var client = new Client("userId", "apiToken", "apiSecret");
```
 Using another server 
```
var client = new Client("userId", "apiToken", "apiSecret", "https://another.server");
```
 Using with own implementaion of HTTP processing (usefull in tests) 
```
var client = new Client("userId", "apiToken", "apiSecret", "https://another.server", new YourMockHttp());
```


## See Also


#### Reference
<a href ="T_Bandwidth_Net_Client.md">Client Class</a><br /><a href ="N_Bandwidth_Net.md">Bandwidth.Net Namespace</a><br />