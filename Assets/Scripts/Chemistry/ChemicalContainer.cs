using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChemicalContainer : MonoBehaviour
{
    [Header("Inhalt")]
    public List<ChemicalData> contents = new List<ChemicalData>();

    [Header("Zustand")]
    public bool isRandomSample = false;
    public bool isHot = false;
    public bool isDissolved = false;
    public bool isContaminated = false; // für die Entsorgung & GameLoop

    [Header("Visuell")]
    public MeshRenderer liquidRenderer;

    // [Chemikalie] in Behälter reinkippen
    public void AddChemical(ChemicalData chemical)
    {
        contents.Add(chemical);
        isContaminated = true;

        // Lösligkeit
        UpdateSolubilityState();

        ReactionManager.Instance.CheckReaction(this);
    }

    private void UpdateSolubilityState()
    {
        bool hasWater = contents.Any(c => c.chemicalName == "Wasser");
        bool hasHCL = contents.Any(c => c.chemicalName == "Salzsäure");
        bool hasLead = contents.Any(c => c.chemicalName == "Blei");

        if (hasLead && hasWater && !hasHCL)
        {
            isDissolved = false; // alle möglichen Blei-Verb. schwerlöslich --> trüb
        }
        else if (hasHCL || hasWater) // für alle anderen Kationen + Blei und HCL
        {
            isDissolved = true;
        }
    }

    public void ClearContainer()
    {
        contents.Clear();
        isHot = false;
        isDissolved = false;
        isContaminated = false;
    }

    // FÜR TESTS ohne das Kippen
    [ContextMenu("CheckForReactions TEST")]
    public void ManualTestTrigger()
    {
        if (ReactionManager.Instance != null)
        {
            UpdateSolubilityState();
            ReactionManager.Instance.CheckReaction(this);
        }
    }
}
