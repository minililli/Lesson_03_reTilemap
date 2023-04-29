using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

[CustomEditor(typeof(RoadTile_CustomRuleTile))]
public class RoadTileEditor : Editor
{
    RoadTile_CustomRuleTile roadTile;

    private void OnEnable()
    {
        roadTile = target as RoadTile_CustomRuleTile; // ������ ������ RoadTile_CustomRuleTile�̸� roadTile ������ �����ϱ�
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(roadTile != null && roadTile.sprite != null )
        {
            EditorGUILayout.LabelField("sprites Preview"); //�߰� ���� ("sprites Preview")
            GUILayout.BeginHorizontal();                    //�������� ��ġ����
            
            Texture2D texture;
            foreach (var sprite in roadTile.sprites)
            {
                texture = AssetPreview.GetAssetPreview(sprite); //��������Ʈ�� ������ �̹����� ������ texture�� ���
                GUILayout.Label("", GUILayout.Height(64),GUILayout.Width(64));  //64x64ũ��� ������ �׸���
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       //�̹��� �׸���(texture�� �׸��)
            }
            GUILayout.EndHorizontal();                      //�����ġ��(Begin ������ End�ؾ���)
        }
    }

}
#endif