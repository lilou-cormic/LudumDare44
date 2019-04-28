using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Block")]
public class BlockDef : ScriptableObject, IEqualityComparer<BlockDef>
{
    public BlockType BlockType;

    public Sprite Sprite;

    public override string ToString()
    {
        return BlockType.ToString();
    }

    public override bool Equals(object other)
    {
        return Equals(this, other as BlockDef);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public bool Equals(BlockDef x, BlockDef y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(null, x))
            return false;

        if (ReferenceEquals(null, y))
            return false;

        return x.BlockType == y.BlockType;
    }

    public int GetHashCode(BlockDef obj)
    {
        if (ReferenceEquals(null, obj))
            return -1;

        return (int)obj.BlockType;
    }
}
