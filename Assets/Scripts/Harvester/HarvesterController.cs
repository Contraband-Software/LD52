using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

#pragma warning disable IDE0090
#pragma warning disable IDE0079 //suppression warning

namespace Architecture.Harvester
{
    using Wheat;
    using Managers;

    [
        RequireComponent(typeof(Rigidbody2D)),
        RequireComponent(typeof(PlayerInput)),
        RequireComponent(typeof(Animator)),
        DisallowMultipleComponent
    ]
    public class HarvesterController : MonoBehaviour
    {
        public static HarvesterController GetReference()
        {
            return GameObject.FindGameObjectWithTag("Player").GetComponent<HarvesterController>();
        }

        #region EVENTS
        public UnityEvent HarvesterDestroyed { get; private set; } = new UnityEvent();
        #endregion

        [Header("Self Component References")]
        [SerializeField] BoxCollider2D bladesCollider;

        [Header("Wheat Collision")]
        [SerializeField] LayerMask collideOnlyWithHarvesterBlade;

        [Header("Particle Effects")]
        [SerializeField] BladePFXController bladePFXController;
        [SerializeField] ParticleSystem wheatEjectPFX;
        [SerializeField] ParticleSystem meatEjectPFX;
        [SerializeField] ParticleSystem deathExplosionPFX;

        [Header("Settings")]
        [SerializeField, Min(0)] float acceleration = 8f;
        [SerializeField, Min(0)] float turnSpeed = 0.6f;
        [SerializeField, Min(0)] float penaltyTimeInSeconds = 10;
        [SerializeField, Range(0, 1)] float hazardSlowdownFactor = 0.3f;
        [SerializeField, Min(0)] float hazardEffectOnSpeed = 176;

        private Rigidbody2D rb;
        private Animator bladeAnimation;
        private WheatFieldManager wheatCollisionScript;

        private float horizontal;
        private float vertical;
        private float currentHazardSlowDownFactor = 1;
        private bool controlsLocked = false;

        public bool Penalty { get; private set; } = false;

        #region UNITY
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            bladeAnimation = GetComponent<Animator>();
            wheatCollisionScript = WheatFieldManager.GetReference();

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
        }

#pragma warning disable IDE0051 //unused warning (it is used)
        private void Move(InputAction.CallbackContext context)
        {
            if (!controlsLocked)
            {
                horizontal = context.ReadValue<Vector2>().x;
                vertical = context.ReadValue<Vector2>().y;
            }
        }
#pragma warning restore IDE0051

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Rocks"))
            {
                OnRockHit();
            }
        }
        #endregion

        private void CheckForBladeCollision()
        {
            if (!Penalty)
            {
                Bounds bladesAABB = bladesCollider.bounds;

                //find AABB coordinates of top left corner
                float leftX = bladesAABB.center.x - bladesAABB.extents.x;
                float topY = bladesAABB.center.y + bladesAABB.extents.y;

                Vector2 topLeft = new Vector2(leftX, topY);
                Vector2 topLeftGridCell = new Vector2(Mathf.Round(topLeft.x), Mathf.Round(topLeft.y));

                //use top left coordinate, sample 10x10
                //rows
                for (int i = 0; i < 10; i++)
                {
                    //columns
                    for (int j = 0; j < 10; j++)
                    {
                        //get coordinate by offsetting by the loop iteration
                        Vector2 probeCoordinate = new Vector2(topLeftGridCell.x + j, topLeftGridCell.y - i);
                        if (wheatCollisionScript.IsWheatTilePresent(probeCoordinate))
                        {
                            //do a point cast to see if it collides with the blades
                            //point cast from centre of tile (+0.5), do later
                            Vector2 centreOfTile = new Vector2(probeCoordinate.x + 0.5f, probeCoordinate.y - 0.5f);
                            RaycastHit2D hit = Physics2D.Raycast(centreOfTile, Vector2.up, 0f, collideOnlyWithHarvesterBlade);
                            if (hit)
                            {
                                wheatCollisionScript.DeleteWheatTileAtCoordinate(probeCoordinate);
                                bladePFXController.PlayHarvestPFX(centreOfTile);
                                wheatEjectPFX.Play();
                            }
                        }
                    }
                }
            }
        }

        private void OnRockHit()
        {
            // To stop the animal hit coroutines
            StopAllCoroutines();

            bladeAnimation.SetFloat("BladeSpeed", 0f);

            SoundSystem.Instance.PlaySound("Harvester_Breakdown");

            // Just so animals no longer get killed by the broken-down harvester
            Penalty = true;

            LockControls(true);
            deathExplosionPFX.Play();

            HarvesterDestroyed.Invoke();
        }

        #region PUBLIC_INTERFACE
        /// <summary>
        /// True = controls locked
        /// False = controls released
        /// </summary>
        /// <param name="status"></param>
        public void LockControls(bool status)
        {
            controlsLocked = status;
            horizontal = 0;
            vertical = 0;
        }

        /// <summary>
        /// Only for the Animal class to call
        /// </summary>
        public void OnAnimalHit()
        {
            meatEjectPFX.Play();

            bladeAnimation.SetFloat("BladeSpeed", 0f);

            SoundSystem.Instance.PlaySound("Harvester_Mincing");

            UIControllerLevel.GetReference().ShowBloodMask();

            StartCoroutine(PenaltyPeriod());
        }
        #endregion

        #region PENALTY_COROUTINES
        IEnumerator PenaltyPeriod()
        {
            Penalty = true;

            currentHazardSlowDownFactor = hazardSlowdownFactor;

            yield return new WaitForSeconds(penaltyTimeInSeconds);

            StartCoroutine(BackToNormal());
        }

        IEnumerator BackToNormal()
        {
            while (Mathf.Abs(1 - currentHazardSlowDownFactor) > 0.1f)
            {
                currentHazardSlowDownFactor += (1 - currentHazardSlowDownFactor) / hazardEffectOnSpeed;
                bladeAnimation.SetFloat("BladeSpeed", currentHazardSlowDownFactor);
                yield return new WaitForSecondsRealtime(0.01f);
            }

            Penalty = false;
        }
        #endregion
    }
}
