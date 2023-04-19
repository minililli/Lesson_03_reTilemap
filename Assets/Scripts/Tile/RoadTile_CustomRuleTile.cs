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
    /// Ÿ���� �׷��� �� �ڵ����� ȣ��Ǵ� �Լ�(�����Լ�)
    /// </summary>
    /// <param name="position"> 9���� Ÿ�� �� �߽�Ÿ���� ��ġ </param>
    /// <param name="tilemap"> Ÿ�ϸ��� ���� </param>
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
    /// mask �ֺ� Ÿ�� ���� ���
    /// </summary>
    /// <param name="position"> �߽�Ÿ���� ��ġ </param>
    /// <param name="tilemap"> Ÿ�ϸ��� ���� </param>
    /// <param name="tileData"> Ÿ���� ���� </param>
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
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);             //tileData ȸ���ϱ�
            tileData.transform = m;                                             //Ÿ�� ȸ����Ų ���� ��ġ�� ȸ�������� ��ġ(m)���� ����
            tileData.flags = TileFlags.LockTransform;                           //�ֺ� Ÿ�� �������� �ʰ� �ϱ�
            tileData.colliderType = ColliderType.None;                          //Ÿ�� �浹 Ÿ���� None
        }
        else
        {
            Debug.LogError($"�߸��� index : {index}, mask : {mask}");
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
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile",     //����
                                                           "New Road Tile",      //������ �⺻ �̸�
                                                            "Asset",            //Ȯ����
                                                            "Save RoadTile",    //��µǴ� �޼���
                                                            "Assets");          //������ �⺻ ����
        if (path != string.Empty)
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile_CustomRuleTile>(), path); //RoadTile(Asset)�� path ��ġ�� ����
        }
    }
#endif
}


