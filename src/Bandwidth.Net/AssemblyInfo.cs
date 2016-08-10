using System.Reflection;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("Bandwidth")]
[assembly: AssemblyProduct("Bandwidth.Net")]
[assembly: AssemblyTrademark("Bandwidth")]

#if DEBUG
// Allow to tests to see internal members
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Bandwidth.Net.Test")]
#endif
