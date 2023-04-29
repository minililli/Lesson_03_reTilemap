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
        roadTile = target as RoadTile_CustomRuleTile; // 선택한 파일이 RoadTile_CustomRuleTile이면 roadTile 변수에 저장하기
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(roadTile != null && roadTile.sprite != null )
        {
            EditorGUILayout.LabelField("sprites Preview"); //중간 제목 ("sprites Preview")
            GUILayout.BeginHorizontal();                    //수평으로 배치시작
            
            Texture2D texture;
            foreach (var sprite in roadTile.sprites)
            {
                texture = AssetPreview.GetAssetPreview(sprite); //스프라이트의 프리뷰 이미지를 가져와 texture에 담기
                GUILayout.Label("", GUILayout.Height(64),GUILayout.Width(64));  //64x64크기로 프리뷰 그리기
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       //이미지 그리기(texture를 네모로)
            }
            GUILayout.EndHorizontal();                      //수평배치끝(Begin 했으면 End해야함)
        }
    }

}
#endif