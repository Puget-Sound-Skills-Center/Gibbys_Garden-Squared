using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InteractableDialogue : MonoBehaviour
{
    [Header("TextBoxes")]
    public TextMeshProUGUI interactionBar;

    public DialogueManagerStuff dialogueManagerStuff;

    [SerializeField] private string Character;
    private bool PlayerInRange = false;

    private void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(KeyCode.E)) {
            if (Character == "Gibby")
            {
                dialogueManagerStuff.CharacterTalk("Gibby", "See those pots over there? Water and collect them for me, pretty please :3");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
            interactionBar.text = "[E] - Interact";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
            interactionBar.text = "";
        }
    }
}
