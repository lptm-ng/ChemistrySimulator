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
        private ChemicalContainer _currentChemical;


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
                Target foundTarget = hit.collider.GetComponentInParent<Target>();
                if (foundTarget != null)
                {
                    if (_currentTarget != foundTarget)
                    {
                        _currentTarget = foundTarget;
                        targetHandler.HandleTarget(foundTarget);
                    }
                }


                if (hit.collider.TryGetComponent<ChemicalContainer>(out var chemical))
                {
                    if (chemical != _currentChemical) // neues object?
                    {
                        ClearCurrentChemical();
                        if (chemical && chemical.contents.Count > 0)
                        {
                            _currentChemical = chemical;
                            UIManager.Instance.ShowChemicalInfo(_currentChemical.contents[0].chemicalName,
                                _currentChemical.contents[0].formula, _currentChemical.isRandomSample
                            );
                        }
                    }
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (hit.collider.TryGetComponent<SubmissionStation>(out var station))
                    {
                        station.Interact();
                    }
                }
            }
            else
            {
                if (!_currentChemical && !_currentTarget) return;
                ClearCurrentChemical();
                _currentTarget = null;
                targetHandler.ClearCurrentHighlight();
            }
        }

        private void HandleInput()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                _currentTarget?.Interact();
            }

            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                _currentTarget?.Interact();
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (_currentTarget)
                {
                    
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                GameObject heldObj = pickUpHandler.HeldObj;
                if (heldObj != null)
                {
                    ChemicalContainer sourceContainer = heldObj.GetComponent<ChemicalContainer>();

                    if (sourceContainer != null && sourceContainer.contents.Count > 0)
                    {
                        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                        if (Physics.Raycast(ray, out var hit, interactionRange, interactableLayer))
                        {
                            ChemicalContainer targetContainer = hit.collider.GetComponent<ChemicalContainer>();

                            // Nicht in sich selbst schütten!
                            if (targetContainer != null && targetContainer != sourceContainer)
                            {
                                PourChemical(sourceContainer, targetContainer);
                            }
                        }
                        
                        
                    }
                    
                }
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                if (UIManager.Instance.isLexikonOpen)
                {
                    UIManager.Instance.CloseLexikon();
                }
                else if (_currentChemical != null && _currentChemical.contents.Count > 0 &&
                         !_currentChemical.isRandomSample)
                {
                    UIManager.Instance.DisplayLexikon(_currentChemical.contents[0]);
                }
            }
        }

        private void ClearCurrentChemical()
        {
            _currentChemical = null;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ClearInfo();
            }
        }
        
        
        private void PourChemical(ChemicalContainer source, ChemicalContainer target)
        {
            foreach (var chem in source.contents)
            {
                target.AddChemical(chem);
            }

            // source.ClearContainer();
    
            Debug.Log("Chemikalien hinzugefügt");
        }
        
    }
}