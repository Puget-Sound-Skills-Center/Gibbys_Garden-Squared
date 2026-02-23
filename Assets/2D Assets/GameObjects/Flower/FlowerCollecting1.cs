using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerCollecting : MonoBehaviour
{

    private int Flower = 0;
    private int FlowerQuota = 104;

    public int PottedFlower = 0;
    private int PottedFlowerQuota = 7;


    public TextMeshProUGUI FlowerText;
    public TextMeshProUGUI PottedFlowerText;

    public SculptureAI_Interaction SculptureAI_Interaction;

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

        if (PottedFlower == 1) // Activate Sculpture on first flower collect
        {
            SculptureAI_Interaction.ActivateSculpture();
        }

        if (PottedFlower == PottedFlowerQuota)
        {
            PottedFlowerText.color = Color.green;
        }
    }

}
