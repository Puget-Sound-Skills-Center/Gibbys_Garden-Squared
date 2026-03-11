using UnityEngine;
using TMPro;

public class SculptureAI_Interaction : MonoBehaviour
{

    public bool SculptureIsActive = true; // Sculpture be able to move (and attack) 
    public bool CanHugPlayer = true; // Sculpture be able to attack
    public bool IsHuggingPlayer = false;
    public int StruggleProgress = 0;

    public bool InactiveTimer = false;
    public float countdown = 0f;

    public AudioSource Source;
    public AudioClip Footstep;
    public AudioClip Caught;
    public AudioClip SculptureTalk;

    public Sprite Active;
    public Sprite Inactive;


    private SpriteRenderer sr;

    public DialogueManagerStuff dialogueManagerStuff;
    public FlowerCollecting FlowerCollecting;
    public PlayerMovement PlayerMovement;
    public SculptureAI SculptureAI;
    public TextMeshProUGUI NotifText;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        InactiveTimer = false;
    }

    private void Update()
    {
        if(InactiveTimer == true) // Countdown Timer
        {
            //Debug.Log("Cooldown Active");
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                CanHugPlayer = true;
                SculptureIsActive = true;
                InactiveTimer = false;
            }
        }




        if (SculptureIsActive) // Visuals for Sculptures being active
        {
            sr.sprite = Active;
        } else
        {
            sr.sprite = Inactive;
        }

        if (SculptureAI.IsLookedAt) // Makes it so Sculpture cant hug player when touched but looked at still
        {
            CanHugPlayer = false;
        } else
        {
            CanHugPlayer = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsHuggingPlayer) // Space to break free
        {
            //Debug.Log("Breaking Free");
            StruggleProgress += 20;
            NotifText.text = "Constricted! Mash [Space] to break free! [" + StruggleProgress.ToString() + "%]";
            if (StruggleProgress == 100) // Player Has broken Free!!
            {
                Source.pitch = Random.Range(0.8f, 1.2f);
                Source.PlayOneShot(SculptureTalk);
                dialogueManagerStuff.CharacterTalk("Sculpture", "What fun! we should do that again sometime! :D");
                //Debug.Log("PLayer is set free");
                countdown = 5f; // in seconds
                NotifText.text = "";
                PlayerMovement.ThawPlayer();
                SculptureIsActive = false;
                CanHugPlayer = false;
                IsHuggingPlayer = false;
                InactiveTimer = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other) // Sculpture touches player
    {
        if (other.transform.tag == "Player" && CanHugPlayer)
        {
            //Debug.Log("Player Is Hugged!");
            Source.pitch = 1;
            Source.PlayOneShot(Caught);
            Source.PlayOneShot(SculptureTalk);
            dialogueManagerStuff.CharacterTalk("SculptureAlt", "Gotcha! >:3");
            IsHuggingPlayer = true;
            StruggleProgress = 0;
            CanHugPlayer = false;
            NotifText.text = "Constricted! Mash [Space] to break free! [" + StruggleProgress.ToString() + "%]";
            PlayerMovement.FreezePlayer();
        }
    }

    public void ActivateSculpture()
    {
        SculptureIsActive = true;
    }

    public void PlayFootStep()
    {
        Source.pitch = Random.Range(0.7f, 1.3f);
        Source.PlayOneShot(Footstep);
    }
}
