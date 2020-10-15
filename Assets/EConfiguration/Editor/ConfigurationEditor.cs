using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

namespace EConfiguration
{
    public class ConfigurationEditor : OdinEditorWindow
    {
        [InlineEditor]
        public ConfigurationDatabase ConfigurationDatabase;


        [MenuItem("Window/ConfigurationEditor")]
        private static void OpenWindow()
        {
            var window = GetWindow<ConfigurationEditor>();
            window.Show();
        }

        protected override void OnGUI()
        {
            base.OnGUI();

        }
    }
}