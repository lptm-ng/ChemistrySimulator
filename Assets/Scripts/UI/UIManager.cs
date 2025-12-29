using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI infoText;
    public TextMeshProUGUI equationText;
    public GameObject equationPanel;

    private float equationTimer = 0f;

    public void Awake()
    {
        Instance = this;
        if(equationPanel != null) equationPanel.SetActive(false);
    }

    public void ShowChemicalInfo(string name, string formula, bool isUnknown = false)
    {
        if (isUnknown)
        {
            infoText.text = "Unbekannte Substanz <color=orange>(???)</color>";
        }
        else
        {
            infoText.text = $"{name} <color=orange> ({formula})</color>";
        }
    }

    public void ClearInfo()
    {
        infoText.text = "";
    }

    public void DisplayEquation(string equation)
    {
        equationText.text = equation;
        equationPanel.SetActive(true);
        equationTimer = 15f;
    }

    void Update()
    {
        if(equationTimer > 0)
        {
            equationTimer -= Time.deltaTime;
            if(equationTimer <= 0) equationPanel.SetActive(false);
        }
    }
}
