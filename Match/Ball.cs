using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    [HideInInspector]
    public Transform dribbler, kicker;
    [HideInInspector]
    public float kickCompleteWaitTime = 0.1f;
   // [HideInInspector]
    public bool beingKicked;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (beingKicked && rb.velocity.magnitude == 0)
            beingKicked = false;
        if (dribbler != null)
        {
            Vector3 dribblePos;
            if (dribbler.GetComponent<PlayerControl>().anim_mov > 2.5f)
                dribblePos = dribbler.transform.position + dribbler.transform.forward * 1.5f;
            else
                dribblePos = dribbler.transform.position + dribbler.transform.forward * 1f;
            dribblePos.y = transform.position.y;
            transform.position = dribblePos;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            dribbler = collision.transform;
            dribbler.GetComponent<PlayerControl>().autoMove = false;
            GetComponent<Rigidbody>().freezeRotation = true;
            Physics.IgnoreCollision(GetComponent<Collider>(), dribbler.GetComponent<Collider>());
        }
        if (collision.gameObject.tag != "Ground" && beingKicked)
        {
            beingKicked = false;
            if (collision.transform.name == "Goalkeeper AI")
            {
                print("saved");
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezePosition;
                rb.constraints = RigidbodyConstraints.None;
                collision.transform.GetComponent<GoalkeeperAI>().ballIsMine = true;
                collision.transform.GetComponent<GoalkeeperAI>().StartCoroutine(collision.transform.GetComponent<GoalkeeperAI>().ThrowBallAnim());
            } else if (collision.transform.tag == "Goal")
            {
                print("Goal! With a velocity of " + rb.velocity.magnitude * 2.23694f + " mph");
            }
        }
        
        //if (collision.gameObject.tag == "Goal")
        //    print(string.Format ("Scored with velocity of: {0} mph", rb.velocity.magnitude * 2.23694f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name == "Goalkeeper AI" && dribbler == null && beingKicked)
        {
          other.transform.parent.GetComponent<GoalkeeperAI>().StartCoroutine(other.transform.parent.GetComponent<GoalkeeperAI>().Save (0, transform.position));
        }
    }

    public void Kick (float power, Vector3 direction)
    {
        dribbler = null;

        float xDis = Mathf.Abs (direction.z - transform.position.z);
        float yDis = direction.y - this.transform.position.y;
        float ang = Mathf.Atan2(yDis, xDis) * Mathf.Rad2Deg;
        print("x: " + xDis + " and y: " + direction.y);

        GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);
        beingKicked = true;
        StartCoroutine(KickComplete());
    }
    public IEnumerator KickComplete ()
    {
        yield return new WaitForSeconds(kickCompleteWaitTime);
        Physics.IgnoreCollision(GetComponent<Collider>(), kicker.GetComponent<Collider>(), false);
        kicker.GetComponent<PlayerControl>().kickStarted = false;
    }
}
