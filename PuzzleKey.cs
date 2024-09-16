using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class PuzzleKey : MonoBehaviour, ITakeDamage
{
    private enum PuzzleType{Contact, Elemental, Damage }
    [SerializeField] PuzzleType puzzleType;
    private ElementType activeType;
    [SerializeField, HideInInspector] private ElementType keyType;
    [SerializeField, HideInInspector] private int puzzleDamageThreshold;
    private SpriteRenderer spriteRenderer;
    [SerializeField, Space(10)] private Material[] materials;
    [SerializeField, Header("Object Effect")] private UnityEvent PuzzleAction;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }


    public void TakeDamage(float damage, float knockX, float knockY, ElementType elementDamage)
    {
        if (puzzleType == PuzzleType.Elemental)
        {
            if (elementDamage == keyType)
                PuzzleAction?.Invoke();

            switch (elementDamage)
            {
                case ElementType.Physical:
                    spriteRenderer.material = materials[(int)ElementType.Physical];
                    break;
                case ElementType.Electric:
                    spriteRenderer.material = materials[(int)ElementType.Electric];
                    break;
                case ElementType.Gravity:
                    spriteRenderer.material = materials[(int)ElementType.Gravity];
                    break;
                case ElementType.Light:
                    spriteRenderer.material = materials[(int)ElementType.Light];
                    break;
                case ElementType.Water:
                    spriteRenderer.material = materials[(int)ElementType.Water];
                    break;
                case ElementType.Fire:
                    spriteRenderer.material = materials[(int)ElementType.Fire];
                    break;
                case ElementType.Void:
                    spriteRenderer.material = materials[(int)ElementType.Void];
                    break;
                case ElementType.Ice:
                    spriteRenderer.material = materials[(int)ElementType.Ice];
                    break;
                default:
                    break;
            }
        }
        else if (puzzleType == PuzzleType.Contact)
        {
            PuzzleAction?.Invoke();
        }
        else if (puzzleType == PuzzleType.Damage)
        {
            if(damage >= puzzleDamageThreshold)
            {
                PuzzleAction?.Invoke();
            }
        }
        activeType = elementDamage;
    }
        #region Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(PuzzleKey))]
    public class PuzzleKeyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PuzzleKey puzzleKey = (PuzzleKey)target;
            EditorGUILayout.LabelField("Key Settings", EditorStyles.boldLabel);
            if(puzzleKey.puzzleType == PuzzleType.Elemental)
            {
                puzzleKey.keyType = (ElementType)EditorGUILayout.EnumPopup("ElementType", puzzleKey.keyType);
            }
            if (puzzleKey.puzzleType == PuzzleType.Damage)
            {
                puzzleKey.puzzleDamageThreshold = EditorGUILayout.IntSlider("Damage Threshold ", puzzleKey.puzzleDamageThreshold, 0, 50);
            }
        }
    }
    #endif
        #endregion
}
