using UnityEngine;

public class TetrominoDef
{
    public TetrominoType TetrominoType;

    public BlockDef[] BlockDefs;

    public static Vector3[] GetPositions(TetrominoType tetrominoType)
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
}
