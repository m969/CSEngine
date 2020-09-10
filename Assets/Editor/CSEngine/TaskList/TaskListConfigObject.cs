using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

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
}