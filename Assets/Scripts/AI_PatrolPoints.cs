using UnityEngine;

public class AI_PatrolPoints : MonoBehaviour
{

    // All patrol points in a list, for randomization
    [Header("Assign these in the Inspector")]
    public Transform[] Patrolpoints;

    private int RandomIndex;

    private void Awake()
    {
        if (Patrolpoints == null || Patrolpoints.Length == 0)
        {
            return;
        }
    }

    public Transform GetRandomPoint() {
        RandomIndex = Random.Range(0, Patrolpoints.Length);
        Debug.Log(RandomIndex);
        return Patrolpoints[RandomIndex];
    }
}
