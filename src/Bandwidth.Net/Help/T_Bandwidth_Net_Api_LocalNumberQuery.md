﻿# LocalNumberQuery Class
 

Search criterias for local numbers


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;Bandwidth.Net.Api.LocalNumberQuery<br />&nbsp;&nbsp;&nbsp;&nbsp;<a href ="T_Bandwidth_Net_Api_LocalNumberQueryForOrder.md">Bandwidth.Net.Api.LocalNumberQueryForOrder</a><br />
**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0

## Syntax

**C#**<br />
``` C#
public class LocalNumberQuery
```

The LocalNumberQuery type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href ="M_Bandwidth_Net_Api_LocalNumberQuery__ctor.md">LocalNumberQuery</a></td><td>
Initializes a new instance of the LocalNumberQuery class</td></tr></table>&nbsp;
<a href="#localnumberquery-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_AreaCode.md">AreaCode</a></td><td>
A 3-digit telephone area code.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_City.md">City</a></td><td>
A city name.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_InLocalCallingArea.md">InLocalCallingArea</a></td><td>
Boolean value to indicate that the search for available numbers must consider overlayed areas. Only applied for localNumber searching.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_LocalNumber.md">LocalNumber</a></td><td>
It is defined as the first digits of a telephone number inside an area code for filtering the results. It must have at least 3 digits and the areaCode field must be filled.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_Quantity.md">Quantity</a></td><td>
The maximum number of numbers to return (default 10, maximum 5000).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_State.md">State</a></td><td>
A two-letter US state abbreviation ("CA" for California).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href ="P_Bandwidth_Net_Api_LocalNumberQuery_Zip.md">Zip</a></td><td>
A 5-digit US ZIP code.</td></tr></table>&nbsp;
<a href="#localnumberquery-class">Back to Top</a>

## See Also


#### Reference
<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />