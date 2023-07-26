using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    [Serializable]
    public class TexturePreprocessor : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;

            List<TexturePreprocessorData> textureFiltersData = new List<TexturePreprocessorData>();
            List<TextureFilters> textureFilters = new List<TextureFilters>();

            var texName = Path.GetFileNameWithoutExtension(assetPath);
            var texExtension = Path.GetExtension(assetPath);
            texExtension = texExtension.Remove(0, 1);

            var guids = AssetDatabase.FindAssets("t:TexturePreprocessorData");
            foreach (var guid in guids)
            {
                var datapath = AssetDatabase.GUIDToAssetPath(guid);
                var data = AssetDatabase.LoadAssetAtPath<TexturePreprocessorData>(datapath);

                if (data.affectCurrentFolder)
                {
                    if (Path.GetDirectoryName(assetPath) == Path.GetDirectoryName(datapath))
                    {
                        textureFiltersData.Add(data);
                    }
                }
                if (data.affectSubfolder)
                {
                    if (Path.GetDirectoryName(assetPath).Contains(Path.GetDirectoryName(datapath)) && Path.GetDirectoryName(assetPath) != Path.GetDirectoryName(datapath))
                    {
                        textureFiltersData.Add(data);
                    }
                }
            }

            foreach (var data in textureFiltersData)
            {
                foreach (var filter in data.textureFilters)
                {
                    textureFilters.Add(filter);
                }
            }

            foreach (var filters in textureFilters)
            {
                foreach (var stringFilters in filters.nameFilters)
                {
                    foreach (var extensionFilters in filters.extensionFilters)
                    {
                        bool nameMatch = false;
                        bool extensionMatch = false;
                        if (texName.Contains(stringFilters, System.StringComparison.OrdinalIgnoreCase) || stringFilters == "all")
                        {
                            nameMatch = true;
                        }
                        if (texExtension == extensionFilters || extensionFilters == "all") 
                        {
                            extensionMatch = true;
                        }

                        if (nameMatch && extensionMatch)
                        {
                            ApplyTexture(textureImporter, filters);
                            break;
                        }
                    }
                }
            }
        }

        private static void ApplyTexture(TextureImporter textureImporter, TextureFilters filters)
        {
            filters.texturePreset.ApplyTo(textureImporter);
        }
    }
}
