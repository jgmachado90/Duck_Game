using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSkill : Skill
{
    public Rigidbody2D rb;


    private void Start()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    /* private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.tag == "AirColumn")
         {
             rb.gravityScale *= -1;
         }
     }

     private void OnTriggerExit2D(Collider2D collision)
     {
         if (collision.tag == "AirColumn")
         {
             rb.gravityScale *= -1;
         }
     }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "AirColumn")
        {
            rb.AddForce(Vector2.up * 3500f * Time.deltaTime, ForceMode2D.Force);
        }
    }
}
