using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace EConfiguration
{
    public enum TableType { Object, Int, Float, Text }

    [CreateAssetMenu(fileName = "ConfigurationDatabase", menuName = "EConfiguration/ConfigurationDatabase")]
    public class ConfigurationDatabase : SerializedScriptableObject
    {
        [EnumToggleButtons]
        [HideLabel]
        public TableType TableType;
        [ShowIf("TableType", TableType.Object)]
        public Dictionary<string, Object> ObjectConfigs;
        [ShowIf("TableType", TableType.Int)]
        public Dictionary<string, int> IntConfigs;
        [ShowIf("TableType", TableType.Float)]
        public Dictionary<string, float> FloatConfigs;
        [ShowIf("TableType", TableType.Text)]
        public Dictionary<string, string> TextConfigs;

        public T GetAsset<T>(string key) where T : Object
        {
            if (!ObjectConfigs.ContainsKey(key))
                return null;
            return ObjectConfigs[key] as T;
        }

        public GameObject GetGameObject(string key)
        {
            return GetAsset<GameObject>(key);
        }

        public int GetInt(string key)
        {
            if (!IntConfigs.ContainsKey(key))
                return 0;
            return IntConfigs[key];
        }

        public float GetFloat(string key)
        {
            if (!FloatConfigs.ContainsKey(key))
                return 0f;
            return FloatConfigs[key];
        }

        public string GetText(string key)
        {
            if (!TextConfigs.ContainsKey(key))
                return "";
            return TextConfigs[key];
        }
    }
}