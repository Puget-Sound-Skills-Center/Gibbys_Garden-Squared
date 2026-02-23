using UnityEngine;

public class DialogueManagerStuff : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    public void ShowDialogue()
    {
        group.alpha = 1f;
    }

    public void HideDialogue()
    {
        group.alpha = 0f;
    }

    private void Awake()
    {
        HideDialogue();
    }
}
