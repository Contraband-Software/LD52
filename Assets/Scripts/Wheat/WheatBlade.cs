using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.Wheat
{
    [
        RequireComponent(typeof(Rigidbody2D)),
        RequireComponent(typeof(Collider2D)),
        RequireComponent(typeof(HingeJoint2D))
    ]
    public class WheatBlade : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody2D bladeRigidbody;

        /// <summary>
        /// The level of force the wheat blade exerts to face in its 
        /// </summary>
        public static float WheatDirectionForce { get; set; } = 5.0f;

        /// <summary>
        /// The left side of the range of directions (in radians) in which the blade can point
        /// </summary>
        public static float LeftAngleLimit { get; set; } = 0.2f;

        /// <summary>
        /// The right side of the range of directions (in radians) in which the blade can point
        /// </summary>
        public static float RightAngleLimit { get; set; } = 0.2f;

        /// <summary>
        /// How large each wave of blades swaying is
        /// </summary>
        public static float SwayWaveSize { get; set; } = 2f;

        /// <summary>
        /// How violently the grass sways in the wind
        /// </summary>
        public static float SwayForce { get; set; } = 4f;

        Vector2 direction;

        void Awake()
        {
            float angle = Random.Range(RightAngleLimit * -1f, LeftAngleLimit) + Mathf.PI / 2f;
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * WheatDirectionForce;

            Physics2D.IgnoreLayerCollision(6, 6, true);
        }

        private void FixedUpdate()
        {
            bladeRigidbody.AddForce(direction);
            bladeRigidbody.AddForce(new Vector2(Mathf.Sin((transform.position.x + Time.fixedTime) / SwayWaveSize) * SwayForce, 0));// + transform.position.y
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.transform.gameObject.tag == "HarvesterBlade"
                || collision.transform.gameObject.tag == "Player")
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                if (collision.collider.transform.gameObject.layer == 6)
                {
                    Physics2D.IgnoreCollision(collision.collider, collision.otherCollider, true);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.gameObject.tag == "HarvesterBlade"
                || collision.transform.gameObject.tag == "Player")
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}