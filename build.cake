var target = Argument("target", "Build-Android");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean-Android")    
    .Does(() =>
{
    CleanDirectory($"./Builds/Android");
});

Task("Build-Android")
    .IsDependentOn("Clean-Android")
    .Does(() =>
{
    Console.WriteLine("Build would be here");
});

Task("Test-Android")
    .IsDependentOn("Build-Android")
    .Does(() =>
{
    Console.WriteLine("Tests would be here");    
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);