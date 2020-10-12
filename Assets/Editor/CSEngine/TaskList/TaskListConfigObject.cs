using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.Utilities.Editor;

namespace CSEngine.Editor
{
    [Serializable]
    public class TaskConfig
    {
        [ToggleGroup("Enabled", "@Title")]
        public bool Enabled;
        [ToggleGroup("Enabled")]
        [HideLabel]
        [GUIColor(.8f, .8f, .8f)]
        public string Title = "任务标题";
        [ToggleGroup("Enabled")]
        [HideLabel]
        [TextArea(2, 4)]
        [GUIColor(1,1,1,.8f)]
        public string Description = "任务描述";
    }

    [CreateAssetMenu(fileName ="任务计划列表", menuName = "任务计划列表")]
    public class TaskListConfigObject : SerializedScriptableObject
    {
        [LabelText("任务计划列表")]
        public List<TaskConfig> TaskConfigs = new List<TaskConfig>();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TaskListConfigObject))]
    public class TaskListConfigObjectInspector : OdinEditor
    {
        protected override void OnHeaderGUI()
        {
            //base.OnHeaderGUI();
            EditorGUILayout.Space(10);
            SirenixEditorGUI.Title("任务清单列表", "", TextAlignment.Center, false, boldLabel: true);
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            var taskListConfigObject = target as TaskListConfigObject;
            TaskConfig remove = null;
            foreach (var taskConfig in taskListConfigObject.TaskConfigs)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();

                var visible = EditorPrefs.GetBool($"{taskConfig.GetHashCode()}", false);
                var toggeled = SirenixEditorGUI.BeginToggleGroup(taskConfig, ref taskConfig.Enabled, ref visible, taskConfig.Title);
                if (toggeled)
                {
                    taskConfig.Title = EditorGUILayout.TextField(taskConfig.Title);
                    taskConfig.Description = EditorGUILayout.TextArea(taskConfig.Description);
                }
                EditorPrefs.SetBool($"{taskConfig.GetHashCode()}", visible);
                SirenixEditorGUI.EndToggleGroup();

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    remove = taskConfig;
                }

                EditorGUILayout.EndHorizontal();
            }
            if (remove != null)
            {
                taskListConfigObject.TaskConfigs.Remove(remove);
            }
            if (GUILayout.Button("+"))
            {
                taskListConfigObject.TaskConfigs.Add(new TaskConfig());
            }
        }
    }
#endif
}