using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MillyAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public AI_PatrolPoints AI_PatrolPoints;

    private bool TimerActive;
    private float countdown;

    public bool PlayerInLineOfSight = false;
    public bool PlayerInVicinity = false;
    public string Behavior = "Patrol";




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        MillyRelocate();
    }

    private void Update()
    {

        // Milly moving script ----------------------------

       if (TimerActive == true)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f) // Moves to another spot after timer is up
            {
                TimerActive = false;
                MillyRelocate();
            }

        }

       // Milly main AI Mechanics -------------------------


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

    
}
