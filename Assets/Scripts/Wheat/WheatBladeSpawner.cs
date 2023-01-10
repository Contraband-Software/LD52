using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Wheat
{
    //[
    //    RequireComponent(typeof(BoxCollider2D))
    //]
    public sealed class WheatBladeSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject wheatBladePrefab;

        [Header("Settings")]
        [SerializeField, Min(0)] uint amount = 250;
        [SerializeField, Min(0)] float wheatDirectionForce = 5.0f;
        [SerializeField, Range(0, Mathf.PI)] float leftAngleLimit = 0.2f;
        [SerializeField, Range(0, Mathf.PI)] float rightAngleLimit = 0.2f;

        void Awake()
        {
#if UNITY_EDITOR
            if (wheatBladePrefab == null)
            {
                throw new MissingReferenceException("Missing wheat blade prefab reference");
            }
#endif

            WheatBlade.LeftAngleLimit = leftAngleLimit;
            WheatBlade.RightAngleLimit = rightAngleLimit;
            WheatBlade.WheatDirectionForce = wheatDirectionForce;

            BoxCollider2D bc = GetComponent<BoxCollider2D>();
            for (int i = 0; i < amount; i++)
            {
                GameObject blade = Instantiate(wheatBladePrefab, transform);
                blade.transform.position = new Vector3(Random.Range(bc.offset.x - bc.size.x/2, bc.size.x/2), Random.Range(bc.offset.y - bc.size.y/2, bc.size.y/2), 0);
            }
        }
    }
}