using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("PopUp")] public TextMeshProUGUI infoText;
    public GameObject infoPanel;
    public TextMeshProUGUI equationText;
    public GameObject equationPanel;
    private float equationTimer = 0f;

    [Header("Lexikon")] public GameObject lexikonPanel;
    public TextMeshProUGUI lexikonName;
    public TextMeshProUGUI lexikonInhalt;
    public bool isLexikonOpen = false;

    [Header("Abgabe")] public GameObject submissionPanel;
    public TMP_Dropdown cationDropdown;
    public TMP_Dropdown anionDropdown;
    public TextMeshProUGUI feedbackText;
    public GameObject feedbackPanel;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // sonst gibt es einen bug mit dem menü
        }

        if (equationPanel != null) equationPanel.SetActive(false);
        if (lexikonPanel != null) lexikonPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
    }

    public void ShowChemicalInfo(string name, string formula, bool isUnknown = false)
    {
        infoPanel.SetActive(true);

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
        if (infoText != null)
        {
            infoText.text = "";
            infoPanel.SetActive(false);
        }
    }

    public void DisplayEquation(string equation)
    {
        equationText.text = equation;
        equationPanel.SetActive(true);
        equationTimer = 15f;
    }

    public void DisplayLexikon(ChemicalData data)
    {
        if (!isLexikonOpen)
        {
            lexikonName.text = data.chemicalName;
            lexikonInhalt.text = data.infoText;
            lexikonPanel.SetActive(true);
            isLexikonOpen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            CloseLexikon();
        }
    }

    public void CloseLexikon()
    {
        lexikonPanel.SetActive(false);
        isLexikonOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSubmission()
    {
        submissionPanel.SetActive(true);
        isLexikonOpen = true; // re-use vom bewegung-lock state
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;

        feedbackPanel.SetActive(true);
    }

    public void CloseSubmission()
    {
        submissionPanel.SetActive(false);
        isLexikonOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnSubmitButtonClick()
    {
        RandomSampleManager manager = FindFirstObjectByType<RandomSampleManager>();
        manager.SubmitSolution(cationDropdown.value, anionDropdown.value);
    }

    void Update()
    {
        if (equationTimer > 0)
        {
            equationTimer -= Time.deltaTime;
            if (equationTimer <= 0) equationPanel.SetActive(false);
        }
    }
}