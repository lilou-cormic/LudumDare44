using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Tetromino : MonoBehaviour
{
    private static int NextID = 1;

    private Rigidbody2D rb;

    [SerializeField]
    private Block BlockPrefab = null;

    public Block[] Blocks { get; } = new Block[4];

    private float FallTimer = 0f;
    private float FallDelay = 1f;

    private static int StaticLayer = 9;

    private bool IsSpeedFalling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        name = "Tetromino" + NextID++.ToString("0000");
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        if (Input.GetButtonUp("Down"))
        {
            IsSpeedFalling = false;
            rb.velocity = Vector2.zero;

            transform.position = new Vector3(transform.position.x, Mathf.FloorToInt(transform.position.y));
        }

        if (IsSpeedFalling)
            return;

        if (FallTimer >= FallDelay)
        {
            FallTimer = 0f;

            rb.MovePosition((Vector2)transform.position + Vector2.down);
        }
        else
        {
            FallTimer += Time.deltaTime;

            if (Input.GetButtonDown("Right") && !Blocks.Any(x => x.GetNeighbor(Vector3.right).transform?.gameObject.layer == StaticLayer))
                rb.MovePosition((Vector2)transform.position + Vector2.right);

            if (Input.GetButtonDown("Left") && !Blocks.Any(x => x.GetNeighbor(Vector3.left).transform?.gameObject.layer == StaticLayer))
                rb.MovePosition((Vector2)transform.position + Vector2.left);

            if (Input.GetButton("Down"))
            {
                IsSpeedFalling = true;
                rb.velocity = Vector2.down * 20f;
            }

            if (Input.GetButtonDown("Up"))
            {
                rb.MoveRotation(rb.rotation -= 90);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        SetStatic();

        CheckMatch3();
    }

    public void SetTetrominoType(TetrominoType tetrominoType)
    {
        Vector3[] positions = GetPositions(tetrominoType);

        for (int i = 0; i < 4; i++)
        {
            var block = Instantiate(BlockPrefab, transform.position + positions[i], Quaternion.identity, transform);
            block.SetBlockDef(BlockDefCollection.GetRandomBlockDef());

            Blocks[i] = block;
        }
    }

    private static Vector3[] GetPositions(TetrominoType tetrominoType)
    {
        Vector3[] positions = new Vector3[4];

        #region switch (tetrominoType)
        switch (tetrominoType)
        {
            case TetrominoType.I:
                positions[0] = new Vector3(0, 1);
                positions[1] = new Vector3(0, 0);
                positions[2] = new Vector3(0, -1);
                positions[3] = new Vector3(0, -2);
                break;

            case TetrominoType.O:
                positions[0] = new Vector3(0, 0);
                positions[1] = new Vector3(1, 0);
                positions[2] = new Vector3(0, -1);
                positions[3] = new Vector3(1, -1);
                break;

            case TetrominoType.T:
                positions[0] = new Vector3(-1, 0);
                positions[1] = new Vector3(0, 0);
                positions[2] = new Vector3(1, 0);
                positions[3] = new Vector3(0, -1);
                break;

            case TetrominoType.J:
                positions[0] = new Vector3(0, 2);
                positions[1] = new Vector3(0, 1);
                positions[2] = new Vector3(0, 0);
                positions[3] = new Vector3(-1, 0);
                break;

            case TetrominoType.L:
                positions[0] = new Vector3(0, 2);
                positions[1] = new Vector3(0, 1);
                positions[2] = new Vector3(0, 0);
                positions[3] = new Vector3(1, 0);
                break;

            case TetrominoType.S:
                positions[0] = new Vector3(1, 0);
                positions[1] = new Vector3(0, 0);
                positions[2] = new Vector3(0, -1);
                positions[3] = new Vector3(-1, -1);
                break;

            case TetrominoType.Z:
                positions[0] = new Vector3(-1, 0);
                positions[1] = new Vector3(0, 0);
                positions[2] = new Vector3(0, -1);
                positions[3] = new Vector3(1, -1);
                break;

            default:
                throw new System.InvalidOperationException($"{tetrominoType} is not valid for {nameof(Tetromino)}.{nameof(GetPositions)}");
        }
        #endregion

        return positions;
    }

    public void SetStatic()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        rb.velocity = Vector2.zero;

        transform.position = new Vector3(transform.position.x, Mathf.RoundToInt(transform.position.y));

        rb.bodyType = RigidbodyType2D.Static;
        IsSpeedFalling = false;
        gameObject.layer = StaticLayer;
        foreach (var block in Blocks)
        {
            block.gameObject.layer = StaticLayer;
        }

        if (transform.position.y > 12)
        {
            SceneManager.LoadScene("GameOver");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (Blocks[i] == null)
                continue;

            Blocks[i].gameObject.transform.SetParent(null);
        }

        TetrominoSpawner.Spawn();
    }

    private void CheckMatch3()
    {
        HashSet<Block>[] dd = new HashSet<Block>[4];

        for (int i = 0; i < 4; i++)
        {
            if (Blocks[i] == null)
                continue;

            dd[i] = new HashSet<Block>() { Blocks[i] };

            Blocks[i].GetMatchingNeighbors(dd[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            var ss = dd[i];

            //Debug.Log(Blocks[i].name + ": " + string.Join(", ", ss.Select(x => x.ID)));

            for (int j = i + 1; j < 4; j++)
            {
                if (dd[j].Any(x => ss.Contains(x)))
                {
                    foreach (var item in dd[j])
                    {
                        ss.Add(item);
                    }

                    dd[j].Clear();
                }
            }

            if (ss.Count >= 3)
            {
                foreach (var block in ss)
                {
                    block.gameObject.transform.SetParent(null);
                    Destroy(block.gameObject);
                }
            }
        }

        Destroy(gameObject);
    }
}

public enum TetrominoType
{
    #region Values
    /// <summary>
    /// #
    /// #
    /// #
    /// #
    /// </summary>
    I,

    /// <summary>
    /// ##
    /// ##
    /// </summary>
    O,

    /// <summary>
    /// ###
    ///  #
    /// </summary>
    T,

    /// <summary>
    ///  #
    ///  #
    /// ##
    /// </summary>
    J,

    /// <summary>
    /// #
    /// #
    /// ##
    /// </summary>
    L,

    /// <summary>
    ///  ##
    /// ##
    /// </summary>
    S,

    /// <summary>
    /// ##
    ///  ##
    /// </summary>
    Z,
    #endregion
}
