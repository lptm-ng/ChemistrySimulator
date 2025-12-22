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
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                //
            }
        }
    }
}
