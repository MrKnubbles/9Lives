using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public static class zUtils {

    public static bool IsPointerOverUIObject()
    {
        // Referencing this code for GraphicRaycaster 
        // https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position
            = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static Vector2 zNormlizedPivot( this Sprite spr )
    {
        return new Vector2(spr.pivot.x / spr.textureRect.width, spr.pivot.y / spr.textureRect.height);
    }

    public static Vector2 zWorldPositionToPivot( this SpriteRenderer rndr, Vector3 world_pos, bool pixel_align = false)
    {
        Sprite spr = rndr.sprite;
        if (spr == null) return Vector2.zero;
        Transform tr = rndr.transform;

        Vector3 lp = tr.InverseTransformPoint(world_pos);
        float pu = spr.pixelsPerUnit;
        float w = spr.textureRect.width;
        float h = spr.textureRect.height;

        Vector2 pivot = spr.zNormlizedPivot() + new Vector2(lp.x * pu / w, lp.y * pu / h);
        if (pixel_align)
        {
            int pcx = (int)(pivot.x * w);
            int pcy = (int)(pivot.y * h);
            float px = pivot.x < 0.0f ? ((float)pcx) - 0.5f : ((float)pcx) + 0.5f;
            float py = pivot.y < 0.0f ? ((float)pcy) - 0.5f : ((float)pcy) + 0.5f;
            pivot = new Vector2(px / w, py / h);
        }
        return pivot;
    }

    public static Vector3 zPivotToLocalPosition( this SpriteRenderer rndr, Vector2 pivot )
    {
        Sprite spr = rndr.sprite;
        if (spr == null) return Vector3.zero;
        Transform tr = rndr.transform;

        float pu = spr.pixelsPerUnit;
        float w = spr.textureRect.width;
        float h = spr.textureRect.height;

        Vector3 pos = Vector3.zero;
        pivot -= spr.zNormlizedPivot();
        pos.x = pivot.x * w / pu;
        pos.y = pivot.y * h / pu;
        return pos;
    }

    public static Vector3 zPivotToWorldPosition( this SpriteRenderer rndr, Vector2 pivot)
    {
        return rndr.transform.TransformPoint(rndr.zPivotToLocalPosition(pivot));
    }

    public static void zSnapToParentPixelCenter( this Transform tr, bool zkeep = true )
    {
        if (tr.parent == null) return;
        SpriteRenderer rndr = tr.parent.GetComponent<SpriteRenderer>();
        if (rndr == null) return;
        Sprite spr = rndr.sprite;
        if (spr == null) return;

        Vector3 snapped = rndr.zPivotToWorldPosition(
            rndr.zWorldPositionToPivot(tr.position, true)
            );
        if (zkeep) snapped.z = tr.position.z;

        tr.position = snapped;
    }
}
