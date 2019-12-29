using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace floatoat.PrefabMenuTool
{
    public static class MenuScriptGenerator
    {
        private static readonly string MenuItemCodeTemplate =
            "        [MenuItem(\"{0}\", false, {1})] protected static void InstantiateMethod{2}() => PrefabMenuUtils.InstantiateInScene(\"{3}\");\n";

        private static readonly string MenuScriptCodeTemplate =
    @"using UnityEditor;

/// <summary>
/// Generated script created by Prefab Menu Builder tool in MenuScriptGenerator.cs
/// </summary>
namespace floatoat.PrefabMenuTool.Menus
{{
    public sealed class {0} 
    {{
{1}    }}
}}
";

        public static void GenerateMenuScript(List<PrefabMenuItem> items, string fileName)
        {
            ValidateItems(items);
            ValidateFileName(fileName);

            // Generate code for MenuItems
            string menuItemsCsCode = "";
            int methodIndex = 0;
            foreach (var item in items)
            {
                string assetPath = AssetDatabase.GetAssetPath(item.PrefabAsset);

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
                ScriptClassName(fileName),
                menuItemsCsCode
            );

            // Write code into file
            string FilePath = "Assets/Editor/PrefabMenuTool/BuiltMenus/" + FileNameWithExtension(fileName);
            (new FileInfo(FilePath)).Directory.Create();
            File.WriteAllText(FilePath, FileContent);
            EditorUtility.DisplayDialog("Menu generated", "Menu script file was generated and was saved to " + FilePath, "Ok");

            // Import the new file
            AssetDatabase.ImportAsset(FilePath);
        }

        private static string ScriptClassName(string fileName)
        {
            return "_" + (new Regex("[^a-zA-Z0-9 -]")).Replace(fileName, "");
        }

        private static string FileNameWithExtension(string fileName)
        {
            if (fileName.EndsWith(".cs", System.StringComparison.Ordinal) == false)
            {
                return fileName + ".cs";
            }
            return fileName;
        }

        private static void ValidateItems(List<PrefabMenuItem> items)
        {
            if (items.Count < 1)
            {
                throw new System.ArgumentException("Must have at least one item to generate menu script");
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (string.IsNullOrEmpty(items[i].MenuPath))
                {
                    throw new System.ArgumentException("Item " + i + " needs a valid menu path");
                }

                if (items[i].PrefabAsset == null)
                {
                    throw new System.ArgumentException("Item " + i + " cannot have a null prefab");
                }

                string assetPath = AssetDatabase.GetAssetPath(items[i].PrefabAsset);
                if (string.IsNullOrEmpty(assetPath))
                {
                    throw new System.ArgumentException("Item " + i + "'s asset has no path");
                }
            }
        }

        private static void ValidateFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new System.ArgumentException("Filename must be valid");
            }
        }
    }
}
