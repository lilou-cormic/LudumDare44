using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour, IEqualityComparer<Block>
{
    private static int NextID = 1;

    //private Rigidbody2D _rb;
    //private Rigidbody2D rb
    //{
    //    get
    //    {
    //        if (_rb == null)
    //            _rb = GetComponent<Rigidbody2D>();

    //        return _rb;
    //    }
    //}

    //private Fall _fall;
    //private Fall fall
    //{
    //    get
    //    {
    //        if (_fall == null)
    //            _fall = GetComponent<Fall>();

    //        return _fall;
    //    }
    //}

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

    public void GetSurroundingNeighbors(HashSet<Block> blocks, float radius)
    {
        foreach (var block in Physics2D.OverlapCircleAll(transform.position, radius).Select(x => x.transform?.GetComponent<Block>()).Where(x => x != null))
        {
            blocks.Add(block);
        }
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
        if (neighbor != null && ((int)BlockDef.BlockType >= 100 || neighbor.BlockDef.Equals(BlockDef)))
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

    public void SetToBeDestroyed()
    {
        SpriteRenderer.color = Color.gray;
    }

    //public void StartFalling()
    //{
    //    GetComponent<Fall>().StartFalling();
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (rb == null || rb.bodyType == RigidbodyType2D.Static)
    //        return;

    //    fall.SetStatic();

    //    //CheckMatch3();
    //}
}


public enum BlockType
{
    Blue = 0,
    Red = 1,
    Coin = 2,
    Bomb = 100,
    Explosive = 101,
}
