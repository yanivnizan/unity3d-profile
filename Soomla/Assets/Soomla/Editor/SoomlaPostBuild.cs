using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class PostProcessScriptStarter : MonoBehaviour {
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
#if UNITY_IOS
		string buildToolsDir = Application.dataPath + @"/Soomla/Editor/build-tools";

		string searchPattern = "Soomla_*.py";  // This would be for you to construct your prefix
		
		DirectoryInfo di = new DirectoryInfo(buildToolsDir);
		FileInfo[] files = di.GetFiles(searchPattern);

		foreach (FileInfo fi in files) { 
			Process proc = new Process();		
			proc.StartInfo.FileName = "python2.6";
//			UnityEngine.Debug.Log("Trying to run: " + fi.FullName);
			proc.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", fi.FullName, pathToBuiltProject);
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			proc.Start(); 
			string output = proc.StandardOutput.ReadToEnd();
			string err = proc.StandardError.ReadToEnd();
			proc.WaitForExit();
			UnityEngine.Debug.Log("out: " + output);
			if (proc.ExitCode != 0) {
				UnityEngine.Debug.Log("error: " + err + "   code: " + proc.ExitCode);
			}
		}
#endif
    }
}
