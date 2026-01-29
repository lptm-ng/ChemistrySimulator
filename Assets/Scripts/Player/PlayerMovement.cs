using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    //Deprecated
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int WalkingTrigger = Animator.StringToHash("WalkingTrigger");
        [Header("Movement")] public float moveSpeed = 5f;
        public float groundDrag = 5f;

        public Transform orientation;
        float horizontalInput;
        float verticalInput;
        Vector3 moveDirection;
        Rigidbody rb;

        [SerializeField] private Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.linearDamping = groundDrag;
        }

        void Update()
        {
            MyInput();
        }

        private void FixedUpdate()
        {
            if (MovePlayer())
            {
                animator.SetTrigger(WalkingTrigger);
            }
        }

        public void MyInput()
        {
            var keyboard = Keyboard.current;

            horizontalInput = (keyboard.dKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed ? 1f : 0f);
            verticalInput = (keyboard.wKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed ? 1f : 0f);
        }

        public bool MovePlayer()
        {
            if (moveDirection != Vector3.zero)
            {
                const float rotationSpeed = 10f;
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);

            return moveDirection.magnitude > 0;
        }
    }
}