using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Hazards
{
    public class Animal : MonoBehaviour
    {
        [SerializeField, Min(0)] float moveSpeed = 1f;
        [SerializeField, Min(0)] float rotationSpeed = 1f;

        [SerializeField, Min(0)] float minTimeToMove = 0f;
        [SerializeField, Min(0)] float maxTimeToMove = 1f;

        [SerializeField, Min(0)] float minMoveDistance = 0f;
        [SerializeField, Min(0)] float maxMoveDistance = 0f;

        Vector2 targetPosition;

        private void Start()
        {
            targetPosition = transform.position;

#if UNITY_EDITOR
            if (maxTimeToMove < minTimeToMove || maxMoveDistance < minMoveDistance)
            {
                throw new System.ArgumentException("Max cannot be smaller than min.");
            }
#endif
        }

        private void FixedUpdate()
        {

        }

        IEnumerator WaitForNextMovement() { 
            yield return new WaitForSeconds(Random.Range(minTimeToMove, maxTimeToMove));
        }
    }
}