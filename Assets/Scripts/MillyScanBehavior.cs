using UnityEngine;

public class MillyScanBehavior : MonoBehaviour
{
    public MillyAI MillyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MillyAI.PlayerInVicinity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MillyAI.PlayerInVicinity = false;
        }
    }
}
