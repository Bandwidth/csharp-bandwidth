# IAccount.GetTransactions Method 
 

GetAsync a list of the transactions made to account

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-preview

## Syntax

**C#**<br />
``` C#
IEnumerable<AccountTransaction> GetTransactions(
	AccountTransactionQuery query = null,
	Nullable<CancellationToken> cancellationToken = null
)
```


#### Parameters
&nbsp;<dl><dt>query (Optional)</dt><dd>Type: <a href ="T_Bandwidth_Net_Api_AccountTransactionQuery.md">Bandwidth.Net.Api.AccountTransactionQuery</a><br />Optional query parameters</dd><dt>cancellationToken (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/dd384802" target="_blank">CancellationToken</a>)<br />>Optional token to cancel async operation</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable</a>(<a href ="T_Bandwidth_Net_Api_AccountTransaction.md">AccountTransaction</a>)<br />Collection with <a href ="T_Bandwidth_Net_Api_AccountTransaction.md">AccountTransaction</a> instances

## See Also


#### Reference
<a href ="T_Bandwidth_Net_Api_IAccount.md">IAccount Interface</a><br /><a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />