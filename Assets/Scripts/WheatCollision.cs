using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        print("balls2");
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        print(collision.GetContacts(contacts));
    }
}
