using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCam : MonoBehaviour
    {
        [Header("Settings")] public float sensX;
        public float sensY;

        [Header("References")] public Transform playerBody;
        public Transform cameraTarget;
        float _xRotation;
        float _yRotation;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (UIManager.Instance && UIManager.Instance.isLexikonOpen) return;
            if (Mouse.current == null) return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float mouseX = mouseDelta.x * Time.deltaTime * sensX;
            float mouseY = mouseDelta.y * Time.deltaTime * sensY;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            if (playerBody)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }

        void LateUpdate()
        {
            if (cameraTarget)
            {
                transform.position = cameraTarget.position;
            }
        }
    }
}