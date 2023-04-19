using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[Flags]
public enum TileDirection : byte
{
    None = 0,
    North = 1,
    East = 2,
    West = 4,
    South = 8,
    All = North | East | West | South
}
public class RoadTile_CustomRuleTile : Tile
{
    Sprite[] sprites;


    private void OnEnable()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
    }
    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    /// <summary>
    /// 타일이 그려질 때 자동으로 호출되는 함수(갱신함수)
    /// </summary>
    /// <param name="position"> 9개의 타일 중 중심타일의 위치 </param>
    /// <param name="tilemap"> 타일맵의 종류 </param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3Int location = new(position.x + x, position.y + y, position.z);
                if (HasThisTile(tilemap, location))
                {
                    tilemap.RefreshTile(position);
                }
            }
        }
    }
    /// <summary>
    /// mask 주변 타일 정보 기록
    /// </summary>
    /// <param name="position"> 중심타일의 위치 </param>
    /// <param name="tilemap"> 타일맵의 종류 </param>
    /// <param name="tileData"> 타일의 종류 </param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        TileDirection mask = TileDirection.None;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? TileDirection.North : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? TileDirection.East : 0;

        int index = GetIndex(mask);
        if (index > -1)
        {
            tileData.sprite = sprites[index];
            tileData.color = Color.white;
            Matrix4x4 m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);             //tileData 회전하기
            tileData.transform = m;                                             //타일 회전시킨 후의 위치는 회전이전의 위치(m)으로 설정
            tileData.flags = TileFlags.LockTransform;                           //주변 타일 움직이지 않게 하기
            tileData.colliderType = ColliderType.None;                          //타일 충돌 타입은 None
        }
        else
        {
            Debug.LogError($"잘못된 index : {index}, mask : {mask}");
        }

    }

    private int GetIndex(TileDirection mask)
    {
        int index = -1;
        switch (mask)
        {
            case TileDirection.None:
            case TileDirection.North:
            case TileDirection.East:
            case TileDirection.South:
            case TileDirection.West:
            case TileDirection.North | TileDirection.South:
            case TileDirection.East | TileDirection.West:
                index = 0;
                break;

            case TileDirection.North | TileDirection.East:
            case TileDirection.North | TileDirection.West:
            case TileDirection.South | TileDirection.East:
            case TileDirection.South | TileDirection.West:

                index = 1;
                break;

            case TileDirection.North | TileDirection.South | TileDirection.West:
            case TileDirection.North | TileDirection.South | TileDirection.East:
            case TileDirection.East | TileDirection.West | TileDirection.North:
            case TileDirection.East | TileDirection.West | TileDirection.South:

                index = 2;
                break;

            case TileDirection.All:
                index = 3;
                break;
        }
        return index;
    }

    Quaternion GetRotation(TileDirection mask)
    {
        Quaternion rotate = Quaternion.identity;

        switch (mask)
        {
            case TileDirection.East:
            case TileDirection.West:
            case TileDirection.East | TileDirection.West:

            case TileDirection.South | TileDirection.East:

            case TileDirection.North | TileDirection.South | TileDirection.West:

                rotate = Quaternion.Euler(0, 0, -90.0f);
                break;

            case TileDirection.North | TileDirection.East:

            case TileDirection.East | TileDirection.West | TileDirection.South:
                rotate = Quaternion.Euler(0, 0, -180.0f);
                break;

            case TileDirection.North | TileDirection.West:

            case TileDirection.North | TileDirection.South | TileDirection.East:

                rotate = Quaternion.Euler(0, 0, -270.0f);
                break;
        }
        return rotate;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/create/2D/Tiles/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile",     //제목
                                                           "New Road Tile",      //파일의 기본 이름
                                                            "Asset",            //확장자
                                                            "Save RoadTile",    //출력되는 메세지
                                                            "Assets");          //열리는 기본 폴더
        if (path != string.Empty)
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile_CustomRuleTile>(), path); //RoadTile(Asset)을 path 위치에 생성
        }
    }
#endif
}


