using UnityEngine;
using UnityEditor;
using System.Collections;

public class AndroidMonitorShortcut {

    [MenuItem("Android/Open Monitor")]
    public static void OpenMonitor()
    {
        string workingDir = (EditorPrefs.GetString("AndroidSdkRoot") + @"\tools").Replace("/", @"\").Replace(@"\\", @"\");

        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = workingDir + @"\monitor.bat";
        proc.StartInfo.WorkingDirectory = workingDir;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = true;

        Debug.Log(proc.StartInfo.FileName);

        proc.Start();
    }
}
