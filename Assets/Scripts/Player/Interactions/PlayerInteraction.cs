using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Interactions
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private float interactionRange = 3f;

        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private TargetHandler targetHandler;
        [SerializeField] private PickUpHandler pickUpHandler;
        private Camera _camera;
        private Target _currentTarget;


        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
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
                _currentTarget?.Interact();
            }
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (_currentTarget)
                {
                    targetHandler.TryMix();
                }
            }
        }
    }
}