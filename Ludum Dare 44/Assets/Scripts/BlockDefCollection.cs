using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDefCollection : MonoBehaviour
{
    private static BlockDef[] BlockDefs;

    private void Awake()
    {
        BlockDefs = Resources.LoadAll<BlockDef>("BlockDefs");
    }

    public static BlockDef GetRandomBlockDef()
    {
        return BlockDefs[Random.Range(0, BlockDefs.Length)];
    }
}
