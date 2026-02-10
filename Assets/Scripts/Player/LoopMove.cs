using System;
using UnityEngine;

namespace Player
{
    public class LoopMove : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 destination;
        public float speed = 2f;
        public float rotationSpeed = 10f;
        public Animator animator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        public GameObject player;
        private Vector3 _lastPosition;

        private void Start()
        {
            _lastPosition = transform.position;
        }

        void Update()
        {
            float time = Mathf.PingPong(Time.time * speed, 1);
            transform.position = Vector3.Lerp(start, destination, time);
            Vector3 direction = transform.position - _lastPosition;
            animator.SetBool(IsWalking, true);

            if (direction.magnitude > 0.001f)
            {
                Quaternion destinationRotation = Quaternion.LookRotation(direction);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, destinationRotation, Time.deltaTime * rotationSpeed);
            }

            _lastPosition = transform.position;
        }
    }
}