using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool slamming = false;
    private bool grounded = false;
    private bool jumping = false;

    private int score = 0;
    private float scoreTimer = 1;   //How long to walk before 1 point added to score

    private float jumpHoldTimer = .4f;

    private float jumpPower = 1;
    private float jumpSubtractHeld = .1f;
    private float jumpSubtractRelease = .2f;
    private float maxFall = -2;

    private float forwardSpeed = .01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + forwardSpeed, transform.position.y, transform.position.z);

        if (Input.GetMouseButtonDown(0))
        {
            if (grounded)   //Start jump off ground
            {

            } else if (jumping) //Hold jump to get higher
            {

            } else if (!slamming)   //Slam down from the air. Prevent multi-slam from multi-clicks
            {

            }
        } else
        {
            if (!grounded)  //Subtract from jump until maxFall speed
            {

            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collectible")
        {
            score++;
        } else if (other.tag == "Hurt")
        {
            Die();
        }
    }
}
