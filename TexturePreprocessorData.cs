using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace EditorTools
{
    [Serializable]
    [CreateAssetMenu(fileName = "TexturePreprocessorData", menuName = "Tools/TexturePreprocessorData", order = 1)]
    public class TexturePreprocessorData : ScriptableObject
    {
        public bool affectCurrentFolder;
        public bool affectSubfolder;
#if ODIN_INSPECTOR
        [ListDrawerSettings(HideAddButton = false,Expanded = false,DraggableItems = true,HideRemoveButton = false)]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
#endif
        public List<TextureFilters> textureFilters = new List<TextureFilters>();

        private void OnValidate()
        {
            EditorUtility.SetDirty(this);
        }
        [Button("Save")]
        void Save()
        {
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }

    }

    [Serializable]
    public class TextureFilters
    {
        public string name = "new Filter";

        public List<string> nameFilters = new List<string>();
        public List<string> extensionFilters = new List<string>();
#if ODIN_INSPECTOR
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
#endif
        public Preset texturePreset;
    }
}