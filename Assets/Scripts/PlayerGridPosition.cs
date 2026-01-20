using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    None,
    North,
    South,
    East,
    West
}

public class PlayerGridPosition : MonoBehaviour
{
    public Vector2Int gridPos = new Vector2Int(2, 2);
    public Direction direction;
    public void Move(Direction dir)
    {
        gridPos += DirectionToDelta(dir);
        transform.position = new Vector3(gridPos.x, transform.position.y, gridPos.y);
    }

    Vector2Int DirectionToDelta(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return new Vector2Int(0, 1);
            case Direction.South: return new Vector2Int(0, -1);
            case Direction.East: return new Vector2Int(1, 0);
            case Direction.West: return new Vector2Int(-1, 0);
            default: return Vector2Int.zero;
        }
    }
}

