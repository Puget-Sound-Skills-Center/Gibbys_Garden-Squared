
using UnityEngine;
using UnityEngine.AI;

public class SculptureAI : MonoBehaviour
{
    [SerializeField] Renderer boundingArea;
    [SerializeField] LayerMask ignoreOnCheck;

    NavMeshAgent agent;
    Transform player;

    public SculptureAI_Interaction SculptureAI_Interaction;
    public bool IsLookedAt = false;
    public float buffer = 0f;

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(CanMove())
        {
            buffer += Time.deltaTime;
            if (buffer > .1f)
            {
                SculptureAI_Interaction.PlayFootStep();
                buffer = 0;
            }
            agent.destination = player.position;
            IsLookedAt = false;
        } else
        {
            agent.destination = this.transform.position;
            IsLookedAt = true;
        }
    }

    private bool CanMove()
    {
        if(SculptureAI_Interaction.SculptureIsActive == true)
        {
            if (boundingArea.isVisible)
            {
                return false;
            }
            return true;
        } else
        {
            return false;
        }
    }
}
