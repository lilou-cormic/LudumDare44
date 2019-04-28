using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Block")]
public class BlockDef : ScriptableObject
{
    public BlockType BlockType;

    public Sprite Sprite;
}
