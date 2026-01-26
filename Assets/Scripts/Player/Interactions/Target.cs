using System;
using Player.Interactions;
using UnityEngine;

namespace Interactions
{
    public class Target : MonoBehaviour, IInteractable
    {
        private Outline _outline;

        private void Awake()
        {
            if (!TryGetComponent<Outline>(out _outline))
            {
                _outline = gameObject.AddComponent<Outline>();
                _outline.OutlineColor = Color.yellow;
                _outline.OutlineWidth = 15.0f;
            }

            _outline.enabled = false;
        }

        public void ActivateHighlight()
        {
            if (_outline is not null) _outline.enabled = true;
        }

        public void DeactivateHighlight()
        {
            if (_outline is not null) _outline.enabled = false;
        }

        public void Interact()
        {
            var pickUpHandler = FindFirstObjectByType<PickUpHandler>();

            if (!pickUpHandler) return;
            pickUpHandler.PickUp(gameObject);
            DeactivateHighlight();

        }

        public string GetDescription()
        {
            return "Using " + gameObject.name;
        }
    }
}