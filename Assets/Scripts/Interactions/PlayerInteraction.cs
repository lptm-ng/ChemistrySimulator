using UnityEngine;


namespace Interactions
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactionRange = 3f; // how far player can reach/interact w/ smth
        public LayerMask interactableLayer;

        [SerializeField] private TargetHandler targetHandler;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            // raycast for interaction logic

            if (_camera is null) return;

            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out var hit, interactionRange, interactableLayer))
            {
                if (hit.collider.TryGetComponent<Target>(out var target))
                {
                    targetHandler?.HandleTarget(target);
                }
                else
                {
                    targetHandler?.ClearCurrentHighlight();
                }
            }
            else
            {
                targetHandler?.ClearCurrentHighlight();
            }
        }
    }
}