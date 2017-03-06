using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.Collections;
using System;
using System.Linq;

/*
 * Editor Utilities
 * http://sunbug.net
 * 
 */

public static class zCommonEditor {

    public static void DrawSprite(Sprite spr, float x, float y, float scale = 1.0f)
    {
        if (spr == null) return;
        Texture t = spr.texture;
        Rect tr = spr.textureRect;
        Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
        Rect pos = new Rect(x, y, tr.width * scale, tr.height * scale);
        GUI.DrawTextureWithTexCoords(pos, t, r);
        //return pos;
    }

    public static void DrawRect(Rect rt, Color color)
    {
        Handles.color = color;
        Handles.DrawPolyLine
            (
                new Vector2(rt.x, rt.y),
                new Vector2(rt.xMax, rt.y),
                new Vector2(rt.xMax, rt.yMax),
                new Vector2(rt.x, rt.yMax),
                new Vector2(rt.x, rt.y)
            );
    }

    public static void DrawRect(Vector3[] p, Color color)
    {
        Handles.color = color;
        Handles.DrawLine(p[0], p[1]);
        Handles.DrawLine(p[1], p[2]);
        Handles.DrawLine(p[2], p[3]);
        Handles.DrawLine(p[3], p[0]);
    }

    public static void DrawDottedRect(Vector3[] p, float size, Color color)
    {
        Handles.color = color;
        Handles.DrawDottedLine(p[0], p[1], size);
        Handles.DrawDottedLine(p[1], p[2], size);
        Handles.DrawDottedLine(p[2], p[3], size);
        Handles.DrawDottedLine(p[3], p[0], size);
    }

    public static string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public static SpriteMetaData[] GetSpriteMetaData( string asset_path )
    {
        TextureImporter ti = AssetImporter.GetAtPath(asset_path) as TextureImporter;
        if (ti == null) return null;

        bool readable = ti.isReadable;
        ti.isReadable = true;

        SpriteMetaData[] ret = ti.spritesheet;
        ti.isReadable = false;

        if (ret == null) return null;
        if (ret.Length == 0) return null;

        return ret;
    }

    public static int GetSpriteAlignment( Sprite spr )
    {
        string path = AssetDatabase.GetAssetPath(spr.texture);
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

        bool saved = ti.isReadable;
        ti.isReadable = true;
        int align = 9;

        if (ti.spriteImportMode == SpriteImportMode.Multiple)
        {
            for (int i = 0; i < ti.spritesheet.Length; i++)
                if (spr.name.Equals(ti.spritesheet[i].name))
                {
                    align = ti.spritesheet[i].alignment;
                    break;
                }
        } else
        {
            TextureImporterSettings st = new TextureImporterSettings();
            ti.ReadTextureSettings(st);
            align = st.spriteAlignment;
        }

        ti.isReadable = saved;

        return align;
    }

    public static Vector2 GetSpritePivot( Sprite spr )
    {// return normalized sprite's pivot
        return new Vector2(spr.pivot.x / spr.textureRect.width, spr.pivot.y / spr.textureRect.height);
    }

    public static void SetSpritePivot(Sprite spr, Vector2 pivot, int align = (int)SpriteAlignment.Custom)
    {
        string path = AssetDatabase.GetAssetPath(spr.texture);
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

        bool saved = ti.isReadable;
        ti.isReadable = true;

        if (ti.spriteImportMode == SpriteImportMode.Multiple)
        {
            SpriteMetaData[] metas = ti.spritesheet;
            int i;
            for (i=0;i<metas.Length;i++)
                if (spr.name.Equals(metas[i].name))
                {
                    metas[i].pivot = pivot;
                    metas[i].alignment = align;
                    break;
                }
            ti.spritesheet = metas;
        } else
        {
            TextureImporterSettings st = new TextureImporterSettings();
            ti.ReadTextureSettings(st);
            st.spritePivot = pivot;
            st.spriteAlignment = align;
            ti.SetTextureSettings(st);
        }

        ti.isReadable = saved;
        AssetDatabase.ImportAsset(ti.assetPath, ImportAssetOptions.ForceUpdate);
    }

    [MenuItem("Assets/Adjust Pixel Pivot")]
    private static void AdjustPixelPivot()
    {
        if (Selection.activeObject == null) return;
        TextureImporter ti = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.activeObject))
            as TextureImporter;
        if (ti == null) return;

        bool readable = ti.isReadable;
        ti.isReadable = true;

        if (ti.spritesheet == null) { ti.isReadable = false; return; }
        if (ti.spritesheet.Length == 0) { ti.isReadable = false; return; }

        SpriteMetaData[] sheet = ti.spritesheet;

        for (int i = 0; i < sheet.Length;i++ )
        {
            SpriteMetaData dt = sheet[i];

            int pcx = (int)(dt.pivot.x * dt.rect.width);
            int pcy = (int)(dt.pivot.y * dt.rect.height);

            sheet[i].pivot = new Vector2( 
                ( ((float)pcx) + 0.5f ) / dt.rect.width,
                ( ((float)pcy) + 0.5f ) / dt.rect.height
                );
            sheet[i].alignment = 9;
        }

        ti.spritesheet = sheet;
        ti.isReadable = false;

        AssetDatabase.ImportAsset(ti.assetPath, ImportAssetOptions.ForceUpdate);

    }

    public static void OutlinedCircleCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
        Handles.color = Color.red;
        Camera cam = SceneView.currentDrawingSceneView.camera;

        Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.15f);
        Handles.DrawSolidArc(position,
            cam.transform.position
            - position,
            Vector3.right, 360.0f, size
            );

        Handles.color = Color.black;
        Handles.DrawSolidArc(position,
            cam.transform.position
            - position,
            Vector3.right, 360.0f, size*0.15f
            );

        if (GUIUtility.hotControl == controlID)
            Handles.color = Color.yellow;
        else
            Handles.color = Color.red;

        Handles.DrawSolidArc(position,
            cam.transform.position
            - position,
            Vector3.right, 360.0f, size*0.15f*0.8f
            );
    }
}

