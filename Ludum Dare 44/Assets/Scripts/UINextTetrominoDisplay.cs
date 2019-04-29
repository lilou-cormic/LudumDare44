using UnityEngine;
using UnityEngine.UI;

public class UINextTetrominoDisplay : MonoBehaviour
{
    [SerializeField]
    private Image[] Blocks = null;

    private void Awake()
    {
        TetrominoSpawner.NextTetrominoDefChanged += TetrominoSpawner_NextTetrominoDefChanged;
    }

    private void OnDestroy()
    {
        TetrominoSpawner.NextTetrominoDefChanged -= TetrominoSpawner_NextTetrominoDefChanged;
    }

    private void SetTetrominoDef(TetrominoDef tetrominoDef)
    {
        Vector3[] positions = TetrominoDef.GetPositions(tetrominoDef.TetrominoType);

        for (int i = 0; i < 4; i++)
        {
            Blocks[i].sprite = tetrominoDef.BlockDefs[i].Sprite;
            Blocks[i].transform.position = transform.position + positions[i];
        }
    }

    private void TetrominoSpawner_NextTetrominoDefChanged(TetrominoDef nextTetrominoDef)
    {
        SetTetrominoDef(nextTetrominoDef);
    }
}
