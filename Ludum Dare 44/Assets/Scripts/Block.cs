using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer SpriteRenderer = null;

    public BlockDef BlockDef { get; private set; }

    public void SetBlockDef(BlockDef blockDef)
    {
        BlockDef = blockDef;
        SpriteRenderer.sprite = blockDef.Sprite;
    }
}

public enum BlockType
{
    Blue,
    Red,
    Coin,
}
