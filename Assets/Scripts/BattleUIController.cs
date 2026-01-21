using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    [Header("Battle Panel")]
    public GameObject battlePanel;
    public Image player1Image;
    public Image player2Image;
    public Sprite deafult;

    [Header("Intro Animation")]
    public GameObject introObject;      // objeto que se moverá hacia abajo
    public Image introImage1;           // imagen 1 que hará fade in
    public Image introImage2;           // imagen 2 que hará fade in
    public float moveDistance = 100f;   // cuánto baja
    public float fadeDuration = 0.5f;   // tiempo del fade
    public float moveDuration = 0.2f;   // tiempo del movimiento

    // Estados internos
    private bool animatingIntro = false;
    private float animTimer = 0f;

    private Vector3 introStartPos;
    private Vector3 introEndPos;

    public AudioClip battleClip;

    // -------------------------------
    // FUNCIÓN 1: Mostrar batalla
    // -------------------------------
    public void ShowBattle(List<PlayerBattleInput> players)
    {
        if (players == null || players.Count < 2)
        {
            Debug.LogError("ShowBattle necesita exactamente 2 PlayerBattleInput");
            return;
        }

        battlePanel.SetActive(true);
        AudioManager.Instance.PlaySFX(battleClip);

        player1Image.sprite = players[0].battleSprite;
        player2Image.sprite = players[1].battleSprite;
    }

    // -------------------------------
    // FUNCIÓN 2: Animación de entrada (SIN CORUTINAS)
    // -------------------------------
    public void AnimateIntro(List<PlayerBattleInput> players)
    {
        if (players == null || players.Count < 2)
        {
            Debug.LogError("AnimateIntro necesita exactamente 2 PlayerBattleInput");
            return;
        }

        introObject.SetActive(true);

        PlayerCardImage pCI1 = introImage1.gameObject.GetComponent<PlayerCardImage>();
        PlayerCardImage pCI2 = introImage2.gameObject.GetComponent<PlayerCardImage>();

        switch (players[0].chosenBattle)

        {
            case 's':
                pCI1.SetTijeras();
                break;
            case 'r':
                pCI1.SetPiedra();
                break;
            case 'p':
                pCI1.SetPapel();
                break;
        }
        switch (players[1].chosenBattle)

        {
            case 's':
                pCI2.SetTijeras();
                break;
            case 'r':
                pCI2.SetPiedra();
                break;
            case 'p':
                pCI2.SetPapel();
                break;
        }

        // Reset alpha
        SetAlpha(introImage1, 0f);
        SetAlpha(introImage2, 0f);

        // Preparar movimiento
        introStartPos = introObject.transform.localPosition;
        introEndPos = introStartPos + new Vector3(0f, -moveDistance, 0f);

        animTimer = 0f;
        animatingIntro = true;
    }

    // -------------------------------
    // UPDATE: controla la animación de intro
    // -------------------------------
    private void Update()
    {
        if (!animatingIntro)
            return;

        animTimer += Time.deltaTime;

        float tMove = Mathf.Clamp01(animTimer / moveDuration);
        float tFade = Mathf.Clamp01(animTimer / fadeDuration);

        // Movimiento hacia abajo
        introObject.transform.localPosition = Vector3.Lerp(introStartPos, introEndPos, tMove);

        // Fade in imágenes
        float alpha = Mathf.Lerp(0f, 1f, tFade);
        SetAlpha(introImage1, alpha);
        SetAlpha(introImage2, alpha);

        // Fin de animación
        if (tMove >= 1f && tFade >= 1f)
        {
            animatingIntro = false;
        }
    }

    // -------------------------------
    // FUNCIÓN 3: Animación inversa INSTANTÁNEA
    // -------------------------------
    public void AnimateOutroInstant()
    {
        // Colocar el objeto en su posición inicial
        introObject.transform.localPosition = introStartPos;

        // Alpha a 0 instantáneamente
        SetAlpha(introImage1, 0f);
        SetAlpha(introImage2, 0f);

        introImage1.sprite = deafult;
        introImage2.sprite = deafult;

        // Ocultar objeto
    }

    // -------------------------------
    // Helper para alpha
    // -------------------------------
    private void SetAlpha(Image img, float a)
    {
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    // -------------------------------
    // Ocultar panel si lo necesitas
    // -------------------------------
    public void HideBattle()
    {
        battlePanel.SetActive(false);
    }
}
