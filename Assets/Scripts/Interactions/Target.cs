using UnityEngine;

namespace Interactions
{
    public class Target : MonoBehaviour
    {
        
        private Outline _outline;

        private void Start()
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
        
    }
}