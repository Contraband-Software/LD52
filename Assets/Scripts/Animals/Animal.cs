using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using Architecture.Harvester;

namespace Architecture.Hazards
{
    using Managers;

    [
        RequireComponent(typeof(BoxCollider2D)),
        DisallowMultipleComponent
    ]
    public class Animal : MonoBehaviour
    {
        public enum Type
        {
            Cow,
            Pig
        }

        public static Vector4 MoveBounds { get; set; } = Vector4.zero;

        [Header("General Settings")]
        [SerializeField] Type animalType = Type.Cow;

        [Header("Movement Settings")]
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
        private static float shakeAngle = 0.2f;

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
                Vector2 direction = targetPosition - transform.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
                transform.Translate(
                    (targetPosition - transform.position).normalized 
                    * Mathf.Clamp((targetPosition - transform.position).magnitude, 0, moveSpeed),
                    transform.parent
                );
            } else
            {
                transform.Rotate(new Vector3(0, 0, shakeAngle * Mathf.Sin(rotationSpeed + Time.fixedTime)));
            }
        }

        IEnumerator WaitForNextMovement() {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeToMove, maxTimeToMove));
            MoveToNewPosition();
        }

        void MoveToNewPosition()
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.tag == "HarvesterBlade")
            {
                spriteRenderer.enabled = false;
                boxCol.enabled = false;
                shadowCaster.enabled = false;
                
                bloodPFX.Play();
                intestinesPFX.Play();

                switch (animalType)
                {
                    case Type.Cow:
                        if (UnityEngine.Random.value > 0.5)
                        {
                            SoundSystem.Instance.PlaySound("Cow_Death_1");
                        } else
                        {
                            SoundSystem.Instance.PlaySound("Cow_Death_2");
                        }
                        break;
                    case Type.Pig:
                        SoundSystem.Instance.PlaySound("Pig_Death_1");
                        break;
                }

                //Plays the harvesters mincing PFX, COUPLING ISSUE
                col.transform.parent.gameObject.GetComponent<HarvesterController>().PlayMincingPFX();
            }
        }
    }
}