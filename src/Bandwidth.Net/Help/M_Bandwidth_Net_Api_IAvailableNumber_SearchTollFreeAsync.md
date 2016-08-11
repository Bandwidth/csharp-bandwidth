# IAvailableNumber.SearchTollFreeAsync Method 
 

Search for available toll free numbers

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
Task<AvailableNumber[]> SearchTollFreeAsync(
	TollFreeNumberQuery query,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>query</dt><dd>Type: <a href ="T_Bandwidth_Net_Api_TollFreeNumberQuery.md">Bandwidth.Net.Api.TollFreeNumberQuery</a><br />Search criterias</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />Optional token to cancel async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd321424" target="_blank">Task</a>(<a href ="T_Bandwidth_Net_Api_AvailableNumber.md">AvailableNumber</a>[])<br />Array with <a href ="T_Bandwidth_Net_Api_AvailableNumber.md">AvailableNumber</a> instances

## Examples

```
var numbers = await client.AvailableNumber.SearchTollFreeAsync(new TollFreeNumberQuery {Quantity = 5});
```


## See Also


#### Reference
<a href ="T_Bandwidth_Net_Api_IAvailableNumber.md">IAvailableNumber Interface</a><br /><a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />