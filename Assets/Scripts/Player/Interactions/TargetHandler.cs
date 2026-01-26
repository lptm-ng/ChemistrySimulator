using Interactions;
using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    private Target _currentTarget;

    public void HandleTarget(Target target)
    {
        if (_currentTarget == target) return;
        ClearCurrentHighlight();
        _currentTarget = target;
        _currentTarget.ActivateHighlight();
    }

    public void ClearCurrentHighlight()
    {
        if (_currentTarget is null) return;
        _currentTarget.DeactivateHighlight();
        _currentTarget = null;
    }
}