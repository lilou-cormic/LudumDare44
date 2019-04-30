using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDefCollection : MonoBehaviour
{
    private static Dictionary<BlockType, BlockDef> BlockDefs;

    private void Awake()
    {
        BlockDefs = Resources.LoadAll<BlockDef>("BlockDefs").ToDictionary(x => x.BlockType, y => y);
    }

    public static BlockDef GetRandomBlockDef()
    {
        if (Random.Range(0, 100) > 95)
            return GetBlockDef(BlockType.Explosive);

        return GetBlockDef((BlockType)Random.Range(0, 3));
    }

    public static BlockDef GetBlockDef(BlockType blockType)
    {
        return BlockDefs[blockType];
    }
}
