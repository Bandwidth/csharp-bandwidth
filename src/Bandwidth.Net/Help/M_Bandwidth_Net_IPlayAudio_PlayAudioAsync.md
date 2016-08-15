# IPlayAudio.PlayAudioAsync Method 
 

Play audio

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net.md">Bandwidth.Net</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
Task PlayAudioAsync(
	string id,
	PlayAudioData data,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>id</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />ID of bridge, call, conference, etc</dd><dt>data</dt><dd>Type: <a href ="T_Bandwidth_Net_PlayAudioData.md">Bandwidth.Net.PlayAudioData</a><br />Parameters for play audio</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />Optional token to cancel async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd235678" target="_blank">Task</a><br />>Task instance for async operation

## Examples

```
await client.Bridge.PlayAudioAsync("bridgeId", new PlayAudioData {FileUrl = "url"});
```


## See Also


#### Reference
<a href ="T_Bandwidth_Net_IPlayAudio.md">IPlayAudio Interface</a><br /><a href ="N_Bandwidth_Net.md">Bandwidth.Net Namespace</a><br />