using System.Collections;
using UnityEngine;

namespace Player.Interactions
{
    public class PickUpHandler : MonoBehaviour
    {
        [Header("References")] public Transform holdPos;
        public Animator playerAnim;

        [Header("Settings")] public float throwForce = 500f;
        private const float RotationSensitivity = 1f;

        private GameObject HeldObj { get; set; }
        private Rigidbody _heldObjRb;
        private bool _canDrop = true;

        private int _layerNumber;
        private int _rememberedLayer;
        

        private static readonly int PickingUpTrigger = Animator.StringToHash("PickUpTrigger");

        void Start()
        {
            _layerNumber = LayerMask.NameToLayer("Hold Layer");

            if (_layerNumber == -1)
            {
                Debug.LogError("Layer 'Hold Layer' existiert nicht!");
            }
        }

        void Update()
        {
            if (!HeldObj) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (HeldObj)
                {
                    DropObject();
                }
            }

            RotateObject();

            if (!Input.GetKeyDown(KeyCode.Mouse0) || !_canDrop) return;
            StopClipping();
            ThrowObject();
        }

        public void PickUp(GameObject pickUpObj)
        {
            if (HeldObj) return;
            

            StartCoroutine(PickUpRoutine(pickUpObj));
        }

        IEnumerator PickUpRoutine(GameObject pickUpObj)
        {
            playerAnim.ResetTrigger(PickingUpTrigger);
            playerAnim.SetTrigger(PickingUpTrigger);

            yield return new WaitForSeconds(0.5f);

            HeldObj = pickUpObj;
            _rememberedLayer = HeldObj.layer;
            _heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            _heldObjRb.isKinematic = true;

            _heldObjRb.transform.SetParent(holdPos);

            HeldObj.transform.localPosition = Vector3.zero;
            HeldObj.transform.localRotation = Quaternion.identity;

            Physics.IgnoreCollision(HeldObj.GetComponent<Collider>(), GetComponent<Collider>(), true);

            SetLayerRecursively(HeldObj, _layerNumber);
            
           
        }

        public void DropObject()
        {
            Debug.Log("Lasse das Objekt fallen");
            if (!HeldObj) return;
            ClearObjectPhysics();
            HeldObj = null;
        }

        void RotateObject()
        {
            if (Input.GetKey(KeyCode.R))
            {
                _canDrop = false;
                float xaxisRotation = Input.GetAxis("Mouse X") * RotationSensitivity;
                float yaxisRotation = Input.GetAxis("Mouse Y") * RotationSensitivity;

                HeldObj.transform.Rotate(Vector3.down, xaxisRotation, Space.Self);
                HeldObj.transform.Rotate(Vector3.right, yaxisRotation, Space.Self);
            }
            else
            {
                _canDrop = true;
            }
        }

        void ThrowObject()
        {
            if (!HeldObj) return;

            Rigidbody rb = _heldObjRb;
            ClearObjectPhysics();

            rb.AddForce(transform.forward * throwForce);

            HeldObj = null;
        }

        void StopClipping()
        {
            var clipRange = Vector3.Distance(HeldObj.transform.position, transform.position);
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward),
                clipRange);

            if (hits.Length > 1)
            {
                HeldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
            }
        }

        private void ClearObjectPhysics()
        {
            if (!HeldObj) return;

            Physics.IgnoreCollision(HeldObj.GetComponent<Collider>(), GetComponent<Collider>(), false);

            SetLayerRecursively(HeldObj, _rememberedLayer);

            _heldObjRb.isKinematic = false;
            HeldObj.transform.parent = null;
        }

        void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (!obj) return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (!child) continue;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}