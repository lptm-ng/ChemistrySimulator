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
            TriggerReaction(container, "Goldregen", "Pb<sup>2+</sup> + 2 I<sup>-</sup> → PbI<sub>2</sub>↓ (gelber Niederschlag)");
            // todo: trigger gelber parikel regen
        }

        // 2. Blutroter Komplex (Eisen + Kaliumthiocyanat)
        if(contentsChemicals.Contains("Eisen") && contentsChemicals.Contains("Kaliumthiocyanat"))
        {
            TriggerReaction(container, "Blutrot", "Fe<sup>3+</sup> + 3 SCN<sup>-</sup> + 3 H<sub>2</sub>O → [Fe(SCN)<sub>3</sub>(H<sub>2</sub>O)<sub>3</sub>] (blutrote Lösung)");
            // todo: trigger blutrote lösung
        }

        // 3. Tollens
        TriggerTollensReaction(container, contentsChemicals);

        // 4. Sonnenuntergang-Nachweis (Thiosulfat + Silbernitrat)
        if(contentsChemicals.Contains("Thiosulfat") && contentsChemicals.Contains("Silbernitrat"))
        {
            TriggerReaction(container, "Sonnenuntergang", "1. 2 Ag<sup>+</sup> + S<sub>2</sub>O<sub>3</sub><sup>2-</sup> → Ag<sub>2</sub>S<sub>2</sub>O<sub>3</sub>↓ (weiß)\n2. Ag<sub>2</sub>S<sub>2</sub>O<sub>3</sub> + H<sub>2</sub>O → Ag<sub>2</sub>S ↓ + H<sub>2</sub>SO<sub>4</sub>");
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
                TriggerReaction(container, "Tollensreagenz", "Ag<sup>+</sup> + 2 NH<sub>3</sub> → [Ag(NH<sub>3</sub>)<sub>2</sub>]<sup>+</sup>"); // klare LÖsung
            }
        }
        
        if(hasSilver && ammoniaCount >= 2 && contentsChemicals.Contains("Tartrat"))
        {
            if (container.isHot)
            {
                TriggerReaction(container, "Silberspiegel", "C<sub>4</sub>H<sub>4</sub>O<sub>6</sub><sup>2-</sup> + 10 [Ag(NH<sub>3</sub>)<sub>2</sub>]<sup>+</sup> + 8 OH<sup>-</sup> → 4 CO<sub>2</sub> + 10 Ag + 20 NH<sub>3</sub> + 6 H<sub>2</sub>O (elementares Silber)");
            }
        }
    }
    

    private void TriggerReaction(ChemicalContainer container, string reactionName, string equation)
    {
        Debug.Log("REAKTION: " + reactionName);
        Debug.Log("REAKTIONSGLEICHUNG: " + equation);

        if(UIManager.Instance != null)
        {
            UIManager.Instance.DisplayEquation(equation);
        }
    }

    public void TriggerFlameTest(ChemicalData chemical, FumeHoodStation station)
    {
        if (!station.isPlayerInZone)
        {
            UIManager.Instance.DisplayEquation("Sicherheitshinweis: Flammenfärbung nur unter dem Abzug erlaubt!");
            return;
        }

        Color flameColor = GetFlameColor(chemical);
        var main = station.burnerFlame.main;
        main.startColor = flameColor;
    }

    private Color GetFlameColor(ChemicalData chemical)
    {
        if(chemical.chemicalName == "Lithium") return new Color(1f, 0f, 0.2f);
        if(chemical.chemicalName == "Kalium") return new Color(0.8f, 0.2f, 1f);
        return Color.blue; // nicht-leuchtende Flamme --> Flammenfärbung macht man nicht mit der leuchtenden Flamme aka. orange/rot
    }
}
