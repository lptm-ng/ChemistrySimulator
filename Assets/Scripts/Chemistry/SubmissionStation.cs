using UnityEngine;

public class SubmissionStation : MonoBehaviour
{
    public void Interact()
    {
        UIManager.Instance.OpenSubmission();
    }
}