using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KelvinAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public AI_PatrolPoints AI_PatrolPoints;
    public PlayerMovement PlayerMovement;
    public DialogueManagerStuff DialogueManagerStuff;
    [SerializeField] private CanvasGroup SpotMechanic;
    public Image SpotImage;
    public Sprite Spotted;
    public Sprite Lost;

    public AudioSource Source;
    public AudioClip KelvinSpotted;
    public AudioClip KelvinLost;
    public AudioClip KelvinShot;
    public AudioClip KelvinRayGun;

    private bool TimerActive;
    private float countdown;

    // Main Mechanic Variables
    public int KelvinSpeed = 14;
    public string Mode = "Wandering";
    public bool ModechangeBuffer = false;
    public float ModeTimer = 0f;
    public bool PlayerInLineOfSight = false;
    public bool PlayerInVicinity = false;

    public bool Engaged = false;
    public bool Observing = false; // set to true to allow frost charge to go down, going to negatives causes kelvin to lose the player.
    public float FrostCharge = 0f; // Player has 3 seconds to get of Kelvin's sight
    public bool OnCooldown = false; // Whether Kelvin can shoot his Ice raygun
    public float CooldownTimer = 0f; // In seconds until Kelvin can shoot again





    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        KelvinRelocate();
    }

    private void Update()
    {

        // Update Kevin's speed 
        agent.speed = KelvinSpeed;
        // Kelvin Movement System
        if (Mode == "Wandering")
        {
            if (TimerActive == true)
            {
                countdown -= Time.deltaTime;
                if (countdown <= 0f) // Moves to another spot after timer is up
                {
                    TimerActive = false;
                    KelvinRelocate();
                }

            }
        } else if (Mode == "Engaged")
        {
            agent.destination = transform.position;
        } else if (Mode == "Fleeing")
        {

        }

        // Kelvin has sight of player

        if (PlayerInLineOfSight && OnCooldown == false && Mode == "Wandering")
        {
            Mode = "Engaged";
            agent.destination = transform.position;
            Engaged = true;
            FrostCharge = 1f;
            Source.PlayOneShot(KelvinSpotted);
        }

        Observing = PlayerInLineOfSight;

        // SpotMechanic Changing Images
        if(SpotMechanic.alpha == 1)
        {
            if(PlayerInLineOfSight)
            {
                SpotImage.sprite = Spotted;
            } else
            {
                SpotImage.sprite = Lost;
            }
        }

        // Kelvin Main Mechanic
        if (OnCooldown == false && Observing && Engaged == true && PlayerMovement.IsInOffice == false)
        {
            if (PlayerInLineOfSight)
            {
                FrostCharge += Time.deltaTime;
                if (FrostCharge < 1.05f)
                {
                    SpotMechanic.alpha = 1;
                    DialogueManagerStuff.CharacterTalk("KelvinSpotted", "Sights on you, human! Hold still!");
                }
                else if (FrostCharge > 3f)
                {
                    DialogueManagerStuff.CharacterTalk("KelvinSuccess", "Heheh! Must be cold for you, hah!");

                    Source.PlayOneShot(KelvinRayGun);
                    Source.PlayOneShot(KelvinShot);
                    SpotMechanic.alpha = 0;
                    // Chill player 
                    PlayerMovement.ChillPlayer();

                    Engaged = false;
                    OnCooldown = true;
                    CooldownTimer = 35f;
                    ModechangeBuffer = true;
                    ModeTimer = 0f;
                }
            }
        } else if (OnCooldown == false && Observing == false && Engaged == true)
        {

            FrostCharge -= Time.deltaTime;
            if (FrostCharge < 0)
                {
                    DialogueManagerStuff.CharacterTalk("KelvinBummed", "You're no fun.");
                    Source.PlayOneShot(KelvinLost);
                    SpotMechanic.alpha = 0;
                    Engaged = false;
                    OnCooldown = true;
                    CooldownTimer = 20f;
                    FrostCharge = 0f;
                    // Engage fleeing thing
                    ModechangeBuffer = true;
                    ModeTimer = 0f;
                } 
        }

        // Ability CoolDown Timer 
        if (CooldownTimer > 0)
        {
            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer <= 0)
            {
                CooldownTimer = 0f;
                OnCooldown = false;
            }
        }

        // Buffer before fleeing
        if (ModechangeBuffer == true)
        {
            ModeTimer += Time.deltaTime;
            if (ModeTimer > 2)
            {
                KelvinSpeed = 32;
                KelvinFlee();
                ModechangeBuffer = false;
                ModeTimer = 0f;
            }
        }
    }

    public void KelvinRelocate()
    {
        KelvinSpeed = 14;
        var t = AI_PatrolPoints.GetRandomPoint(); // Assigns patrol point
        if (t == null || agent == null) // Safety stuff
        {
            return;
        }

        //Debug.Log($"Milly moving to {t.position}");
        agent.SetDestination(t.position); // Moves to chosen Patrol Point
        countdown = Random.Range(15, 20);
        TimerActive = true;
    }


    public void KelvinFlee()
    {
        Mode = "Wandering";
        var t = AI_PatrolPoints.GetRandomPoint(); // Assigns patrol point
        if (t == null || agent == null) // Safety stuff
        {
            return;
        }
        agent.SetDestination(t.position);
        countdown = Random.Range(9, 12);
        TimerActive = true;
    }
}
