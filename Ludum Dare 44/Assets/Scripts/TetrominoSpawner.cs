using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    private static TetrominoSpawner _instance;

    [SerializeField]
    private Tetromino TetrominoPrefab = null;

    private void Awake()
    {
        _instance = this;
    }

    public static Tetromino Spawn()
    {
        return _instance.Spawn(_instance.transform.position);
    }

    private Tetromino Spawn(Vector2 position)
    {
        //TODO Random rotation?
        var tetromino = Instantiate(TetrominoPrefab, new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)), Quaternion.identity);
        tetromino.SetTetrominoType((TetrominoType)Random.Range(0, 7));

        return tetromino;
    }

    private void Start()
    {
        //var tetromino = Spawn(Vector2.zero);
        //tetromino.SetStatic();

        //Destroy(tetromino.gameObject);

        Spawn();
    }
}
