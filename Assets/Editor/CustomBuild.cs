using UnityEditor;
using UnityEngine;

public class SpecialBuild
{
    private const string buildPath = "Builds/"+ GameName + "/" + GameName + ".exe";
    private const string GameName = "Bendight";
    private static string path = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + buildPath;
    private static string[] levels = new string[] { Paths.SCENE_MAIN };

    [MenuItem("MyTools/BuildAndRun %g")]
    public static void BuildAndRun()
    {
        if (BuildGame(levels, path))
        {
            RunGame();
            EditorApplication.isPlaying = true;
        }
    }

    private static bool BuildGame(string[] levels, string path)
    {
        PrepareBuild();

        string x = BuildPipeline.BuildPlayer(levels, path, BuildTarget.StandaloneWindows64, BuildOptions.Development);
        if (x.Contains("cancelled") || x.Contains("error"))
        {
            Debug.LogError(x);
            return false;
        }
        Debug.Log(x);
        return true;

    }

    private static void PrepareBuild()
    {
        KillGames();
        EditorApplication.isPlaying = false;
    }

    [MenuItem("MyTools/RunGame %q")]
    private static void RunGame()
    {
        var proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = path;
        proc.Start();
    }

    [MenuItem("MyTools/KillGames %k")]
    private static void KillGames()
    {
        foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(GameName))
        {
            process.Kill();
        }
    }
}
