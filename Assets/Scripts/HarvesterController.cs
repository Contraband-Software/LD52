using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Harvester
{
    public class HarvesterController : MonoBehaviour
    {
        public Vector2 topLeftGrid;
        public Tile testingTile;

        [Header("Self Component References")]
        [SerializeField] Rigidbody2D rb;
        [SerializeField] BoxCollider2D bladesCollider;

        [Header("Wheat Collision")]
        [SerializeField] WheatCollision wheatCollisionScript;
        [SerializeField] LayerMask collideOnlyWithHarvesterBlade;

        private float horizontal;
        private float vertical;
        private float speed = 8f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckForBladeCollision();

            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }

        public void Move(InputAction.CallbackContext context)
        {
            vertical = context.ReadValue<Vector2>().y;
        }

        private void CheckForBladeCollision()
        {
            Bounds bladesAABB = bladesCollider.bounds;

            //find AABB coordinates
            float leftX = bladesAABB.center.x - bladesAABB.extents.x;
            float rightX = bladesAABB.center.x + bladesAABB.extents.x;
            float topY = bladesAABB.center.y + bladesAABB.extents.y;
            float bottomY = bladesAABB.center.y - bladesAABB.extents.y;

            Vector2 topLeft = new Vector2(leftX, topY);
            Vector2 topRight = new Vector2(rightX, topY);
            Vector2 bottomLeft = new Vector2(leftX, bottomY);
            Vector2 bottomRight = new Vector2(rightX, bottomY);

            Vector2 topLeftGridCell = new Vector2(Mathf.Round(topLeft.x), Mathf.Round(topLeft.y));
            print(wheatCollisionScript.IsWheatTilePresent(topLeftGridCell));


            topLeftGrid = topLeftGridCell;
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
                        RaycastHit2D hit = Physics2D.Raycast(probeCoordinate, Vector2.up, 0f, collideOnlyWithHarvesterBlade);
                        if(hit)
                        {
                            wheatCollisionScript.DeleteWheatTileAtCoordinate(probeCoordinate);
                        }
                    }
                }
            }
        }
    }
}
