using UnityEngine;

public class TileVisual : MonoBehaviour
{
    public Material hiddenMat;
    public Material emptyMat;
    public Material dmg1Mat;
    public Material dmg2Mat;
    public Material dmg3Mat;
    public Material dmg5Mat;
    public Material dmg8Mat;
    public Material victoryMat;
    public Material spawnMat;

    public MeshRenderer[] renderers;

    public void SetHidden()
    {
        foreach (var r in renderers)
            r.material = hiddenMat;
    }

    public void SetType(TileType type)
    {
        Material mat = emptyMat;

        switch (type)
        {
            case TileType.Empty: mat = emptyMat; break;
            case TileType.Damage1: mat = dmg1Mat; break;
            case TileType.Damage2: mat = dmg2Mat; break;
            case TileType.Damage3: mat = dmg3Mat; break;
            case TileType.Damage5: mat = dmg5Mat; break;
            case TileType.Damage8: mat = dmg8Mat; break;
            case TileType.Victory: mat = victoryMat; break;
            case TileType.Spawn: mat = spawnMat; break;
        }

        foreach (var r in renderers)
            r.material = mat;
    }
}
