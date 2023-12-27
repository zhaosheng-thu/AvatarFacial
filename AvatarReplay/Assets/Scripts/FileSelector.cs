using UnityEngine;
using UnityEditor;
using System.IO;

public class FileSelector : MonoBehaviour
{
    public string filepathFace;
    public string filepathEyes;
    public string filepathBody;
    public string filepathWav;
}

[CustomEditor(typeof(FileSelector))]
public class FileSelectorExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FileSelector fileSelector = (FileSelector)target;

        //EditorGUILayout.Space();

        //// 显示三个文件选择按钮，允许在Inspector面板中选择三个txt文件
        //fileSelector.filepathFace = DrawFileField("Face File", fileSelector.filepathFace, "txt");
        //fileSelector.filepathEyes = DrawFileField("Eyes File", fileSelector.filepathEyes, "txt");
        //fileSelector.filepathBody = DrawFileField("Body File", fileSelector.filepathBody, "txt");

        EditorGUILayout.Space();

        // 显示一个文件选择按钮，允许在Inspector面板中选择wav文件
        fileSelector.filepathWav = DrawFileField("File", fileSelector.filepathWav, "wav");
        if (fileSelector.filepathWav != null && fileSelector.filepathWav.IndexOf("voice") != -1)
        {
            string filepath = fileSelector.filepathWav.Replace(".wav", ".txt");
            //E:/Unity workspce/Avatar environment/Assets/FIle/VRchat/Voice/voice_gaoj_happy_MM7DD18_21_36_26.wav
            fileSelector.filepathFace = filepath.Replace("voice", "face").Replace("Voice", "Face");
            fileSelector.filepathEyes = filepath.Replace("voice", "eye").Replace("Voice", "Eye");
            fileSelector.filepathBody = filepath.Replace("voice", "body").Replace("Voice", "Body");
        }
    }

    private string DrawFileField(string label, string filePath, string extension)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);

        if (GUILayout.Button("Select"))
        {
            string path = EditorUtility.OpenFilePanel(label, "", extension);
            if (!string.IsNullOrEmpty(path))
            {
                filePath = path;
            }
        }

        EditorGUILayout.EndHorizontal();
        return filePath;
    }
}
