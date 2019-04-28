using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IEqualityComparer<Block>
{
    private static int NextID = 1;

    [SerializeField]
    private SpriteRenderer SpriteRenderer = null;

    public BlockDef BlockDef { get; private set; }

    public int ID { get; private set; }

    private void Awake()
    {
        ID = NextID++;
        name = "Block" + ID.ToString("0000");
    }

    public void SetBlockDef(BlockDef blockDef)
    {
        BlockDef = blockDef;
        SpriteRenderer.sprite = blockDef.Sprite;
    }

    public void GetMatchingNeighbors(HashSet<Block> blocks)
    {
        GetMatchingNeighbor(blocks, Vector3.right);
        GetMatchingNeighbor(blocks, Vector3.left);
        GetMatchingNeighbor(blocks, Vector3.up);
        GetMatchingNeighbor(blocks, Vector3.down);
    }

    private void GetMatchingNeighbor(HashSet<Block> blocks, Vector3 direction)
    {
        var neighbor = GetNeighbor(direction).transform?.GetComponent<Block>();
        if (neighbor != null && neighbor.BlockDef.Equals(BlockDef))
        {
            if (blocks.Add(neighbor))
                neighbor.GetMatchingNeighbors(blocks);
        }
    }

    public RaycastHit2D GetNeighbor(Vector3 direction)
    {
        return Physics2D.Raycast(transform.position + (direction * 0.6f), direction, 0.2f);
    }

    public override bool Equals(object other)
    {
        return Equals(this, other as Block);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public bool Equals(Block x, Block y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(null, x))
            return false;

        if (ReferenceEquals(null, y))
            return false;

        return x.ID == y.ID;
    }

    public int GetHashCode(Block obj)
    {
        if (ReferenceEquals(null, obj))
            return -1;

        return obj.ID;
    }
}


public enum BlockType
{
    Blue,
    Red,
    Coin,
}
