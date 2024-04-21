using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;

namespace FlawareStudios.Translation
{
    public class TranslationModuleAssetHandler
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            TranslationModule module = EditorUtility.InstanceIDToObject(instanceID) as TranslationModule;
            if (module != null)
            {
                TranslationEditorWindow.OpenEditorWindow(module);
                return true;
            }
            return false;
        }
    }
}
#endif