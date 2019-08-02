#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class BYDataTableMaker : MonoBehaviour
{
    [MenuItem("Assets/BY/Make Data by CSV")]
    public static void CreateAsset()
    {
        foreach (UnityEngine.Object obj in Selection.objects)
        {
            TextAsset csvFile = (TextAsset)obj;
            string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(csvFile));
            ScriptableObject scriptableTable = ScriptableObject.CreateInstance(tableName);

            if (scriptableTable == null)
                return;

            AssetDatabase.CreateAsset(scriptableTable, "Assets/Resources/DataTable/" + tableName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            BYDataTableOgrin by = (BYDataTableOgrin)scriptableTable;
            by.ImportData(csvFile.text);

            EditorUtility.SetDirty(by);
        }
    }
}
#endif