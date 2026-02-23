using System.Xml.Serialization;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]

public class FlowerPot_Collection : MonoBehaviour
{

    [Header("Flower Sprites")]
    public Sprite Sprout;
    public Sprite Stage1;
    public Sprite FullGrown;
    public Sprite Harvested;

    [Header("TextBoxes")]
    public TextMeshProUGUI interactionBar;

    private decimal progress = 0m;
    private bool playerInRange = false;
    private bool CanBeCollected = false;
    private bool HasBeenCollected = false;
    private SpriteRenderer sr;

    public float fireRate = 0.05f;
    private float nextFireTime = 0f;

    public FlowerCollecting Script;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Sprout;
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && CanBeCollected == true)
        {
            //Debug.Log("Flower Collected!");
            Script.PotFlowerCollected();
            interactionBar.text = "";
            CanBeCollected = false;
            HasBeenCollected = true;
        }
        if (playerInRange && Input.GetKey(KeyCode.E) && CanBeCollected == false && Time.time >= nextFireTime)
        {

            progress += 5m;

            if (progress >= 100m)
            {
                progress = 100m;
                CanBeCollected = true;
            }

            if (HasBeenCollected == true)
            {
                interactionBar.text = "";
            } 
            else
            {
                if (progress == 100m)
                {
                    interactionBar.text = "[F] - Collect";
                }
                else
                {
                    interactionBar.text = "[E] - Water (" + progress.ToString() + "%)";
                }
            }

            nextFireTime = Time.time + fireRate;
        }

        // Sprite States 
        if (HasBeenCollected == false)
        {
            if (progress >= 100)
            {
                sr.sprite = FullGrown;
            }
            else if (progress >= 50)
            {
                sr.sprite = Stage1;
            }
        } else
        {
            sr.sprite = Harvested;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (HasBeenCollected == true)
            {
                interactionBar.text = "";
            }
            else
            {
                if (progress == 100m)
                {
                    interactionBar.text = "[F] - Collect";
                }
                else
                {
                    interactionBar.text = "[E] - Water (" + progress.ToString() + "%)";
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionBar.text = "";
        }
    }
}
