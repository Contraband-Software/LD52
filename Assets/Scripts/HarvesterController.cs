using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class HarvesterController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Tilemap wheatTileMap;
    [SerializeField] Tile dirtTile;

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
        rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>().y;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        print("balls");
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        print(collision.GetContacts(contacts));
    }
}
