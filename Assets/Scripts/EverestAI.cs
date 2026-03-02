using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EverestAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public AI_PatrolPoints AI_PatrolPoints;
    public FlowerCollecting FlowerCollecting;

    private bool TimerActive;
    private float countdown;

    // Main Mechanics
    public float AllergyMeter = 0; // Goes from 0% to 100%, affected by AllergyFactor
    public float AllergyFactor = 0; // Proportional to how many flowers the player have, maxes out at 7% per second
    public bool IceCooldown = false;
    public float AbilityTimer = 0; // Cooldown (in seconds) between frost ability
    public float Buffer = 0; // Makes it so it fires event every second or less
    public bool PlayerInRange = false;




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EverestRelocate();
        PlayerInRange = false;
        AllergyFactor = 0;
        AllergyMeter = 0;
}

    private void Update()
    {
       // AI WANDERING SECTION -----------
       if (TimerActive == true) 
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f) // Moves to another spot after timer is up
            {
                TimerActive = false;
                EverestRelocate();
            }

        }

        // MAIN MECHANIC --------------
        AllergyFactor = FlowerCollecting.PottedFlower; // Increase meter percentage depending on how many purple flowers were collected

        if (PlayerInRange && IceCooldown == false)
        {
            Buffer += Time.unscaledDeltaTime;
            if(Buffer >= 0.25f)
            {
                AllergyMeter += AllergyFactor;
                if (AllergyMeter >= 100)
                {
                    Debug.Log("Everest used frozen ability!");
                    AllergyMeter = 100;
                    IceCooldown = true;
                }
                Buffer = 0f;
            }
        } else if (PlayerInRange == false && IceCooldown == false)
        {
            Buffer += Time.unscaledDeltaTime;
            if (Buffer >= 1f)
            {
                AllergyMeter -= 1;
                if (AllergyMeter <= 0)
                {
                    AllergyMeter = 0;
                }
                Buffer = 0f;
            }
        }


    }

    private void OnTriggerEnter(Collider other) // Player gets near Everest
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }

    public void EverestRelocate()
    {
        var t = AI_PatrolPoints.GetRandomPoint(); // Assigns patrol point
        if (t == null || agent == null) // Safety stuff
        {
            return;
        }

        //Debug.Log($"Everest moving to {t.position}");
        agent.SetDestination(t.position); // Moves to chosen Patrol Point
        countdown = Random.Range(15, 20);
        TimerActive = true;
       
    }
}
