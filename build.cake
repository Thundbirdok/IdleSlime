#addin nuget:?package=Cake.Unity&version=0.9.0
#tool nuget:?package=Cake.Tool&version=3.0.0

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

Task("Build-Android")
    .IsDependentOn("Clean-Android")
    .Does(() =>
{
    UnityEditor
    (
        new UnityEditorArguments
        {
            ExecuteMethod = "Editor.Builder.UnityBuilder.BuildAndroid",
            BuildTarget = Android,
            LogFile = "./Builds/Android/unity.log"
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
    Console.WriteLine("Tests would be here");    
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);