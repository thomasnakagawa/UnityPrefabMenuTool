using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;

public class MenuScriptGenerator
{
    private static readonly string MenuItemCodeTemplate =
        "    [MenuItem(\"{0}\", false, {1})] protected static void InstantiateMethod{2}() => PrefabMenuUtils.InstantiateInScene(\"{3}\");\n";

    private static readonly string MenuScriptCodeTemplate =
@"using UnityEditor;

/// <summary>
/// Auto-generated script created by Prefab Menu Builder tool in MenuScriptGenerator.cs
/// </summary>
public class {0}
{{
{1}}}
";

    private static string scriptClassName(string fileName)
    {
        return "uniqueclassname";
    }

    public static void GenerateMenuScript(List<PrefabMenuEditorWindow.PrefabMenuItem> items, string fileName)
    {
        // todo: validate input

        // Generate code for MenuItems
        string menuItemsCsCode = "";
        int methodIndex = 0;
        foreach (var item in items)
        {
            string assetPath = AssetDatabase.GetAssetPath(item.PrefabAsset);
            // todo: check that assetpath is valid

            menuItemsCsCode += string.Format(
                MenuItemCodeTemplate,
                item.MenuPath,
                item.Priority,
                methodIndex,
                assetPath
            );

            methodIndex += 1;
        }

        // Generate script code
        string FileContent = string.Format(
            MenuScriptCodeTemplate,
            scriptClassName(fileName),
            menuItemsCsCode
        );

        // Write code into file
        string FilePath = "Assets/Editor/PrefabMenuTool/BuiltMenus/" + fileName;
        (new FileInfo(FilePath)).Directory.Create();
        File.WriteAllText(FilePath, FileContent);

        // Import the new file
        AssetDatabase.ImportAsset(FilePath);
    }
}
