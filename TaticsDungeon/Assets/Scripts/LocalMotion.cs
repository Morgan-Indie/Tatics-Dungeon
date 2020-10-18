using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class LocalMotion : MonoBehaviour
    {
        public Rigidbody characterRigidbody;
        public AnimationHandler animationHandler;

        float movementSpeed = 3f;
        float rotationSpeed = 25f;

        // Start is called before the first frame update
        void Start()
        {
            characterRigidbody = GetComponent<Rigidbody>();
            animationHandler = GetComponent<AnimationHandler>();
        }

        public void HandleRotation(float delta, Vector3 moveDirection)
        {
            if (moveDirection == Vector3.zero)
                moveDirection = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation.x = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                targetRotation, rotationSpeed * delta);
        }

        public void Move(Vector3 currentDirection, Vector3 nextPos, float delta)
        {
            HandleRotation(delta, currentDirection);
            characterRigidbody.velocity = movementSpeed * currentDirection;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, (1 << 0)))
            {
                if (hit.distance > .4f && transform.position.y > nextPos.y + .2f)
                {
                    characterRigidbody.velocity += Vector3.down * 10f;
                }
            }
            if (transform.position.y < nextPos.y - .2f)
            {
                characterRigidbody.velocity += Vector3.up * 1.5f;
            }

            animationHandler.UpdateAnimatorValues(delta, 1f);
        }

        public void Stop()
        {
            characterRigidbody.velocity = Vector3.zero;
            characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            animationHandler.PlayTargetAnimation("CombatIdle");
        }
    }
}

