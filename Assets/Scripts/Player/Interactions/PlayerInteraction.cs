using UnityEngine;
using System;
using Player.Interactions;
using UnityEngine.InputSystem;

namespace Interactions
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Settings")] [SerializeField]
        private float interactionRange = 3f; // how far player can reach/interact w/ smth

        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private TargetHandler targetHandler;
        [SerializeField] private PickUpHandler pickUpHandler; // Referenz im Inspector ziehen!
        private Camera _camera;
        private Target _currentTarget;


        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (pickUpHandler.HeldObj)
            {
                targetHandler.ClearCurrentHighlight();
        
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickUpHandler.DropObject();
                }
                return;
            }
            
            HandleDetection();
            HandleInput();
        }

        private void HandleDetection()
        {
            if (_camera is null) return;

            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out var hit, interactionRange, interactableLayer))
            {
                if (hit.collider.TryGetComponent<Target>(out var target))
                {
                    if (_currentTarget == target) return;
                    _currentTarget = target;
                    targetHandler.HandleTarget(target);
                    return;
                }
            }

            if (!_currentTarget) return;
            _currentTarget = null;
            targetHandler.ClearCurrentHighlight();
        }

        private void HandleInput()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryInteract();
            }
        }

        private void TryInteract()
        {
            _currentTarget?.Interact();
        }
    }
}