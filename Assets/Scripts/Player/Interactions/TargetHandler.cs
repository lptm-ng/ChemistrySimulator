using System;
using UnityEngine;

namespace Player.Interactions
{
    
    public class TargetHandler : MonoBehaviour
    {
        public event Action<GameObject> OnMix;
        private Target _currentTarget;

        public void HandleTarget(Target target)
        {
            if (_currentTarget == target) return;
            ClearCurrentHighlight();
            _currentTarget = target;
            _currentTarget.ActivateHighlight();
        }
        
        public void TryMix()
        {
            if (_currentTarget != null)
            {
                Debug.Log("Mix Triggered via Input");
                
                OnMix?.Invoke(_currentTarget.gameObject); 
            }
        }

        public void ClearCurrentHighlight()
        {
            if (_currentTarget is null) return;
            _currentTarget.DeactivateHighlight();
            _currentTarget = null;
        }
    }
}