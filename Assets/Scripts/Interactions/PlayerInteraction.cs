using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    private ChemicalContainer currentTarget; // fürs lexikon

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            ChemicalContainer container = hit.collider.GetComponent<ChemicalContainer>();
            if (container != currentTarget) // neues object?
            {
                ClearCurrentTarget();

                if (container != null && container.contents.Count > 0)
                {
                    currentTarget = container;
                    UIManager.Instance.ShowChemicalInfo(currentTarget.contents[0].chemicalName, currentTarget.contents[0].formula, currentTarget.isRandomSample
                    );
                }
            }
        }
        else
        {
            if (currentTarget != null)
            {
                ClearCurrentTarget();
            }
        }

        // lexikon (L für das öffnen)
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            if (UIManager.Instance.isLexikonOpen)
            {
                UIManager.Instance.CloseLexikon();
            }
            else if (currentTarget != null && currentTarget.contents.Count > 0 && !currentTarget.isRandomSample)
            {
                UIManager.Instance.DisplayLexikon(currentTarget.contents[0]);
            }
        }
    }

    private void ClearCurrentTarget()
    {
        currentTarget = null;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClearInfo();
        }
    }
}