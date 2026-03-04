using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class DialogueManagerStuff : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    public TextMeshProUGUI DialogueText;
    [SerializeField] private Image IconImage;

    private bool TimerActive;
    private float countdown;

    // Icon Sprites (ADD MORE)
    public Sprite GibbyIcon;
    public Sprite SculptureIcon;
    public Sprite SculptureIcon_Alt;
    // Milly Sprites
    public Sprite MillyW1;
    public Sprite MillyW2;
    public Sprite MillyW3;

    private void Update()
    {
        if (TimerActive == true)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                TimerActive = false;
                ClearDialogue();
            }
            
        }
    }






    // Dialogue Functions
    public void CharacterTalk(string Who,string Dialogue)
    {
        // Managing who is speaking lol
        if (Who == "Gibby")
        {
            IconImage.sprite = GibbyIcon;
        }
        if (Who == "Sculpture")
        {
            IconImage.sprite = SculptureIcon;
        }
        if (Who == "SculptureAlt")
        {
            IconImage.sprite = SculptureIcon_Alt;
        }
        // Milly Stuff for organization
        if (Who == "MillyW1")
        {
            IconImage.sprite = MillyW1;
        }
        if (Who == "MillyW2")
        {
            IconImage.sprite = MillyW2;
        }
        if (Who == "MillyW3")
        {
            IconImage.sprite = MillyW3;
        }

        //Debug.Log($"{Who}, {Dialogue}");
        DialogueText.text = Dialogue;
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        group.alpha = 1f;
        countdown = 5f; // Seconds
        TimerActive = true;
    }

    public void ClearDialogue()
    {
        group.alpha = 0f;
    }

    // ----------------
    private void Awake()
    {
        ClearDialogue();
    }
}
