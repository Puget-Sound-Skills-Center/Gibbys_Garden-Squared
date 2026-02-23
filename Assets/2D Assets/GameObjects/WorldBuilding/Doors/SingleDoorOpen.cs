using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SingleDoorOpen : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite DoorClosed;
    public Sprite DoorOpen;


    [Header("Filter")]
    public string playerTag = "Player";
    public string NPCTag = "NPC";


    private SpriteRenderer sr;
    private int touchingCount;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (DoorClosed != null) sr.sprite = DoorClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag))
            return;


        touchingCount++;
        if (DoorOpen != null) sr.sprite = DoorOpen;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag))
            return;


        touchingCount = Mathf.Max(0, touchingCount - 1);
        if (touchingCount == 0 && DoorClosed != null)
            sr.sprite = DoorClosed;
    }

}
