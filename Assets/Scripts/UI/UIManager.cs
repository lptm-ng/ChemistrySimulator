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
        if(equationText != null) equationText.text = "";
        if(equationPanel != null) equationPanel.SetActive(false);
    }

    public void ShowChemicalInfo(string name, string formula, bool isUnknown = false)
    {
        infoText.transform.parent.gameObject.SetActive(true);

        if (isUnknown)
        {
            infoText.text = "Unbekannte Substanz <color=blue>(???)</color>";
        }
        else
        {
            infoText.text = $"{name} <color=blue> ({formula})</color>";
        }
    }

    public void ClearInfo()
    {
        if(infoText != null)
        {
            infoText.text = "";
            infoText.transform.parent.gameObject.SetActive(false);
        }
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
