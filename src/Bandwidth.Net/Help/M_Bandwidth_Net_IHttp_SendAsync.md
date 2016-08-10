# IHttp.SendAsync Method 
 

Send http request and return response message

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net.md">Bandwidth.Net</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
Task<HttpResponseMessage> SendAsync(
	HttpRequestMessage request,
	HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>request</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/hh159020" target="_blank">System.Net.Http.HttpRequestMessage</a><br />Request message to send</dd><dt>completionOption (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/hh158990" target="_blank">System.Net.Http.HttpCompletionOption</a><br />Indicates if current http operation should be considered completed either as soon as a response is available, or after reading the entire response message including the content.</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />Cancelation token for current async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd321424" target="_blank">Task</a>(<a href="http://msdn2.microsoft.com/en-us/library/hh159046" target="_blank">HttpResponseMessage</a>)<br />Task with response message

## See Also


#### Reference
<a href ="T_Bandwidth_Net_IHttp.md">IHttp Interface</a><br /><a href ="N_Bandwidth_Net.md">Bandwidth.Net Namespace</a><br />