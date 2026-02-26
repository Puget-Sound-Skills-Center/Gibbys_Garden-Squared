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
    public float AbilityTimer = 0;
    private float Buffer = 0;
    public bool PlayerInRange = false;




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EverestRelocate();
        PlayerInRange = false;
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
        AllergyFactor = FlowerCollecting.PottedFlower;

    }

    private void OnCollisionEnter(Collision other) // Player gets near Everest
    {
        if(other.transform.tag == "Player")
        {
            PlayerInRange = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Player")
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
