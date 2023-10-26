using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Text;

public class FacebookKeyHash : EditorWindow
{
    [MenuItem("Facebook/Key Hash Tool")]
    public static void FacebookKeyHashMenu()
    {
        EditorWindow.GetWindow(typeof(FacebookKeyHash));
    }

    private string alias;
    private string keyStore;
    private string password;
    private string keyHash;
    private string error;

    void OnGUI()
    {
        GUI.skin.label.wordWrap = true;
        GUILayout.BeginVertical();
        {
            GUILayout.Space(10);
            GUILayout.Label("Facebook Key Hash Tool", EditorStyles.boldLabel);

            string oldAlias = alias, oldKeyStore = keyStore, oldPassword = password;

            alias = EditorGUILayout.TextField("alias", alias);

            keyStore = EditorGUILayout.TextField("keyStore", keyStore);

            password = EditorGUILayout.TextField("password", password);

            if (string.IsNullOrEmpty(alias) || string.IsNullOrEmpty(keyStore) || string.IsNullOrEmpty(password))
            {
                keyHash = null;
            }
            else if (oldAlias != alias || oldKeyStore != keyStore || oldPassword != password)
            {
                error = TestGetKeyHash(alias, keyStore, password);

                if (string.IsNullOrEmpty(error))
                {
                    keyHash = GetKeyHash(alias, keyStore, password);
                }
                else
                {
                    keyHash = null;
                }
            }


            EditorGUILayout.TextField("Key Hash", keyHash);

            EditorGUILayout.TextArea(error, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            GUILayout.Space(10);
        }
        GUILayout.EndVertical();
        
    }

    private static string TestGetKeyHash(string alias, string keyStore, string password)
    {
        var proc = new Process();
        var arguments = @"""keytool -storepass {0} -keypass {1} -exportcert -alias {2} -keystore {3}""";

        proc.StartInfo.FileName = "cmd";
        arguments = @"/C " + arguments;

        proc.StartInfo.Arguments = string.Format(arguments, password, password, alias, keyStore);
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();
        var keyHash = new StringBuilder();
        while (!proc.HasExited)
        {
            keyHash.Append(proc.StandardOutput.ReadToEnd());
        }

        string ret = keyHash.ToString().TrimEnd('\n');
        if (ret.ToLower().StartsWith("keytool error:"))
        {
            return ret;
        }
        else
        {
            return null;
        }
    }


    private static string GetKeyHash(string alias, string keyStore, string password)
    {
        var proc = new Process();
        var arguments = @"""keytool -storepass {0} -keypass {1} -exportcert -alias {2} -keystore {3} | openssl sha1 -binary | openssl base64""";

        proc.StartInfo.FileName = "cmd";
        arguments = @"/C " + arguments;

        proc.StartInfo.Arguments = string.Format(arguments, password, password, alias, keyStore);
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();
        var keyHash = new StringBuilder();
        while (!proc.HasExited)
        {
            keyHash.Append(proc.StandardOutput.ReadToEnd());
        }

        switch (proc.ExitCode)
        {
            case 255:
                return null;
        }

        return keyHash.ToString().TrimEnd('\n');
    }

}
