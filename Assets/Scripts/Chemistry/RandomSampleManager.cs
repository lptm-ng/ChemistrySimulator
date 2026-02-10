using System.Collections.Generic;
using UnityEngine;

public class RandomSampleManager : MonoBehaviour
{
    [Header("Container mit der Probe")] public ChemicalContainer targetContainer;

    [Header("Mögliche Ionen")] public List<ChemicalData> allCations;
    public List<ChemicalData> allAnions;

    [Header("Aktuelle Probe")] public ChemicalData currentCation;
    public ChemicalData currentAnion;

    void Start()
    {
        GenerateNewTask();
    }

    public void GenerateNewTask()
    {
        // falls noch voll bevor der neuen aufgabe
        targetContainer.ClearContainer();

        bool validCombination = false;
        while (!validCombination)
        {
            // random
            currentCation = allCations[Random.Range(0, allCations.Count)];
            currentAnion = allAnions[Random.Range(0, allAnions.Count)];

            // Eisen(II)-thiosulfat ist instabil in wasser, deswegen ist diese kombi nicht möglich (Thiosulfatsalze wird auch für den Eisen-Nachweis benutzt)
            if (currentCation.chemicalName == "Eisen" && currentAnion.chemicalName == "Thiosulfat")
            {
                validCombination = false;
            }
            else
            {
                validCombination = true;
            }
        }

        // Ionen adden in Container
        targetContainer.AddChemical(currentCation);
        targetContainer.AddChemical(currentAnion);
        targetContainer.isRandomSample = true;

        Debug.Log($"Aufgabe mit Kation: {currentCation.chemicalName} + Anion: {currentAnion.chemicalName}");
    }

    public bool CheckPlayerSolution(string guessedCation, string guessedAnion)
    {
        return guessedCation == currentCation.chemicalName && guessedAnion == currentAnion.chemicalName;
    }

    public void SubmitSolution(int guessedCationIndex, int guessedAnionIndex)
    {
        string guessedCation = allCations[guessedCationIndex].chemicalName;
        string guessedAnion = allAnions[guessedAnionIndex].chemicalName;

        if (CheckPlayerSolution(guessedCation, guessedAnion))
        {
            Debug.Log("Richtige Abgabe");
            UIManager.Instance.ShowFeedback("Richtig!<br>Deine Analyse ist korrekt.", Color.darkGreen);

            // neue aufgabe + automatisches schließen
            Invoke("CloseUIAndNewTask", 4f);
        }
        else
        {
            Debug.Log("Falsche Abgabe");
            UIManager.Instance.ShowFeedback("Falsch!<br>Überprüfe deine Analyse noch einmal.", Color.red);
        }
    }

    private void CloseUIAndNewTask()
    {
        UIManager.Instance.CloseSubmission();
        UIManager.Instance.ShowFeedback("", Color.white);
        GenerateNewTask();
    }
}