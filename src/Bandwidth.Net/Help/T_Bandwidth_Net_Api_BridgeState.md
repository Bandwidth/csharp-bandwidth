# BridgeState Enumeration
 

Possible bridge state

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
public enum BridgeState
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:Bandwidth.Net.Api.BridgeState.Created">**Created**</td><td>0</td><td>The bridge was created but the audio was never bridged.</td></tr><tr><td /><td target="F:Bandwidth.Net.Api.BridgeState.Active">**Active**</td><td>1</td><td>The bridge has two active calls and the audio was already bridged before.</td></tr><tr><td /><td target="F:Bandwidth.Net.Api.BridgeState.Hold">**Hold**</td><td>2</td><td>The bridge calls are on hold (bridgeAudio was set to false).</td></tr><tr><td /><td target="F:Bandwidth.Net.Api.BridgeState.Completed">**Completed**</td><td>3</td><td>The bridge was completed. The bridge is completed when all calls hangup or when all calls are removed from bridge.</td></tr><tr><td /><td target="F:Bandwidth.Net.Api.BridgeState.Error">**Error**</td><td>4</td><td>Some error was detected in bridge.</td></tr></table>

## See Also


#### Reference
<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />