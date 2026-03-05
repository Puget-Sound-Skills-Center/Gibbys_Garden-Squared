using UnityEngine;

public class OfficeZoneScript : MonoBehaviour
{

    public PlayerMovement PlayerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement.IsInOffice = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement.IsInOffice = false;
        }
    }
}
