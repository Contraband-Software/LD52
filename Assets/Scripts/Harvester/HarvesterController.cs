using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [Header("Self Component References")]
        [SerializeField] BoxCollider2D bladesCollider;

        [Header("Wheat Collision")]
        [SerializeField] WheatFieldManager wheatCollisionScript;
        [SerializeField] LayerMask collideOnlyWithHarvesterBlade;

        [Header("Particle Effects")]
        [SerializeField] BladePFXController bladePFXController;

        [Header("Settings")]
        [SerializeField, Min(0)] float acceleration = 8f;
        [SerializeField, Min(0)] float turnSpeed = 0.6f;

        private Rigidbody2D rb;
        private float horizontal;
        private float vertical;

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
            rb.AddForce(acceleration * vertical * transform.up);
            transform.Rotate(horizontal * Vector3.forward * -1 * turnSpeed * Mathf.Abs(vertical));
        }

        private void Move(InputAction.CallbackContext context)
        {
            horizontal = context.ReadValue<Vector2>().x;
            vertical = context.ReadValue<Vector2>().y;
        }
        #endregion

        private void CheckForBladeCollision()
        {
            Bounds bladesAABB = bladesCollider.bounds;

            //find AABB coordinates
            float leftX = bladesAABB.center.x - bladesAABB.extents.x;
            //float rightX = bladesAABB.center.x + bladesAABB.extents.x;
            float topY = bladesAABB.center.y + bladesAABB.extents.y;
            //float bottomY = bladesAABB.center.y - bladesAABB.extents.y;

            Vector2 topLeft = new Vector2(leftX, topY);
            //Vector2 topRight = new Vector2(rightX, topY);
            //Vector2 bottomLeft = new Vector2(leftX, bottomY);
            //Vector2 bottomRight = new Vector2(rightX, bottomY);

            Vector2 topLeftGridCell = new Vector2(Mathf.Round(topLeft.x), Mathf.Round(topLeft.y));
            //print(wheatCollisionScript.IsWheatTilePresent(topLeftGridCell));


            //topLeftGrid = topLeftGridCell;
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
                        }
                    }
                }
            }
        }
    }
}
