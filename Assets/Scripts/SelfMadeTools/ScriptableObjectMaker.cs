#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;



public static class ScriptableObjectMaker
{

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Instance of Selected Script", false, 10)]
    private static void CreateScriptableObjectInstance()
    {
        Object selectedObject = Selection.objects[0];

        if (selectedObject != null && (selectedObject is MonoScript script && script.GetClass().IsSubclassOf(typeof(ScriptableObject))))
        {
            ScriptableObject instance = ScriptableObject.CreateInstance(script.GetClass());
            string path = AssetDatabase.GetAssetPath(selectedObject);
            path = Path.GetDirectoryName(path);
            Debug.Log("path: " + path);
            if (AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateAsset(instance, AssetDatabase.GenerateUniqueAssetPath($"{path}/{script.name}.asset"));
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = instance;
                Debug.Log("creation success");
            }
        }
    }

    [MenuItem("Assets/Create/Instance of Selected Script", true, 10)]
    private static bool ValidateCreateScriptableObjectInstance()
    {
        Object selectedObject = Selection.objects[0];

        if (selectedObject == null || !(selectedObject is MonoScript script && script.GetClass().IsSubclassOf(typeof(ScriptableObject))))
        {
            return false;
        }
        return true;
    }

#endif
}