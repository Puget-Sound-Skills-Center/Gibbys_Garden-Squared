using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class FlowerCollecting : MonoBehaviour
{

    public int Flower = 0;
    private int FlowerQuota = 104;

    public int PottedFlower = 0;
    private int PottedFlowerQuota = 7;

    public AudioSource Speaker;
    public AudioClip CollectSFX;
    public AudioClip ScissorCut;


    public TextMeshProUGUI FlowerText;
    public TextMeshProUGUI PottedFlowerText;

    public SculptureAI_Interaction SculptureAI_Interaction;
    public TutorialScript TutorialScript;

    void Start()
    {
        FlowerText.text = Flower.ToString() + "/" + FlowerQuota.ToString();
    }

    private void Update()
    {
        PottedFlowerText.text = PottedFlower.ToString() + "/" + PottedFlowerQuota.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Flower")
        {

            Speaker.pitch = Random.Range(0.95f, 1.05f);
            Speaker.PlayOneShot(CollectSFX);


            Flower++;
            FlowerText.text = Flower.ToString() + "/" + FlowerQuota.ToString();
            //Debug.Log(Flower);
            Destroy(other.gameObject);
        }

        if (Flower == FlowerQuota)
        {
            FlowerText.color = Color.green;
        }
    }

    public void PotFlowerCollected()
    {
        PottedFlower++;
        Speaker.PlayOneShot(ScissorCut);
        if (PottedFlower == 1 && TutorialScript.step == 2) // Advances tutorial
        {
            TutorialScript.step++;
            TutorialScript.UpdateStep();
        }
        if (PottedFlower == 2) // Activate Sculpture on first flower collect
        {
            SculptureAI_Interaction.ActivateSculpture();
            TutorialScript.step++;
            TutorialScript.UpdateStep();
        }

        if (PottedFlower == PottedFlowerQuota)
        {
            PottedFlowerText.color = Color.green;
        }
    }

}
