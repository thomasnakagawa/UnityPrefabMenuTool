using UnityEngine;

namespace float_oat.PrefabMenuTool
{
    /// <summary>
    /// Represents one configuration of a menu item
    /// </summary>
    [System.Serializable]
    public class PrefabMenuItem
    {
        public GameObject PrefabAsset;
        public int Priority;
        public string MenuPath;

        public PrefabMenuItem()
        {
            Priority = 10;
            MenuPath = "GameObject/my prefab";
        }
    }
}
