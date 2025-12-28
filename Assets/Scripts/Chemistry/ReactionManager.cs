using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReactionManager : MonoBehaviour
{
    public static ReactionManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckReaction(ChemicalContainer container)
    {
        var contentsChemicals = container.contents.Select(c => c.chemicalName).ToList();

        // 1. Goldregen-Nachweis (Blei + Kaliumiodid)
        if(contentsChemicals.Contains("Blei") && contentsChemicals.Contains("Kaliumiodid"))
        {
            TriggerReaction(container, "Goldregen", "Pb²⁺ + 2 I⁻ → PbI₂ ↓ (gelber Niederschlag)");
            // todo: trigger gelber parikel regen
        }

        // 2. Blutroter Komplex (Eisen + Kaliumthiocyanat)
        if(contentsChemicals.Contains("Eisen") && contentsChemicals.Contains("Kaliumthiocyanat"))
        {
            TriggerReaction(container, "Blutrot", "Fe³⁺ + 3 SCN⁻ + 3 H₂O → [Fe(SCN)₃(H₂O)₃]");
            // todo: trigger blutrote lösung
        }

        // 3. Tollens
        TriggerTollensReaction(container, contentsChemicals);

        // 4. Sonnenuntergang-Nachweis (Thiosulfat + Silbernitrat)
        if(contentsChemicals.Contains("Thiosulfat") && contentsChemicals.Contains("Silbernitrat"))
        {
            TriggerReaction(container, "Sonnenuntergang", "1. 2 Ag⁺ + S₂O₃²⁻ → Ag₂S₂O₃ ↓ (weiß)\n2. Ag₂S₂O₃ + H₂O → Ag₂S ↓ (schwarz) + H₂SO₄");
            // todo: zeitgesteuerter farbechsel (weiß -> gelb -> schwarz)
        }
    
    }

    public void TriggerTollensReaction(ChemicalContainer container, List<string>contentsChemicals)
    {
        // Tollens-Probe (Nachweis red. Anionen (eigentlich Carboxylate); Tartrat + Tollensreagenz (Silbernitrat, Wasser, Ammoniak))
        // Tollensreagenz (Silbernitrat und Ammoniak)
        int ammoniaCount = container.contents.Count(c => c.chemicalName == "Ammoniak");
        bool hasSilver = contentsChemicals.Contains("Silbernitrat");

        if (hasSilver)
        {
            if(ammoniaCount == 1)
            {
                container.isDissolved = false;
                TriggerReaction(container, "Silberoxid-Fällung", "Ag⁺ + OH⁻ → Ag₂O (brauner Niederschlag)");
            }
            else if(ammoniaCount >= 2)
            {
                container.isDissolved = true;
                TriggerReaction(container, "Tollensreagenz", "Ag⁺ + 2 NH₃ → [Ag(NH₃)₂]⁺"); // klare LÖsung
            }
        }
        
        if(hasSilver && ammoniaCount >= 2 && contentsChemicals.Contains("Tartrat"))
        {
            if (container.isHot)
            {
                TriggerReaction(container, "Silberspiegel", "C₄H₄O₆²⁻ + 10 [Ag(NH₃)₂]⁺ + 8 OH⁻ → 4 CO₂ + 10 Ag + 20 NH₃ + 6 H₂O (elementares Silber)");
            }
        }
    }
    

    private void TriggerReaction(ChemicalContainer container, string reactionName, string equation)
    {
        // später, for now erstmal debug logs
        Debug.Log("REAKTION: " + reactionName);
        Debug.Log("REAKTIONSGLEICHUNG: " + equation);

        // später halt, dass UI die Gleichung zeigt und dass sich der Inhalt vom Container verändert
    }

    private Color GetFlameColor(ChemicalData chemical)
    {
        if(chemical.chemicalName == "Lithium") return new Color(1f, 0f, 0.2f);
        if(chemical.chemicalName == "Kalium") return new Color(0.8f, 0.2f, 1f);
        return Color.blue; // nicht-leuchtende Flamme --> Flammenfärbung macht man nicht mit der leuchtenden Flamme aka. orange/rot
    }
}
