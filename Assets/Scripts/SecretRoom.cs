using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    [SerializeField] Transform frontWall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if(player != null)
        {
            OpenRoom(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            OpenRoom(false);
        }
    }


    private void OpenRoom(bool value)
    {
        frontWall.gameObject.SetActive(!value);
    }
}
