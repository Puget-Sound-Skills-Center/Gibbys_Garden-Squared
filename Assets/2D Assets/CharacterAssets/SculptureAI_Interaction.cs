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

    public Sprite Active;
    public Sprite Inactive;


    private SpriteRenderer sr;

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
            StruggleProgress += 10;
            NotifText.text = "Constricted! Mash [Space] to break free! [" + StruggleProgress.ToString() + "%]";
            if (StruggleProgress == 100) // Player Has broken Free!!
            {
                Debug.Log("PLayer is set free");
                countdown = 10f;
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
            Debug.Log("Player Is Hugged!");
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
}
