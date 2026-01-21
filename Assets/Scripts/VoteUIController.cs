using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteUIController : MonoBehaviour
{
    public GameObject votePanel;
    public Image[] voteImages; // Deben estar en orden: 0=Up, 1=Right, 2=Down, 3=Left

    public void ShowVoteUI(List<PlayerVoteInput> players)
    {
        votePanel.SetActive(true);

        // Reset
        foreach (var img in voteImages)
            img.gameObject.SetActive(false);

        // Activar solo las necesarias según el orden de la lista
        for (int i = 0; i < players.Count; i++)
        {
            voteImages[i].sprite = players[i].voteSprite;
            voteImages[i].gameObject.SetActive(true);
        }
    }

    public void HideVoteUI()
    {
        votePanel.SetActive(false);
    }
}
