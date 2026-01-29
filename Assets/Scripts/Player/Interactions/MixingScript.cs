
using UnityEngine;

namespace Player.Interactions
{
    public class MixingScript : MonoBehaviour
    {

        [SerializeField] private TargetHandler handler;
        
        private void Start()
        {
            Renderer rend = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            handler.OnMix += OnMix;
        }

        private void OnDisable()
        {
            handler.OnMix -= OnMix;
        }

        private void OnMix(GameObject obj)
        {
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (!child.CompareTag("chemical")) continue;
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer)
                {
                    Debug.Log("Changed Color into Red");
                    childRenderer.material.color = Color.red;
                }
            } 
        }

        public void GetDescription()
        {
        }
    }
}