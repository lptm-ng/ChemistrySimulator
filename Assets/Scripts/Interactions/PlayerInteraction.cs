using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // how far player can reach/interact w/ smth
    public LayerMask interactableLayer;

    void Update()
    {
        // raycast for interaction logic
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            ChemicalContainer container = hit.collider.GetComponent<ChemicalContainer>();
            if(container != null && container.contents.Count > 0)
            {
                ChemicalData chemicals = container.contents[0];
                if(UIManager.Instance != null)
                {
                    UIManager.Instance.ShowChemicalInfo(chemicals.chemicalName, chemicals.formula);
                }
                else
                {
                    if(UIManager.Instance != null) UIManager.Instance.ClearInfo();
                }
            }
            
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                //
            }
        }
    }
}
