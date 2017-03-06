using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

/*
 * Custom sprite-pivot editor, trapung, 2015.
 * http://sunbug.net
 *
 */

[CustomEditor(typeof(SpriteRenderer), true)]
public class zSpritePivotExt : zSpriteEditor {
    ////// statics
    static bool _LockPosition = false;
    static bool _PixelAlign = false;
    static List<Vector3> _Childs = new List<Vector3>();
    
    static string[] _AlignStrs = null;
    static Vector3[] _EditRect = new Vector3[4];
    static Vector3[] _PixelRect = new Vector3[4];
    static Vector2 _SavedPivot = Vector2.zero;
    static int _SavedAlignment = (int)SpriteAlignment.Center;
    static string _SavedName = null;


    ////// members
    Vector3 _PrevPos;
    Vector3 _CapPos;
    int _Alignment = (int)SpriteAlignment.Center;
    bool _OnEdit = false;
    bool _AlignParentPixel = false;
    bool _SnapDown = false;

    public override void OnEnable()
    {
        base.OnEnable();
        OnParamChanged();
    }

    public virtual void OnDisable()
    {
        if (_OnEdit) OnEditCancel();
    }

    protected override void OnParamChanged()
    {
        Sprite spr = ((SpriteRenderer)target).sprite;
        if (spr == null) return;
        
        _Alignment = zCommonEditor.GetSpriteAlignment(spr);
        if (_OnEdit) OnEditCancel();

        if (_AlignStrs == null)
        {
            int len = ((int)SpriteAlignment.Custom) + 1;
            _AlignStrs = new string[len];
            for (int i = 0; i < len; i++)
                _AlignStrs[i] = ((SpriteAlignment)i).ToString();
        }

        _AlignParentPixel = false;
        _SnapDown = false;
    }

    void PushChildren()
    {
        Transform tr = (target as SpriteRenderer).transform;        
        _Childs.Clear();
        for (int i = 0; i < tr.childCount;i++ ) _Childs.Add(tr.GetChild(i).position);
    }

    void PopChildren()
    {
        Transform tr = (target as SpriteRenderer).transform;
        if (tr.childCount != _Childs.Count) return;

        for (int i = 0; i < tr.childCount;i++ ) tr.GetChild(i).position = _Childs[i];

        _Childs.Clear();
    }

    void UpdatePivot( bool pixel_align = false, SpriteAlignment align = SpriteAlignment.Custom)
    {
        SpriteRenderer rndr = target as SpriteRenderer;
        Vector2 new_pivot = Vector2.zero;

        ////// gets new pivot point
        if (align == SpriteAlignment.Custom)
        {
            new_pivot = rndr.zWorldPositionToPivot(_CapPos, pixel_align);
        } else
        {
            switch ((SpriteAlignment)align)
            {
                case SpriteAlignment.Center: new_pivot = new Vector2(0.5f, 0.5f); break;
                case SpriteAlignment.TopLeft: new_pivot = new Vector2(0f, 1f); break;
                case SpriteAlignment.TopCenter: new_pivot = new Vector2(0.5f, 1f); break;
                case SpriteAlignment.TopRight: new_pivot = new Vector2(1f, 1f); break;
                case SpriteAlignment.LeftCenter: new_pivot = new Vector2(0f, 0.5f); break;
                case SpriteAlignment.RightCenter: new_pivot = new Vector2(1f, 0.5f); break;
                case SpriteAlignment.BottomLeft: new_pivot = new Vector2(0f, 0f); break;
                case SpriteAlignment.BottomCenter: new_pivot = new Vector2(0.5f, 0f); break;
                case SpriteAlignment.BottomRight: new_pivot = new Vector2(1f, 0f); break;
            }
        }
        
        ////// updates new pivot
        UpdatePivotTo(new_pivot, pixel_align, align);
    }

    void UpdatePivotTo( Vector2 new_pivot, bool pixel_align = false, SpriteAlignment align = SpriteAlignment.Custom)
    {
        SpriteRenderer rndr = target as SpriteRenderer;
        Sprite spr = rndr.sprite;
        Vector2 prev_pivot = spr.zNormlizedPivot();

        _OnEdit = false;

        Vector3 dPworld = rndr.zPivotToWorldPosition(new_pivot) - rndr.zPivotToWorldPosition(prev_pivot);
        dPworld.z = 0.0f;
        zCommonEditor.SetSpritePivot(spr, new_pivot, (int)align);
        _Alignment = (int)align;

        if (_LockPosition)
            rndr.transform.position = _PrevPos;
        else
            rndr.transform.position += dPworld;

        PopChildren();
    }

    void OnEditStart()
    {
        _OnEdit = true;
        PushChildren();
        _CapPos = _PrevPos = (target as SpriteRenderer).transform.position;
        UpdateEditRect();
        SceneView.RepaintAll();
    }

