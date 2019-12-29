using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabMenuEditorWindow : EditorWindow
{
    public class PrefabMenuItem
    {
        public GameObject PrefabAsset { get; set; }
        public int Priority { get; set; } = 10;
        public string MenuPath { get; set; } = "GameObject/my prefab";
    }

    private static List<PrefabMenuItem> Items;
    private static Vector2 scrollPosition = Vector2.zero;
    private static string FileName = "PrefabMenu.cs";

    static PrefabMenuEditorWindow()
    {
        Items = new List<PrefabMenuItem>();
    }

    [MenuItem("Tools/Prefab Menu Builder")]
    protected static void InitWindow() => GetWindow(typeof(PrefabMenuEditorWindow), false, "Prefab menu builder").Show();

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        var listCopy = Items.ToList(); // use a copy of the list so the original can be modified while the copy is being rendered
        for (int i = 0; i < Items.Count; i++)
        {
            var item = listCopy[i];

            // top row
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            // delete button
            if (GUILayout.Button(new GUIContent("x", "Delete menu item"), GUILayout.ExpandWidth(false)))
            {
                Items.Remove(item);
            }

            // label
            GUILayout.Label("Item " + i);

            GUILayout.FlexibleSpace();

            // move up button
            GUI.enabled = i > 0; // cannot move first item up
            if (GUILayout.Button(new GUIContent("↑", "Move up"), GUILayout.ExpandWidth(false)))
            {
                // swap up
                var temp = item;
                Items[i] = Items[i - 1];
                Items[i - 1] = temp;
                Repaint();
            }

            // move down button
            GUI.enabled = i < listCopy.Count - 1; // cannot move last item down
            if (GUILayout.Button(new GUIContent("↓", "Move down"), GUILayout.ExpandWidth(false)))
            {
                // swap down
                var temp = item;
                Items[i] = Items[i + 1];
                Items[i + 1] = temp;
                Repaint();
            }
            GUILayout.EndHorizontal();

            // main data fields
            GUI.enabled = true;
            item.PrefabAsset = (GameObject)EditorGUILayout.ObjectField("Prefab asset", item.PrefabAsset, typeof(GameObject), false);
            item.MenuPath = EditorGUILayout.TextField("Menu path", item.MenuPath);
            item.Priority = EditorGUILayout.IntField("Priority", item.Priority);

            // item seperator
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), Color.grey);
        }

        // new item button
        if (GUILayout.Button(new GUIContent("+", "Add new menu item")))
        {
            Items.Add(new PrefabMenuItem());
        }

        // file name field
        FileName = EditorGUILayout.TextField("Menu script file name", FileName);

        // generate button
        GUI.enabled = listCopy.Count > 0 && FileName.Length > 0;
        if (GUILayout.Button("Generate menu script"))
        {
            MenuScriptGenerator.GenerateMenuScript(Items, FileName);
        }

        GUI.enabled = true;
        GUILayout.EndScrollView();
    }
}
