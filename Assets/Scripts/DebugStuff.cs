using UnityEngine;
using TMPro;

public class DebugStuff : MonoBehaviour
{

    [Header("NPC AI Scripts")]
    public EverestAI EverestAI;
    public MillyAI MillyAI;
    public KelvinAI KelvinAI;
    public SculptureAI_Interaction SculptureAI;

    [Header("Texts for each NPC")]
    public TextMeshProUGUI Everest;
    public TextMeshProUGUI Milly;
    public TextMeshProUGUI Kelvin;
    public TextMeshProUGUI Sculpture;

    private void Update()
    {
        // Sculpture Info stuff
        if(SculptureAI.SculptureIsActive)
        {
            Sculpture.text = $"Sculpture is Active!";
        } else
        {
            Sculpture.text = $"Sculpture is Inactive for {SculptureAI.countdown} seconds";
        }

        // Everest Info stuff
        Everest.text = $"Everest - AbilityMeter: {EverestAI.AllergyMeter}% / AbilityFactor: {EverestAI.AllergyFactor}% per second / Player in range: {EverestAI.PlayerInRange} / Buffer: {EverestAI.Buffer}";

        // Milly Info stuff
        Milly.text = $"Milly - Behavior: {MillyAI.Behavior} / Player in range: {MillyAI.PlayerInVicinity} / Player in Line of Sight: {MillyAI.PlayerInLineOfSight}";

        // Kelvin Info stuff
        Kelvin.text = $"Kelvin - Player in range: {KelvinAI.PlayerInVicinity} / Player in Line of Sight: {KelvinAI.PlayerInLineOfSight}";
    }

}
