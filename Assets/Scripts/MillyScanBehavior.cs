using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

public class MillyScanBehavior : MonoBehaviour
{
    public MillyAI MillyAI;

    [Header("References")]
    public Transform player;

    [Header("Ray Settings")]
    public float maxDistance = 50f;
    public LayerMask hitmask = ~0;
    public bool requireLineOfSight = true;

    [Header("Debug")]
    public bool drawDebug = true;
    public Color debugColorHit = Color.green;
    public Color debugColorBlocked = Color.red;

    private void Update() // Raycast thingy
    {
        if (player == null) return;

        Vector3 origin = transform.position;
        Vector3 dir = (player.position - origin).normalized;

        if (drawDebug)
        {
            UnityEngine.Debug.DrawRay(origin, dir * maxDistance, Color.cyan);
        }

        if (Physics.Raycast(origin, dir, out RaycastHit hit, maxDistance, hitmask, QueryTriggerInteraction.Ignore))
        {
            bool isPlayer = hit.collider.transform == player || hit.collider.transform.IsChildOf(player);

            if (drawDebug)
            {
                UnityEngine.Debug.DrawLine(origin, hit.point, isPlayer ? debugColorHit : debugColorBlocked);
            }

            if (!requireLineOfSight || isPlayer) // Raycast hits player!
            {
                //UnityEngine.Debug.Log("Player in sight!");
                MillyAI.PlayerInLineOfSight = true;

            } else // Something is blocking the view from player!
            {
                // UnityEngine.Debug.Log("Player blocked by: " + hit.collider.name);
                MillyAI.PlayerInLineOfSight = false;
            }
        } else
        {
            if (drawDebug)
            {
                UnityEngine.Debug.DrawRay(origin, dir * maxDistance, debugColorBlocked);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MillyAI.PlayerInVicinity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MillyAI.PlayerInVicinity = false;
            MillyAI.PlayerInLineOfSight = false;
        }
    }
}
