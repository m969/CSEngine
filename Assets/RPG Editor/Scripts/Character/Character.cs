#if UNITY_EDITOR
namespace RPGEditor
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    //
    // Instead of adding [CreateAssetMenu] attribute, we've created a Scriptable Object Creator using Odin Selectors.
    // Characters can then be easily created in the RPG Editor window, which also helps ensure that they get located in the right folder.
    //
    // By inheriting from SerializedScriptableObject, we can then also utilize the extra serialization power Odin brings.
    // In this case, Odin serializes the Inventory which is a two-dimensional array. Everything else is serialized by Unity.
    // 

    public class Character : SerializedScriptableObject
    {
        //[HorizontalGroup("Split/基本信息", 80, LabelWidth = 70)]
        [BoxGroup("基本信息")]
        [PreviewField(60, ObjectFieldAlignment.Left)]
        [AssetsOnly]
        [LabelText(IconLabel)]
        public GameObject Prefab;

        //[HorizontalGroup("Split/基本信息")]
        //[BoxGroup("基本信息")]
        //[HideLabel]
        //[TextArea]
        //public string Description = "";

        //[VerticalGroup("Split/Meta")]
        [BoxGroup("基本信息")]
        [LabelText(NameLabel)]
        public string Name;

        [BoxGroup(BasicAttrbuteLabel)]
        [LabelText(HealthLabel)]
        public uint Health;

        [BoxGroup(BasicAttrbuteLabel)]
        [LabelText(PowerLabel)]
        public uint Power;

        [BoxGroup(BasicAttrbuteLabel)]
        [LabelText(AttackLabel)]
        public uint Attack;

        [BoxGroup(BasicAttrbuteLabel)]
        [LabelText(DefenseLabel)]
        public uint Defense;

        //[VerticalGroup("Split/Meta")]
        //[LabelText(SurnameLabel)]
        //public string Surname;

        //[VerticalGroup("Split/Meta"), Range(0, 100)]
        //[LabelText(AgeLabel)]
        //public int Age;

        //角色定位
        //[HorizontalGroup("Split", 290), EnumToggleButtons, HideLabel]
        //public CharacterAlignment CharacterAlignment;

        //[TabGroup("Starting Inventory")]
        //public ItemSlot[,] Inventory = new ItemSlot[12, 6];

        //[TabGroup("初始状态"), HideLabel]
        //public CharacterStats Skills = new CharacterStats();

        //[HideLabel]
        //[TabGroup("Starting Equipment")]
        //public CharacterEquipment StartingEquipment;


        const string IconLabel = "预设";
        const string NameLabel = "名称";
        const string HealthLabel = "生命值";
        const string AttackLabel = "攻击";
        const string DefenseLabel = "防御";
        const string PowerLabel = "能量";
        const string BasicAttrbuteLabel = "基础属性";
        const string SurnameLabel = "姓氏";
        const string AgeLabel = "等级";
    }
}
#endif
