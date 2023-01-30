#addin nuget:?package=Cake.Unity&version=0.9.0
#tool nuget:?package=Cake.Tool&version=3.0.0

#tool nuget:?package=NuGet.CommandLine&version=6.4.0

#tool nuget:?package=NUnit&version=3.13.3
#tool nuget:?package=NUnit.ConsoleRunner&version=3.16.2
#tool nuget:?package=NUnit.Extension.NUnitProjectLoader&version=3.7.1
#tool nuget:?package=NUnit3TestAdapter&version=4.3.1

using static Cake.Unity.Arguments.BuildTarget;

var target = Argument("target", "Build-Android");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean-Android")    
    .Does(() =>
{
    CleanDirectory($"./Builds/Android");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean-Android")
	.Does(() =>
{
	NuGetRestore("./IdleSlime.sln");
});

Task("Build-Android")    
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    UnityEditor
    (
        new UnityEditorArguments
        {
            ProjectPath = ".",
            ExecuteMethod = "Editor.Builder.UnityBuilder.BuildAndroid",
            BuildTarget = Android,
            TestPlatform = TestPlatform.Android,        
            LogFile = "./Builds/Android/unity.log",
        },
        new UnityEditorSettings 
        {
            RealTimeLog = true
        }
    );
});

Task("Test-Android")
    .IsDependentOn("Build-Android")    
    .Does(() =>
{    
    var testAssemblies = GetFiles("Library/ScriptAssemblies/*.Tests.dll");

    NUnit3
    (
        testAssemblies,
        new NUnit3Settings 
        {
		    NoResults = false,
            Work = "./Builds/Android/"
        }
    );
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);