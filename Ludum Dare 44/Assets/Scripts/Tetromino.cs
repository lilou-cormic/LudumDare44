using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Fall))]
public class Tetromino : MonoBehaviour
{
    private static int NextID = 1;

    private Rigidbody2D rb;

    private Fall fall;

    [SerializeField]
    private Block BlockPrefab = null;

    [SerializeField]
    private UIPoints PointsDisplayPrefab = null;

    [SerializeField]
    private AudioClip MoveSound = null;

    [SerializeField]
    private AudioClip RotateSound = null;

    [SerializeField]
    private AudioClip LandSound = null;

    [SerializeField]
    private AudioClip MatchedSound = null;

    [SerializeField]
    private AudioClip CoinSound = null;

    [SerializeField]
    private AudioClip BombSound = null;

    [SerializeField]
    private AudioClip ExplosiveSound = null;

    [SerializeField]
    private AudioClip GameOverSound = null;

    public Block[] Blocks { get; } = new Block[4];

    private float FallTimer { get; set; } = 0f;
    public static float FallDelay { get; set; } = 1f;

    private static int StaticLayer = 9;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fall = GetComponent<Fall>();
        name = "Tetromino" + NextID++.ToString("0000");

        Health.ValueChanged += Health_ValueChanged;
    }

    private void OnDestroy()
    {
        Health.ValueChanged -= Health_ValueChanged;
    }

    private void Health_ValueChanged()
    {
        if (Health.Value <= 0)
            GameOver();
    }

    public void SetTetrominoDef(TetrominoDef tetrominoDef)
    {
        Vector3[] positions = TetrominoDef.GetPositions(tetrominoDef.TetrominoType);

        for (int i = 0; i < 4; i++)
        {
            var block = Instantiate(BlockPrefab, transform.position + positions[i], Quaternion.identity, transform);
            block.SetBlockDef(tetrominoDef.BlockDefs[i]);

            Blocks[i] = block;
        }
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        if (Input.GetButtonUp("Down"))
            fall.StopFalling();

        if (fall.IsFalling)
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
                Move(Vector2.right);

            if (Input.GetButtonDown("Left") && !Blocks.Any(x => x.GetNeighbor(Vector3.left).transform?.gameObject.layer == StaticLayer))
                Move(Vector2.left);

            if (Input.GetButton("Down"))
                fall.StartFalling();

            if (Input.GetButtonDown("Up"))
                Rotate();
        }
    }

    private void Move(Vector2 direction)
    {
        SoundPlayer.Play(MoveSound);

        rb.MovePosition((Vector2)transform.position + direction);
    }

    private void Rotate()
    {
        //TODO Check if it can be rotated

        SoundPlayer.Play(RotateSound);

        rb.rotation -= 90;

        foreach (var block in Blocks)
        {
            block.transform.rotation = Quaternion.Euler(0, 0, block.transform.rotation.eulerAngles.z + 90);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        SetStatic();

        StartCoroutine(CheckMatch3());
    }

    private void SetStatic()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        SoundPlayer.Play(LandSound);

        fall.SetStatic();

        foreach (var block in Blocks)
        {
            block.gameObject.layer = StaticLayer;
        }

        if (transform.position.y > 11)
        {
            GameOver();
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (Blocks[i] == null)
                continue;

            Blocks[i].gameObject.transform.SetParent(Board.Instance.transform);

            //Rigidbody2D newRB = Blocks[i].gameObject.AddComponent<Rigidbody2D>();
            //newRB.bodyType = RigidbodyType2D.Static;
            //newRB.useFullKinematicContacts = true;
            //newRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            //Fall newFall = Blocks[i].gameObject.AddComponent<Fall>();
            //newFall.FallSpeed = fall.FallSpeed * 2;
        }
    }

    private void GameOver()
    {
        SoundPlayer.Play(GameOverSound);

        ScoreManager.SetHighScore();
        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator CheckMatch3()
    {
        HashSet<Block>[] matchingBlocks = new HashSet<Block>[4];

        for (int i = 0; i < 4; i++)
        {
            Block block = Blocks[i];

            matchingBlocks[i] = new HashSet<Block>() { block };

            if (block.BlockDef.BlockType == BlockType.Bomb)
                block.GetSurroundingNeighbors(matchingBlocks[i], 2f);
            else
                block.GetMatchingNeighbors(matchingBlocks[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            var matchSet = matchingBlocks[i];

            for (int j = i + 1; j < 4; j++)
            {
                if (matchingBlocks[j].Any(x => matchSet.Contains(x)))
                {
                    foreach (var item in matchingBlocks[j])
                    {
                        matchSet.Add(item);
                    }

                    matchingBlocks[j].Clear();
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            var matchSet = matchingBlocks[i];

            if (matchSet.Count >= 3)
            {
                foreach (var block in matchSet)
                {
                    block.SetToBeDestroyed();
                }
            }
        }

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < 4; i++)
        {
            var matchSet = matchingBlocks[i];

            if (matchSet.Count >= 3)
            {
                if (matchSet.Any(x => x.BlockDef.BlockType == BlockType.Explosive))
                {
                    SoundPlayer.Play(ExplosiveSound);

                    var pointsDisplay = Instantiate(PointsDisplayPrefab, Blocks[i].transform.position, Quaternion.identity);

                    int points = ScoreManager.AddPoints(-matchSet.Count(x => x.BlockDef.BlockType == BlockType.Coin));

                    Health.RemoveHealth(points);
                    pointsDisplay.SetPointsText(points);
                }
                else if (matchSet.Any(x => x.BlockDef.BlockType == BlockType.Bomb))
                {
                    SoundPlayer.Play(BombSound);
                }
                else if (matchSet.Any(x => x.BlockDef.BlockType == BlockType.Coin))
                {
                    SoundPlayer.Play(CoinSound);

                    var pointsDisplay = Instantiate(PointsDisplayPrefab, Blocks[i].transform.position, Quaternion.identity);

                    int points = ScoreManager.AddPoints(matchSet.Count);

                    Health.AddHealth(points);
                    pointsDisplay.SetPointsText(points);
                }
                else
                {
                    SoundPlayer.Play(MatchedSound);
                }

                foreach (var block in matchSet)
                {
                    block.gameObject.transform.SetParent(null);
                    Destroy(block.gameObject);
                }
            }
        }

        //Board.Instance.MakeBlocksFall();

        ScoreManager.AddTetromino();

        if (ScoreManager.TetrominoCount % 15 == 0)
        {
            ScoreManager.AddLevel();
            FallDelay *= 0.6f;
        }

        TetrominoSpawner.Spawn();

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
