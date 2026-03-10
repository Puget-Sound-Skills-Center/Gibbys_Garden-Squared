using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SingleDoorOpen_FORNPCS : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite DoorClosed;
    public Sprite DoorOpen;


    [Header("Filter")]
    public string NPCTag = "NPC";

    public AudioSource Source;
    public AudioClip OpenSound;
    public AudioClip CloseSound;


    private SpriteRenderer sr;
    private int touchingCount;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (DoorClosed != null) sr.sprite = DoorClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(NPCTag) && !other.CompareTag(NPCTag))
            return;


        touchingCount++;
        if (DoorOpen != null)
        {
            sr.sprite = DoorOpen;
            Source.PlayOneShot(OpenSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(NPCTag) && !other.CompareTag(NPCTag))
            return;


        touchingCount = Mathf.Max(0, touchingCount - 1);
        if (touchingCount == 0 && DoorClosed != null)
        {
            Source.PlayOneShot(CloseSound);
            sr.sprite = DoorClosed;
        }
    }

}
