using System;
using UnityEditor;
using UnityEngine;

namespace float_oat.PrefabMenuTool
{
    /// <summary>
    /// Class with methods for generated menu scripts to call
    /// </summary>
    public static class PrefabMenuUtils
    {
        public static void InstantiateInScene(string prefabPath)
        {
            try
            {
                // validate argument
                if (string.IsNullOrEmpty(prefabPath))
                {
                    throw new ArgumentException("Prefab path cannot be empty");
                }

                // check that prefabpath leads to a valid prefab
                GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefabObject == null)
                {
                    throw new System.IO.FileNotFoundException("Could not find prefab at " + prefabPath + ". The prefab file may have been moved, renamed or deleted.");
                }

                // instantiate the prefab at the focus position
                GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabObject);

                // parent the instantiated prefab to the selected object if there is one
                if (Selection.activeTransform != null)
                {
                    prefabInstance.transform.parent = Selection.activeTransform;
                }

                // select the new object
                Selection.activeGameObject = prefabInstance;

                // register Undo
                Undo.RegisterCreatedObjectUndo(prefabInstance, "Created " + prefabInstance.name);

            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Could not instantiate prefab", e.Message, "Ok");
                throw e;
            }
        }
    }
}
