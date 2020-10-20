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
        //[HorizontalGroup("Split/������Ϣ", 80, LabelWidth = 70)]
        [BoxGroup("������Ϣ")]
        [PreviewField(60, ObjectFieldAlignment.Left)]
        [AssetsOnly]
        [LabelText(IconLabel)]
        public GameObject Prefab;

        //[HorizontalGroup("Split/������Ϣ")]
        //[BoxGroup("������Ϣ")]
        //[HideLabel]
        //[TextArea]
        //public string Description = "";

        //[VerticalGroup("Split/Meta")]
        [BoxGroup("������Ϣ")]
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

        //��ɫ��λ
        //[HorizontalGroup("Split", 290), EnumToggleButtons, HideLabel]
        //public CharacterAlignment CharacterAlignment;

        //[TabGroup("Starting Inventory")]
        //public ItemSlot[,] Inventory = new ItemSlot[12, 6];

        //[TabGroup("��ʼ״̬"), HideLabel]
        //public CharacterStats Skills = new CharacterStats();

        //[HideLabel]
        //[TabGroup("Starting Equipment")]
        //public CharacterEquipment StartingEquipment;


        const string IconLabel = "Ԥ��";
        const string NameLabel = "����";
        const string HealthLabel = "����ֵ";
        const string AttackLabel = "����";
        const string DefenseLabel = "����";
        const string PowerLabel = "����";
        const string BasicAttrbuteLabel = "��������";
        const string SurnameLabel = "����";
        const string AgeLabel = "�ȼ�";
    }
}
#endif
