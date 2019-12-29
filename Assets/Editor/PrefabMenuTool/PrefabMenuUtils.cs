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
            // validate argument
            // check that prefabpath leads to a valid prefab
            // instantiate the prefab at the focus position
            // parent the instantiated prefab to the selected object if there is one
            // register Undo
        }
    }
}