    void OnEditCancel()
    {
        SpriteRenderer rndr = target as SpriteRenderer;

        // restores object's position
        rndr.transform.position = _PrevPos;

        _OnEdit = false;
        SceneView.RepaintAll();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SpriteRenderer rndr = target as SpriteRenderer;
        Sprite spr = rndr.sprite;
        if (spr == null) return;

        GUILayout.Label("");
        //EditorGUI.BeginDisabledGroup(true);
        //bool parent_pixel_snap = EditorGUILayout.Toggle("Snap To Parent's pixel", _AlignParentPixel);
        //if (parent_pixel_snap != _AlignParentPixel)
        //{
        //    _AlignParentPixel = parent_pixel_snap;
        //    if (_AlignParentPixel) SceneView.RepaintAll();
        //}
        //EditorGUI.EndDisabledGroup();

        GUILayout.Label("Key[A]+Move : snap to parent's pixel");

        Vector2 pv = spr.zNormlizedPivot();
        EditorGUILayout.HelpBox( "Pivot:(" + pv.x.ToString() + ", " + pv.y.ToString() + ")" , MessageType.None );

        _LockPosition = EditorGUILayout.Toggle("Lock Position", _LockPosition);
        _PixelAlign = EditorGUILayout.Toggle("Align to Pixel", _PixelAlign);

        if (!_OnEdit)
        {
            EditorGUI.BeginChangeCheck();
            int align = EditorGUILayout.Popup("Presets", _Alignment, _AlignStrs);
            if (EditorGUI.EndChangeCheck())
            {
                OnEditStart();
                UpdatePivot(false, (SpriteAlignment)align);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Edit")) OnEditStart();
            if (GUILayout.Button("Copy"))
            {
                _SavedPivot = pv;
                _SavedAlignment = _Alignment;
                _SavedName = spr.name;
            }
            EditorGUI.BeginDisabledGroup(_SavedName==null);
            if (GUILayout.Button("Paste"))
            {
                OnEditStart();
                UpdatePivotTo( _SavedPivot, false, (SpriteAlignment)_SavedAlignment);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (_SavedName != null)
                EditorGUILayout.HelpBox("Clip:" + _SavedName + " (" + _SavedPivot.x.ToString() + ", " + _SavedPivot.y.ToString() + ")", MessageType.None);
        } else
        {
            if (GUILayout.Button("Apply")) UpdatePivot(_PixelAlign);
            if (GUILayout.Button("Cancel")) OnEditCancel();

        }
    }
    
    void UpdatePixelRect()
    {
        SpriteRenderer rndr = ((SpriteRenderer)target);
        //Vector3 p = PivotToLocalPosition(WorldPositionToPivot(_CapPos, true));
        Vector3 p = rndr.zPivotToLocalPosition(rndr.zWorldPositionToPivot(_CapPos,true));
        float hu = 0.475f / rndr.sprite.pixelsPerUnit;
        Transform tr = rndr.transform;
        _PixelRect[0] = tr.TransformPoint(p - new Vector3(-hu, -hu, 0f));
        _PixelRect[1] = tr.TransformPoint(p - new Vector3(hu, -hu, 0f));
        _PixelRect[2] = tr.TransformPoint(p - new Vector3(hu, hu, 0f));
        _PixelRect[3] = tr.TransformPoint(p - new Vector3(-hu, hu, 0f));
    }

    void UpdateEditRect()
    {
        SpriteRenderer rndr = ((SpriteRenderer)target);
        _EditRect[0] = rndr.zPivotToWorldPosition(new Vector2(0.0f, 0.0f));
        _EditRect[1] = rndr.zPivotToWorldPosition(new Vector2(1.0f, 0.0f));
        _EditRect[2] = rndr.zPivotToWorldPosition(new Vector2(1.0f, 1.0f));
        _EditRect[3] = rndr.zPivotToWorldPosition(new Vector2(0.0f, 1.0f));
    }

    //[DrawGizmo(GizmoType.Selected)]
    //static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
    //{
    //}

    
    public void OnSceneGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.keyDown && e.keyCode == KeyCode.A) _SnapDown = true;
        else if (e.type == EventType.keyUp && e.keyCode == KeyCode.A) _SnapDown = false;
        
        if (_OnEdit)
        {
            float screenSpaceSize = 0.25f / SceneView.currentDrawingSceneView.camera.orthographicSize;
            zCommonEditor.DrawDottedRect(_EditRect, screenSpaceSize, Color.yellow);

            ////// renders pivot-gizmo
            SpriteRenderer rndr = target as SpriteRenderer;
            _CapPos = Handles.FreeMoveHandle(
                        _CapPos,
                        Quaternion.identity,
                        0.1f * SceneView.currentDrawingSceneView.camera.orthographicSize,
                        Vector3.zero,
                        zCommonEditor.OutlinedCircleCap
                    );

            if (GUI.changed && _PixelAlign)
                _CapPos = rndr.zPivotToWorldPosition(rndr.zWorldPositionToPivot(_CapPos, true));

            if (_LockPosition && rndr.transform.position != _PrevPos)
                rndr.transform.position = _PrevPos;
        }

        if ((_AlignParentPixel || _SnapDown) && (!(_LockPosition && _OnEdit)))
        {
            (target as SpriteRenderer).transform.zSnapToParentPixelCenter();
        }
    }
}
