using UnityEngine;

namespace Interactions
{
    public class TubeSlot : MonoBehaviour
    {
        public bool isOccupied = false;

        public GameObject currentTube;

        
        void Start()
        {
            if (transform.childCount > 0)
            {
                GameObject existingTube = transform.GetChild(0).gameObject;
                
                PlaceTube(existingTube);
            }
        }
        
        public void PlaceTube(GameObject tube)
        {
            tube.transform.position = transform.position;
            tube.transform.rotation = transform.rotation;

            tube.transform.SetParent(transform);

            Rigidbody rb = tube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            isOccupied = true;
            currentTube = tube;
        }

        public void RemoveTube()
        {
            isOccupied = false;
            currentTube = null;
        }
    }
}