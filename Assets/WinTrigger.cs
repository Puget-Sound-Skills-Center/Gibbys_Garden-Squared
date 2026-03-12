using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] CanvasGroup WinMenu;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            WinMenu.alpha = 1;
            Time.timeScale = 0;
        }
    }
}