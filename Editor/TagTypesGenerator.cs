using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Bipolar.Editor
{
    //[InitializeOnLoad]
    public class TagTypesGenerator
    {
        private const string tagTypesFileDirectory = "Assets";
        private const string tagTypesClassName = "Tag";
        private static readonly Regex notValidCharacters = new Regex("[^a-zA-Z0-9_]");

        private static string FilePath => $"{tagTypesFileDirectory}/{tagTypesClassName}.cs";

        static TagTypesGenerator()
        {
            if (File.Exists(FilePath) == false)
            {
                Generate();
                return;
            }
            string oldClassContent = File.ReadAllText(FilePath);
            string newClassContent = GetClassContent();
            if (newClassContent != oldClassContent)
            {
                Generate(newClassContent);
            }
        }

        private static void Generate()
        {
            Generate(GetClassContent());
        }

        private static void Generate(string content)
        {
            var path = FilePath;
            AssetDatabase.MakeEditable(path);
            File.WriteAllText(path, content); 
            AssetDatabase.ImportAsset(path);
            AssetDatabase.SaveAssets();
        }

        private static string GetClassContent()
        {
            var tagsArray = UnityEditorInternal.InternalEditorUtility.tags;

            var builder = new StringBuilder();
            builder.AppendLine($"public static class {tagTypesClassName}");
            builder.AppendLine("{");
            
            for (int i = 0; i < tagsArray.Length; i++)
            {
                string tag = tagsArray[i];
                tag = notValidCharacters.Replace(tag, string.Empty);
                if (string.IsNullOrEmpty(tag))
                    continue;

                builder.AppendLine($"\tpublic const string {tag} = @\"{tagsArray[i]}\";");
            }

            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
