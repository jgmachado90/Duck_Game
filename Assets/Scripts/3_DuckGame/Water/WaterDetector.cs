using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour {

    public bool splashing;

    void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.GetComponent<Rigidbody2D>() != null)
        {
            Water water = GetComponentInParent<Water>();
            Rigidbody2D hitRb = Hit.GetComponent<Rigidbody2D>();
            FloatComponent floatable = Hit.GetComponentInParent<FloatComponent>();

            water.Splash(transform.position.x, hitRb.velocity.y * hitRb.mass / 40f);

            if (hitRb.velocity.y < -0.5f || hitRb.velocity.y > 0.5f)
            {
                StartCoroutine(SplashingTime(water, hitRb));
            }

            
            if (floatable == null) return;
            floatable.floating = true;
            floatable.myWater = water;
        }
    }

    public IEnumerator SplashingTime(Water water, Rigidbody2D rb)
    {
        if (!splashing)
        {
            splashing = true;
            BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
            //Vector3 splashPos = new Vector3(transform.position.x, transform.position.y + (myCollider.bounds.size.y * 0.5f), transform.position.z);
            if (rb.transform.position.y > transform.position.y + (myCollider.bounds.size.y * 0.5f))
            {

                Vector3 splashPos = new Vector3(transform.position.x, water.GetYPos(transform.position.x) + rb.velocity.y * 0.01f, transform.position.z);
                water.InstantiateSplash(splashPos);
            }
            yield return new WaitForSeconds(1f);
            splashing = false;
        }
    }

}
