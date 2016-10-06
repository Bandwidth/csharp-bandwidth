﻿# IBridge.CreateAsync Method 
 

Create a bridge.

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-beta3

## Syntax

**C#**<br />
``` C#
Task<ILazyInstance<Bridge>> CreateAsync(
	CreateBridgeData data,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>data</dt><dd>Type: <a href ="T_Bandwidth_Net_Api_CreateBridgeData.md">Bandwidth.Net.Api.CreateBridgeData</a><br />Parameters of new bridge</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />Optional token to cancel async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd321424" target="_blank">Task</a>(<a href ="T_Bandwidth_Net_ILazyInstance_1.md">ILazyInstance</a>(<a href ="T_Bandwidth_Net_Api_Bridge.md">Bridge</a>))<br />Instance of created bridge

## Examples

```
var bridge = await client.Bridge.CreateAsync(new CreateBridgeData{ CallIds = new[]{"callId"}});
```


## See Also


#### Reference
<a href ="T_Bandwidth_Net_Api_IBridge.md">IBridge Interface</a><br /><a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />