# PlayAudioExtensions.PlayAudioFileAsync Method 
 

Play audio file by url

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net.md">Bandwidth.Net</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
public static Task PlayAudioFileAsync(
	this IPlayAudio instance,
	string id,
	string fileUrl,
	string tag = null,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>instance</dt><dd>Type: <a href ="T_Bandwidth_Net_IPlayAudio.md">Bandwidth.Net.IPlayAudio</a><br />>Instance of <a href ="T_Bandwidth_Net_IPlayAudio.md">IPlayAudio</a></dd><dt>id</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />ID of bridge, call, conference, etc</dd><dt>fileUrl</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Url to file to play</dd><dt>tag (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />A string that will be included in the events delivered when the audio playback starts or finishes</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />Optional token to cancel async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd235678" target="_blank">Task</a><br />Task instance for async operation

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type <a href ="T_Bandwidth_Net_IPlayAudio.md">IPlayAudio</a>. When you use instance method syntax to call this method, omit the first parameter. For more information, see <a href="http://msdn.microsoft.com/en-us/library/bb384936.aspx">Extension Methods (Visual Basic)</a> or <a href="http://msdn.microsoft.com/en-us/library/bb383977.aspx">Extension Methods (C# Programming Guide)</a>.

## Examples

```
await client.Bridge.PlayAudioFileAsync("bridgeId", "http://host/path/to/file.mp3");
```


## See Also


#### Reference
<a href ="T_Bandwidth_Net_PlayAudioExtensions.md">PlayAudioExtensions Class</a><br /><a href ="N_Bandwidth_Net.md">Bandwidth.Net Namespace</a><br />