using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InteractableDialogue : MonoBehaviour
{
    [Header("TextBoxes")]
    public TextMeshProUGUI interactionBar;

    public DialogueManagerStuff dialogueManagerStuff;

    private bool PlayerInRange = false;




    private void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(KeyCode.E)) {
            dialogueManagerStuff.ShowDialogue();
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
