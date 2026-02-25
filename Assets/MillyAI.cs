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




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        MillyRelocate();
    }

    private void Update()
    {
       if (TimerActive == true)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f) // Moves to another spot after timer is up
            {
                TimerActive = false;
                MillyRelocate();
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

        Debug.Log($"Milly moving to {t.position}");
        agent.SetDestination(t.position); // Moves to chosen Patrol Point
        countdown = Random.Range(30, 40);
        TimerActive = true;
       
    }
}
