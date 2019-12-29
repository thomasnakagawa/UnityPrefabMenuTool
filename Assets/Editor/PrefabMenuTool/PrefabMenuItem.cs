using UnityEngine;

namespace floatoat.PrefabMenuTool
{
    public class PrefabMenuItem
    {
        public GameObject PrefabAsset { get; set; }
        public int Priority { get; set; } = 10;
        public string MenuPath { get; set; } = "GameObject/my prefab";
    }
}
