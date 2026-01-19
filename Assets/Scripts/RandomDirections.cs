using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RandomDirections : MonoBehaviour
{
    public static RandomDirections Instance;

    public int south;
    public int east;
    public int north;
    public int west;

    public TMP_Text southText;
    public TMP_Text eastText;
    public TMP_Text northText;
    public TMP_Text westText;

    public KeyCode testKey = KeyCode.Space;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
        RegenerateDirections();
    }

    void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            RegenerateDirections();
        }
    }

    public int[] RegenerateDirections()
    {
        south = Random.Range(1, 5);
        east = Random.Range(1, 5);
        north = Random.Range(1, 5);
        west = Random.Range(1, 5);

        UpdateUI();

        return new int[]
        {
            north,south,east,west
        };
    }

    void UpdateUI()
    {
        if (southText)
        {
            southText.text = south.ToString();
        }
        if (eastText)
        {
            eastText.text = east.ToString();
        }
        if (northText)
        {
            northText.text = north.ToString();
        }
        if (westText)
        {
            westText.text = west.ToString();
        }
    }
}
