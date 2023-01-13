namespace Editor.Builder
{
    using System;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public static class UnityBuilder
    {
        [MenuItem("Build/🤖 Android")]
        public static void BuildAndroid()
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                target = BuildTarget.Android,
                locationPathName = "./Builds/Android/IdleSlime.apk",
                scenes = new [] { "Assets/Scenes/Game.unity" }
            };

            var report = BuildPipeline.BuildPlayer
            (
                buildPlayerOptions
            );

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception
                (
                    "Failed to build Android with " 
                    + report.summary 
                    + " errors. See log for details."
                );
            }

            Debug.Log("Build result is: " + report.summary.result);
            Debug.Log("Path: " + report.summary.outputPath);
        }
    }
}
