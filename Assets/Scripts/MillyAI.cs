using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MillyAI : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public Transform Player;
    public DialogueManagerStuff DialogueManagerStuff;

    public NavMeshAgent agent;
    public AI_PatrolPoints AI_PatrolPoints;

    private bool TimerActive;
    private float countdown;

    public bool PlayerInLineOfSight = false;
    public bool PlayerInVicinity = false;

    // Main Mechanic Variables
    public string Behavior = "Patrol";
    public int PatrolSpeed = 15;
    public int DetainSpeed = 40;
    public float forgiveness = 0f; // Increases when not sprinting, default 30 seconds if not seen sprinting she takes away 1 warning from you
    public float WarnBuffer = 0f; // Increases when sprinting on her zone
    public float WarnCooldown = 0f; // Cooldown per warn




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (Behavior == "Patrol")
        {
            MillyRelocate();
        }
    }

    private void Update()
    {

        // Milly moving script ----------------------------

        if (Behavior == "Patrol")
        {
            agent.speed = PatrolSpeed;
            if (TimerActive == true)
            {
                countdown -= Time.deltaTime;
                if (countdown <= 0f) // Moves to another spot after timer is up
                {
                    TimerActive = false;
                    MillyRelocate();
                }

            }
        } else if (Behavior == "Detain")
        {
            TimerActive = false;
            agent.speed = DetainSpeed;
            agent.destination = Player.position;
        } else if (Behavior == "Stall")
        {
            transform.position = new Vector3(0f, 2.542645f, 6f);
            agent.destination = new Vector3(0f, 2.542645f, 6f);
            if (WarnCooldown < 2)
            {
                agent.destination = new Vector3(0f, 2.542645f, 6f);
                Behavior = "Patrol";
                MillyRelocate();
            }
        }

        // Milly main AI Mechanics -------------------------

        // Buffer for Warns 
        if (WarnCooldown > 0f)
        {
            WarnCooldown -= Time.deltaTime;
            if (WarnCooldown < 0f)
            {
                WarnCooldown = 0f;
            }
        }


       // checks if player is sprinting while in the line of sight of Milly
       if (PlayerInLineOfSight && WarnCooldown == 0 && Behavior == "Patrol")
        {
            if (PlayerMovement.InDetention && PlayerMovement.IsInOffice == false)
            {
                DialogueManagerStuff.CharacterTalk("MillyW3", "You should be in detention still!");
                Behavior = "Detain";
            }
            if (PlayerMovement.SprintMoving)
            {
                WarnBuffer += .01f;
                if (WarnBuffer > .5f && PlayerMovement.InDetention == false)
                {
                    WarnPlayer();
                }
            }
        } else
        {
            WarnBuffer -= .005f;
            if (WarnBuffer < 0f)
            {
                WarnBuffer = 0f;
            }
        }
    }

    public void MillyRelocate()
    {
        var t = AI_PatrolPoints.GetRandomPoint(); // Assigns patrol point
        if (t == null || agent == null) // Safety stuff
        {
            return;
        }

        //Debug.Log($"Milly moving to {t.position}");
        agent.SetDestination(t.position); // Moves to chosen Patrol Point
        countdown = Random.Range(8, 17);
        TimerActive = true;
       
    }

    public void WarnPlayer()
    {
        WarnCooldown = 1f;
        WarnBuffer = 0f;
        PlayerMovement.Warns++;
        // Stages of Warn
        if (PlayerMovement.Warns == 3)
        {
            DialogueManagerStuff.CharacterTalk("MillyW3", "For the last time, no running!");
            Behavior = "Detain";
            PlayerMovement.Warns = 0;
            WarnCooldown = 99f;
        }
        else if (PlayerMovement.Warns == 2)
        {
            DialogueManagerStuff.CharacterTalk("MillyW2", "You're going to detention if you keep running, final warning...");
        }
        else if (PlayerMovement.Warns == 1)
        {
            DialogueManagerStuff.CharacterTalk("MillyW1", "Hey slow down there! Don't want ya getting hurt >w>'");
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Behavior == "Detain")
        {
            PlayerMovement.InDetention = true;
            PlayerMovement.SendToDetention();
            DialogueManagerStuff.CharacterTalk("MillyW3", "You should know better...");
            Behavior = "Stall";
            WarnCooldown = 5f;
            transform.position = new Vector3(0f, 2.542645f, 6f);
            agent.destination = new Vector3(0f, 2.542645f, 6f);
        }
    }
}
