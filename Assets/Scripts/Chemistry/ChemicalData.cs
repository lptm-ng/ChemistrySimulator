using UnityEngine;

[CreateAssetMenu(fileName = "ChemicalData", menuName = "Scriptable Objects/ChemicalData")]
public class ChemicalData : ScriptableObject
{
    public enum ChemicalType
    {
        NONE,
        CATION,
        ANION,
        REAGENT, // Chemiekalien, die man fürs Nachweisen braucht
        SOLVENT // wie Wasser
    }

    [Header("Allg. Eigenschaften")] public string chemicalName;
    public string formula; // Strukturformel hier
    public ChemicalType type;

    [Header("Visuelle Eigenschaften")] public Color
        liquidColor =
            Color.clear; // Trübung der LÖSUNG (trüb oder nicht trüb, um z.B. schon einmal zu wissen, ob da Blei drin ist)

    public bool isSolid = false; // Pulver oder Flüssigkeit --> Spatel oder kein Spatel?

    [Header("Spez. Nachweis-Infos")] [TextArea(15, 20)]
    public string infoText; // für das Nachschlagewerk
}