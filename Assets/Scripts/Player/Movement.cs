using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [Header("Setup")] public Transform orientation;
        [SerializeField] private Animator animator;

        [Header("Movement Settings")] public float moveSpeed = 5f;
        public float gravity = -9.81f;

        private CharacterController _controller;
        private Vector3 _moveDirection;
        private Vector3 _velocity;
        private float _horizontalInput;
        private float _verticalInput;

        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        void Start()
        {
            _controller = GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            MyInput();

            MovePlayer();
        }

        private void MyInput()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            _horizontalInput = (keyboard.dKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed ? 1f : 0f);
            _verticalInput = (keyboard.wKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed ? 1f : 0f);
        }

        private void MovePlayer()
        {
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            Vector3 forward = orientation.forward;
            Vector3 right = orientation.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            _moveDirection = forward * _verticalInput + right * _horizontalInput;

            Vector3 finalMove = Vector3.zero;

            if (_moveDirection.magnitude >= 0.1f)
            {
                finalMove = _moveDirection.normalized * moveSpeed;

                animator.SetBool(IsWalking, true);
            }
            else
            {
                animator.SetBool(IsWalking, false);
            }

            _velocity.y += gravity * Time.deltaTime;

            finalMove.y = _velocity.y;

            _controller.Move(finalMove * Time.deltaTime);
        }
    }
}