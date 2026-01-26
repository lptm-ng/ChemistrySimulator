using System.Collections;
using UnityEngine;

namespace Player.Interactions
{
    public class PickUpHandler : MonoBehaviour
    {
        [Header("References")]
        public Transform holdPos;
        public Animator playerAnim;
        
        [Header("Settings")] 
        public float throwForce = 500f;
        private const float RotationSensitivity = 1f;

        public GameObject HeldObj { get; private set; }
        private Rigidbody _heldObjRb;
        private bool _canDrop = true;
        private int _layerNumber;
        private int _originalLayer;
        private static readonly int PickingUpTrigger = Animator.StringToHash("PickUpTrigger");

        void Start()
        {
            _layerNumber = LayerMask.NameToLayer("Hold Layer");
        }

        void Update()
        {
            if (HeldObj) 
            {
                MoveObject();
                RotateObject();
                if (Input.GetKeyDown(KeyCode.Mouse0) && _canDrop)
                {
                    StopClipping();
                    ThrowObject();
                }
            }
            if (!HeldObj) return;
            MoveObject();
            RotateObject();
            if (!Input.GetKeyDown(KeyCode.Mouse0) || !_canDrop) return;
            StopClipping();
            ThrowObject();
        }

        public void PickUp(GameObject pickUpObj)
        {
            if (HeldObj is not null) return;

            StartCoroutine(PickUpRoutine(pickUpObj));
        }

        IEnumerator PickUpRoutine(GameObject pickUpObj)
        {
            playerAnim.ResetTrigger(PickingUpTrigger);
            playerAnim.SetTrigger(PickingUpTrigger);

            yield return new WaitForSeconds(0.3f);

            HeldObj = pickUpObj;
            _heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            _heldObjRb.isKinematic = true;
            _heldObjRb.transform.parent = holdPos.transform;

            _originalLayer = HeldObj.layer;
            HeldObj.layer = _layerNumber;

            Physics.IgnoreCollision(HeldObj.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }

        public void DropObject()
        {
            if (!HeldObj) return;

            ClearObjectPhysics();
            HeldObj = null;
        }
        
        void MoveObject()
        {
            HeldObj.transform.position = holdPos.transform.position;
        }

        void RotateObject()
        {
            if (Input.GetKey(KeyCode.R))
            {
                _canDrop = false;
                float xaxisRotation = Input.GetAxis("Mouse X") * RotationSensitivity;
                float yaxisRotation = Input.GetAxis("Mouse Y") * RotationSensitivity;
                HeldObj.transform.Rotate(Vector3.down, xaxisRotation);
                HeldObj.transform.Rotate(Vector3.right, yaxisRotation);
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
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
            if (hits.Length > 1)
            {
                HeldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
            }
        }
        private void ClearObjectPhysics()
        {
            Physics.IgnoreCollision(HeldObj.GetComponent<Collider>(), GetComponent<Collider>(), false);
            HeldObj.layer = _originalLayer;
            _heldObjRb.isKinematic = false;
            HeldObj.transform.parent = null;
        }
    }
}