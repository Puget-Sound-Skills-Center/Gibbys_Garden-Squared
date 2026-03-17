using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TutorialScript : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public TextMeshProUGUI Tutorial;
    public int step = 1;
    public GameObject blocker1;
    public GameObject blocker2;

    public GameObject Exit;

    // Initiate on startup
    public void Awake()
    {
        UpdateStep();
    }

    public void UpdateStep ()
    {
        if (step == 1)
        {
            Tutorial.text = $"({step}/5) 'Head over to Gibby and talk to him'";
        }
        if (step == 2)
        {
            Tutorial.text = $"({step}/5) 'Head over to the desk with a pot and harvest it'";
        }
        if (step == 3)
        {
            Tutorial.text = $"({step}/5) 'Report back to Gibby'";
        }
        if (step == 4) // Unlocks the rest of the map
        {
            Destroy(blocker1);
            Destroy(blocker2);
            Tutorial.text = $"({step}/5) 'Collect another purple flower'";
        }
        if (step == 5)
        {
            Tutorial.text = $"({step}/5) 'Collect the rest of the flowers then talk to Gibby'";
            PlayerMovement.TutorialComplete = true;
        }
        if (step == 6)
        {
            Instantiate(Exit, new Vector3(-50.08f, 1.811094f, 55.01f), Quaternion.identity);
            Tutorial.text = $"({step}/5) Leave.";
        }
    }
}
