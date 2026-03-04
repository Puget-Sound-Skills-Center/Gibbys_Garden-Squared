using UnityEngine;
using TMPro;

public class DebugStuff : MonoBehaviour
{

    [Header("NPC AI Scripts")]
    public EverestAI EverestAI;
    public MillyAI MillyAI;
    public KelvinAI KelvinAI;
    public SculptureAI_Interaction SculptureAI;
    public PlayerMovement PlayerMovement;

    [Header("Texts for each NPC")]
    public TextMeshProUGUI Everest;
    public TextMeshProUGUI Milly;
    public TextMeshProUGUI Kelvin;
    public TextMeshProUGUI Sculpture;
    public TextMeshProUGUI Player;

    private void Update()
    {
        // Sculpture Info stuff ---------------------------------
        if (SculptureAI.SculptureIsActive)
        {
            Sculpture.text = $"Sculpture is Active!";
        } else
        {
            Sculpture.text = $"Sculpture is Inactive for {SculptureAI.countdown} seconds";
        }

        // Everest Info stuff ---------------------------------
        Everest.text = $"Everest - AbilityMeter: {EverestAI.AllergyMeter}% / AbilityFactor: {EverestAI.AllergyFactor}% per second / Player in range: {EverestAI.PlayerInRange} / Buffer: {EverestAI.Buffer}";

        // Milly Info stuff ---------------------------------
        Milly.text = $"Milly - Behavior: {MillyAI.Behavior} / Warn Meter: {MillyAI.WarnBuffer} / Warn Cooldown: {MillyAI.WarnCooldown}";
        
        if (MillyAI.PlayerInVicinity)
        {
            if (MillyAI.PlayerInLineOfSight)
            {
                Milly.color = Color.red;
            } else
            {
                Milly.color = new Color32(255, 205, 112, 255);
            }
        } else
        {
            Milly.color = Color.white;
        }

        // Kelvin Info stuff ---------------------------------
        Kelvin.text = $"Kelvin - Player in range: {KelvinAI.PlayerInVicinity} / Player in Line of Sight: {KelvinAI.PlayerInLineOfSight}";

        if (KelvinAI.PlayerInVicinity)
        {
            if (KelvinAI.PlayerInLineOfSight)
            {
                Kelvin.color = Color.red;
            }
            else
            {
                Kelvin.color = new Color32(255, 205, 112, 255);
            }
        }
        else
        {
            Kelvin.color = Color.white;
        }

        // Player Info stuff ---------------------------------
        Player.text = $"Player - Sprinting: {PlayerMovement.IsSprinting} / Milly Warnings: {PlayerMovement.Warns}";
    }

}
