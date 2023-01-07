using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Rendering.Universal;

namespace Architecture.Hazards
{
    [
        RequireComponent(typeof(BoxCollider2D)),
        //RequireComponent(typeof(Rigidbody2D))
    ]
    public class Animal : MonoBehaviour
    {
        public static Vector4 MoveBounds { get; set; } = Vector4.zero;

        [Header("Movement Variables")]
        [SerializeField, Min(0)] float moveSpeed = 1f;
        [SerializeField, Min(0)] float rotationSpeed = 7f;

        [SerializeField, Min(0), Tooltip("In seconds")] float minTimeToMove = 0f;
        [SerializeField, Min(0), Tooltip("In seconds")] float maxTimeToMove = 10f;

        [SerializeField, Min(0), Tooltip("In world units")] float minMoveDistance = 10f;
        [SerializeField, Min(0), Tooltip("In world units")] float maxMoveDistance = 100f;

        [Header("Particle Effects")]
        [SerializeField] ParticleSystem bloodPFX;
        [SerializeField] ParticleSystem intestinesPFX;

        [Header("Self Component References")]
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] BoxCollider2D boxCol;
        [SerializeField] ShadowCaster2D shadowCaster;

        private static float stoppingThreshold = 1f;

        Vector3 targetPosition;
        bool moving = false;

        private void Start()
        {
            targetPosition = transform.position;

#if UNITY_EDITOR
            if (maxTimeToMove < minTimeToMove || maxMoveDistance < minMoveDistance)
            {
                throw new System.ArgumentException("Max cannot be smaller than min.");
            }
#endif

            StartCoroutine(WaitForNextMovement());
        }

        private void Update()
        {
            if ((targetPosition - transform.position).magnitude < stoppingThreshold && moving)
            {
                moving = false;
                StartCoroutine(WaitForNextMovement());
            }
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                transform.Translate(
                    (targetPosition - transform.position).normalized 
                    * Mathf.Clamp((targetPosition - transform.position).magnitude, 0, moveSpeed),
                    transform.parent
                );
            } else
            {
                transform.Rotate(new Vector3(0, 0, rotationSpeed));
            }
        }

        IEnumerator WaitForNextMovement() {
            Debug.Log("Nigger");
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeToMove, maxTimeToMove));
            moveToNewPosition();
        }

        void moveToNewPosition()
        {
            float directionToMove = UnityEngine.Random.Range(0, Mathf.PI * 2);
            float distanceToMove = UnityEngine.Random.Range(minMoveDistance, maxMoveDistance);

            moving = true;
            targetPosition = new Vector3(
                Mathf.Clamp(Mathf.Cos(directionToMove) * distanceToMove, MoveBounds.x, MoveBounds.z),
                Mathf.Clamp(Mathf.Sin(directionToMove) * distanceToMove, MoveBounds.y, MoveBounds.w),
                0
            );
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.gameObject.tag == "HarvesterBlade")
            {
                spriteRenderer.enabled = false;
                boxCol.enabled = false;
                shadowCaster.enabled = false;
                
                bloodPFX.Play();
                intestinesPFX.Play();
            }
        }
    }
}