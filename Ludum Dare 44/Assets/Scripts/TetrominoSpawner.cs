using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    private static TetrominoSpawner _instance;

    [SerializeField]
    private Tetromino TetrominoPrefab = null;

    private TetrominoDef _NextTetrominoDef = null;
    private TetrominoDef NextTetrominoDef
    {
        get
        {
            // Fail-safe
            if (_NextTetrominoDef == null)
                PrepareNextTetromino();

            return _NextTetrominoDef;
        }

        set
        {
            _NextTetrominoDef = value;

            NextTetrominoDefChanged?.Invoke(_NextTetrominoDef);
        }
    }

    public static event System.Action<TetrominoDef> NextTetrominoDefChanged;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        PrepareNextTetromino();

        Spawn();
    }

    private void PrepareNextTetromino()
    {
        TetrominoDef nextTetrominoDef = new TetrominoDef()
        {
            TetrominoType = (TetrominoType)Random.Range(0, 7),
            BlockDefs = new BlockDef[]
            {
                BlockDefCollection.GetRandomBlockDef(),
                BlockDefCollection.GetRandomBlockDef(),
                BlockDefCollection.GetRandomBlockDef(),
                BlockDefCollection.GetRandomBlockDef(),
            },
        };

        NextTetrominoDef = nextTetrominoDef;
    }

    public static Tetromino Spawn()
    {
        return _instance.Spawn(_instance.transform.position);
    }

    private Tetromino Spawn(Vector2 position)
    {
        var tetromino = Instantiate(TetrominoPrefab, new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)), Quaternion.identity);
        tetromino.SetTetrominoDef(NextTetrominoDef);

        PrepareNextTetromino();

        return tetromino;
    }
}
