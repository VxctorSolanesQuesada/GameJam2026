using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerGridPosition : MonoBehaviour
{

    public Vector2Int gridPos = new Vector2Int(2, 2);
    public void Move(char c)
    {
        Vector2Int delta = DirectionToDelta(c);

        // Actualizar gridPos
        gridPos += delta;

        float tileSize = 15f;
        // Mover físicamente sumando el delta
        transform.position += new Vector3(delta.x * tileSize, 0, delta.y * tileSize);
    }


    Vector2Int DirectionToDelta(char c)
    {
        switch (c)
        {
            case 'n': return new Vector2Int(0, 1);
            case 's': return new Vector2Int(0, -1);
            case 'e': return new Vector2Int(1, 0);
            case 'w': return new Vector2Int(-1, 0);
            default: return Vector2Int.zero;
        }
    }
}

