using System.Runtime.CompilerServices;

// The test project reaches JsoncEditor.Quote, which is internal because it is the writer's
// escaping step rather than an API callers use on its own.
//
// ⚠ This must name the test ASSEMBLY that exists, and a wrong name fails silently: the grant simply
// goes nowhere, and only the test that needs it notices. This repository has one test project.
[assembly: InternalsVisibleTo("JsonC.Tests")]
