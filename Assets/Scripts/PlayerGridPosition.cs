using UnityEngine;

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
    public Vector2Int gridPos = new Vector2Int(2, 2); // center for 5x5 (0..4)

    // Call this to move 1 tile in the chosen direction
    public void Move(Direction dir, int gridSize = 5)
    {
        Vector2Int delta = DirectionToDelta(dir);
        Vector2Int target = gridPos + delta;

        // board limits: 0..gridSize-1
        if (target.x < 0 || target.x >= gridSize || target.y < 0 || target.y >= gridSize)
        {
            Debug.Log($"{gameObject.name} tried to move out of bounds: {target}");
            return;
        }

        gridPos = target;
        Debug.Log($"{gameObject.name} moved to {gridPos}");

        // Optional: if you want the GameObject to move in world space too
        // transform.position = new Vector3(gridPos.x, gridPos.y, 0f);
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
