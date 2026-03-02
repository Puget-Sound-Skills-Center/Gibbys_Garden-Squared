using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KelvinAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public AI_PatrolPoints AI_PatrolPoints;

    private bool TimerActive;
    private float countdown;

    // Main Mechanic Variables
    public bool PlayerInLineOfSight = false;
    public bool PlayerInVicinity = false;

    public bool OnCooldown = false; // Whether Kelvin can shoot his Ice raygun
    public float CooldownTimer = 0f; // In seconds until Kelvin can shoot again





    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        KelvinRelocate();
    }

    private void Update()
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
    }

    public void KelvinRelocate()
    {
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
}
