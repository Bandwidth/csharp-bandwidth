# IApplication.List Method 
 

Get a list of applications

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
IEnumerable<Application> List(
	ApplicationQuery query = null,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>query (Optional)</dt><dd>Type: <a href ="T_Bandwidth_Net_Api_ApplicationQuery.md">Bandwidth.Net.Api.ApplicationQuery</a><br />Optional query parameters</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />>Optional token to cancel async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable</a>(<a href ="T_Bandwidth_Net_Api_Application.md">Application</a>)<br />Collection with <a href ="T_Bandwidth_Net_Api_Application.md">Application</a> instances

## Examples

```
var applications = client.Application.List(); // get access to all applications of user
```


## See Also


#### Reference
<a href ="T_Bandwidth_Net_Api_IApplication.md">IApplication Interface</a><br /><a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />