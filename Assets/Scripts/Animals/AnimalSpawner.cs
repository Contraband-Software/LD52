using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Architecture.Hazards
{
    [
        RequireComponent(typeof(RectTransform)), 
        DisallowMultipleComponent
    ]
    public class AnimalSpawner : MonoBehaviour
    {
        [Serializable]
        private sealed class AnimalSpawnOptions
        {
            [Header("Spawn animal")]
            public GameObject instance;
            [Min(0)] public uint amount = 10;
        }

        [Header("References")]
        [SerializeField] GameObject instanceParent;

        [Header("Settings")]
        [SerializeField] AnimalSpawnOptions[] animalSpawnOptions;

        void Start()
        {
            RectTransform bc = GetComponent<RectTransform>();

            Animal.MoveBounds = new Vector4(
                0,
                0,
                bc.sizeDelta.x,
                bc.sizeDelta.y
            );

            Debug.Log(Animal.MoveBounds.ToString());

            foreach (AnimalSpawnOptions animal in animalSpawnOptions)
            {
                for (int i = 0; i < animal.amount; i++) {
                    GameObject obj = Instantiate(animal.instance, instanceParent.transform);
                    obj.transform.localPosition = new Vector3(
                        UnityEngine.Random.Range(Animal.MoveBounds.x, Animal.MoveBounds.z),
                        UnityEngine.Random.Range(Animal.MoveBounds.y, Animal.MoveBounds.w),
                        0
                    );
                    obj.SetActive(true);
                }
            }
        }
    }
}