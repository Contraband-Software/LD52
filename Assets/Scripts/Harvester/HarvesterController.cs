using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

#pragma warning disable IDE0090

namespace Architecture.Harvester
{
    using Wheat;
    using Managers;

    [
        RequireComponent(typeof(Rigidbody2D)),
        RequireComponent(typeof(PlayerInput)),
        DisallowMultipleComponent
    ]
    public class HarvesterController : MonoBehaviour
    {
        #region EVENTS
        public UnityEvent HarvesterDestroyed { get; private set; } = new UnityEvent();
        #endregion

        [Header("Self Component References")]
        [SerializeField] BoxCollider2D bladesCollider;

        [Header("Wheat Collision")]
        [SerializeField] WheatFieldManager wheatCollisionScript;
        [SerializeField] LayerMask collideOnlyWithHarvesterBlade;

        [Header("Particle Effects")]
        [SerializeField] BladePFXController bladePFXController;
        [SerializeField] ParticleSystem wheatEjectPFX;
        [SerializeField] ParticleSystem meatEjectPFX;

        [Header("Settings")]
        [SerializeField, Min(0)] float acceleration = 8f;
        [SerializeField, Min(0)] float turnSpeed = 0.6f;
        [SerializeField, Range(0, 1)] float hazardSlowdownFactor = 0.3f;
        [SerializeField, Min(0)] float hazardEffectOnSpeed = 176;

        private Rigidbody2D rb;
        private float horizontal;
        private float vertical;
        private float currentHazardSlowDownFactor = 1;

        #region UNITY
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            SoundSystem.Instance.PlaySound("Harvester_Motor");
        }

        private void Update()
        {
            CheckForBladeCollision();
        }

        private void FixedUpdate()
        {
            rb.AddForce(acceleration * vertical * currentHazardSlowDownFactor * transform.up);
            //It is multiplied with Mathf.Abs(vertical) to make it harder to turn when slower, impossible when still
            transform.Rotate(-1 * currentHazardSlowDownFactor * turnSpeed * horizontal * Mathf.Abs(vertical) * Vector3.forward);

            currentHazardSlowDownFactor += (1 - currentHazardSlowDownFactor) / hazardEffectOnSpeed;
        }

#pragma warning disable IDE0051
        private void Move(InputAction.CallbackContext context)
        {
            horizontal = context.ReadValue<Vector2>().x;
            vertical = context.ReadValue<Vector2>().y;
        }
#pragma warning restore IDE0051

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Rocks")
            {
                OnRockHit();
            }
        }
        #endregion

        private void CheckForBladeCollision()
        {
            Bounds bladesAABB = bladesCollider.bounds;

            //find AABB coordinates of top left corner
            float leftX = bladesAABB.center.x - bladesAABB.extents.x;
            float topY = bladesAABB.center.y + bladesAABB.extents.y;

            Vector2 topLeft = new Vector2(leftX, topY);
            Vector2 topLeftGridCell = new Vector2(Mathf.Round(topLeft.x), Mathf.Round(topLeft.y));

            //use top left coordinate, sample 10x10
            //rows
            for(int i = 0; i < 10; i++)
            {
                //columns
                for(int j = 0; j < 10; j++)
                {
                    //get coordinate by offsetting by the loop iteration
                    Vector2 probeCoordinate = new Vector2(topLeftGridCell.x + j, topLeftGridCell.y - i);
                    if (wheatCollisionScript.IsWheatTilePresent(probeCoordinate))
                    {
                        //do a point cast to see if it collides with the blades
                        //point cast from centre of tile (+0.5), do later
                        Vector2 centreOfTile = new Vector2(probeCoordinate.x + 0.5f, probeCoordinate.y - 0.5f);
                        RaycastHit2D hit = Physics2D.Raycast(centreOfTile, Vector2.up, 0f, collideOnlyWithHarvesterBlade);
                        if(hit)
                        {
                            wheatCollisionScript.DeleteWheatTileAtCoordinate(probeCoordinate);
                            bladePFXController.PlayHarvestPFX(centreOfTile);
                            wheatEjectPFX.Play();
                        }
                    }
                }
            }
        }

        void OnRockHit()
        {
            HarvesterDestroyed.Invoke();

            SoundSystem.Instance.PlaySound("Harvester_Breakdown");
        }
        
        public void OnAnimalHit()
        {
            meatEjectPFX.Play();

            SoundSystem.Instance.PlaySound("Harvester_Mincing");

            currentHazardSlowDownFactor = hazardSlowdownFactor;
        }
    }
}
