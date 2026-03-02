using UnityEngine;

public class KelvinScanBehavior : MonoBehaviour
{
    public KelvinAI KelvinAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KelvinAI.PlayerInVicinity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KelvinAI.PlayerInVicinity = false;
        }
    }
}
