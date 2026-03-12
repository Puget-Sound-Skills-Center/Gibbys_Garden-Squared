using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableDialogue : MonoBehaviour
{
    [Header("TextBoxes")]
    public TextMeshProUGUI interactionBar;

    public DialogueManagerStuff dialogueManagerStuff;
    public TutorialScript TutorialScript;
    public FlowerCollecting FlowerCollecting;

    [SerializeField] private string Character;
    private bool PlayerInRange = false;

    private void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(KeyCode.E)) {
            if (Character == "Gibby")
            {

                // Check for win conditions post step 4
                if (TutorialScript.step == 5)
                {
                    if (FlowerCollecting.Flower == 104 && FlowerCollecting.PottedFlower == 7) // Both quotas met
                    {
                        dialogueManagerStuff.CharacterTalk("GibbyHappy", "Yay! you're free to go now");
                        TutorialScript.step++;
                        TutorialScript.UpdateStep();
                    }
                    else if (FlowerCollecting.Flower == 104 && FlowerCollecting.PottedFlower != 7) // Only met Flower quota
                    {
                        dialogueManagerStuff.CharacterTalk("Gibby", "Not quite, you've still yet to collect all the Purple flowers!");
                    }
                    else if (FlowerCollecting.Flower != 104 && FlowerCollecting.PottedFlower == 7) // Only met PottedFlower quota
                    {
                        dialogueManagerStuff.CharacterTalk("Gibby", "Oh- you should collect the smaller flowers in the hallways!");
                    }
                    else if (FlowerCollecting.Flower != 104 && FlowerCollecting.PottedFlower != 7) // Didn't meet either
                    {
                        dialogueManagerStuff.CharacterTalk("Gibby", "Silly, come back when you've gotten all the flowers!");
                    }
                }

                // Tutorial Step thingys
                if (TutorialScript.step == 3 && FlowerCollecting.PottedFlower == 1)
                {
                    dialogueManagerStuff.CharacterTalk("GibbyHappy", "Lovely! There should be more scattered around, grab the lesser ones while you're at it!");
                    TutorialScript.step++;
                    TutorialScript.UpdateStep();
                }
                if (TutorialScript.step == 1 && FlowerCollecting.PottedFlower == 0)
                {
                    dialogueManagerStuff.CharacterTalk("Gibby", "See those pots over there? Water and collect them for me, pretty please :3");
                    TutorialScript.step++;
                    TutorialScript.UpdateStep();
                } else if (TutorialScript.step == 1 && FlowerCollecting.PottedFlower == 1)
                {
                    TutorialScript.step = 3;
                    TutorialScript.UpdateStep();
                }

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
