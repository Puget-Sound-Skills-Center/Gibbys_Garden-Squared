
using UnityEngine;
using UnityEngine.AI;

public class SculptureAI_AIFIX : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Renderer boundingArea;         // Renderer of the statue
    [SerializeField] Transform player;              // Player root transform
    [SerializeField] Camera playerCamera;           // Player's camera

    [Header("Masks & Distances")]
    [SerializeField] LayerMask occlusionMask = ~0;  // Include Environment, Default, etc. EXCLUDE this statue's layer
    [SerializeField] float maxViewDistance = 200f;  // How far the player can 'see'

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (!playerCamera) playerCamera = Camera.main;
        if (!boundingArea) boundingArea = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (!agent || !player || !playerCamera || !boundingArea) return;

        if (CanMove())
        {
            if (agent.isStopped) agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    private bool CanMove()
    {
        // If the player camera can't see us (offscreen / behind / occluded), we can move.
        return !IsVisibleToPlayerCamera();
    }

    private bool IsVisibleToPlayerCamera()
    {
        // Use the renderer bounds center as the target point
        Vector3 target = boundingArea.bounds.center;

        // 1) Must be in front of the camera and within screen bounds
        Vector3 toTarget = target - playerCamera.transform.position;

        // Behind camera ? not visible
        if (Vector3.Dot(playerCamera.transform.forward, toTarget.normalized) <= 0f)
            return false;

        // Too far ? not considered visible
        if (toTarget.sqrMagnitude > maxViewDistance * maxViewDistance)
            return false;

        // On screen?
        Vector3 vp = playerCamera.WorldToViewportPoint(target);
        bool onScreen = vp.z > 0f && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f;
        if (!onScreen) return false;

        // 2) Occlusion test: camera ? statue must hit statue first
        float dist = toTarget.magnitude;

        // IMPORTANT: occlusionMask must EXCLUDE this statue's layer to avoid self-hits
        if (Physics.Raycast(playerCamera.transform.position,
                            toTarget.normalized,
                            out RaycastHit hit,
                            dist + 0.5f,
                            occlusionMask,
                            QueryTriggerInteraction.Ignore))
        {
            // Compare roots to tolerate child colliders/rigidbodies
            Transform hitRoot = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.transform
                                                               : hit.collider.transform.root;
            return hitRoot == transform.root; // true => camera sees the statue
        }

        // Nothing blocked it, treat as visible
        return true;
    }
}

