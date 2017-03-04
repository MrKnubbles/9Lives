using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;

/*
 * Sprite-editor Template
 * http://sunbug.net
 * 
 */

public class zSpriteEditor : Editor {
    int popupMenuIndex;
    string[] sortingLayerNames;

    public virtual void OnEnable()
    {
        sortingLayerNames = zCommonEditor.GetSortingLayerNames();

        SpriteRenderer rndr = target as SpriteRenderer;
        string name = rndr.sortingLayerName;
        for (int i = 0; i < sortingLayerNames.Length; i++)
            if (name == sortingLayerNames[i])
            {
                popupMenuIndex = i;
                break;
            }
    }

    protected virtual void OnParamChanged() {}

    public override void OnInspectorGUI()
    {
        SpriteRenderer rndr = target as SpriteRenderer;

        EditorGUI.BeginChangeCheck();

        Sprite new_spr = (Sprite)EditorGUILayout.ObjectField("Sprite", rndr.sprite, typeof(Sprite), 
            GUILayout.Height(EditorGUIUtility.singleLineHeight)
            );

        Color color = EditorGUILayout.ColorField("Color", rndr.color);
        Material mat = (Material)EditorGUILayout.ObjectField("Material", rndr.sharedMaterial, typeof(Material), false);

        //GUILayout.Label("");
        popupMenuIndex = EditorGUILayout.Popup("Sorting Layer", popupMenuIndex, sortingLayerNames);
        int sorting_order = EditorGUILayout.IntField("Order in Layer", rndr.sortingOrder);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(rndr, "Sprite Renderer");
            rndr.sprite = new_spr;
            rndr.color = color;
            rndr.sharedMaterial = mat;
            rndr.sortingOrder = sorting_order;
            rndr.sortingLayerName = sortingLayerNames[popupMenuIndex];
            EditorUtility.SetDirty(target);
            OnParamChanged();
        }
    }
}
