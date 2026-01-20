[System.Serializable]
public class Tile
{
    public TileType type;
    public bool revealed = false;
    public TileVisual visual;

    public int damageValue; // valor numérico del daño
}
