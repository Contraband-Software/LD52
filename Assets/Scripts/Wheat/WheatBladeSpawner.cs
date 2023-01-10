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
        [SerializeField] RectTransform wheatBladeSpawnArea;
        [SerializeField] Transform parent;

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

            Vector4 bounds = new Vector4(
                Mathf.FloorToInt(wheatBladeSpawnArea.localPosition.x),
                Mathf.FloorToInt(wheatBladeSpawnArea.localPosition.y),
                Mathf.FloorToInt(wheatBladeSpawnArea.localPosition.x + wheatBladeSpawnArea.sizeDelta.x),
                Mathf.FloorToInt(wheatBladeSpawnArea.localPosition.y + wheatBladeSpawnArea.sizeDelta.y)
            );

            for (int i = 0; i < amount; i++)
            {
                GameObject blade = Instantiate(wheatBladePrefab, parent);
                blade.transform.position = new Vector3(Random.Range(bounds.x, bounds.z), Random.Range(bounds.y, bounds.w), 0);
            }
        }
    }
}