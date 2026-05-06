using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperAI : MonoBehaviour {
    public Animator anim;
    public Ball ball;
    public float t;

    public Transform[] cubesPosRef;
    Vector3 curCubeRefPos;

    public Transform rightHand;

    [HideInInspector]
    public bool ballIsMine;

    private void Start()
    {
        curCubeRefPos = cubesPosRef[0].position;
    }
    private void Update()
    {
        //if (transform.position.y > 0 && !anim.GetCurrentAnimatorStateInfo (0).IsName("Diving Save")) transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if (ball.dribbler != null)
            transform.position = Vector3.Lerp(transform.position, new Vector3(FindNextPos(cubesPosRef).x, transform.position.y, -51), t);
        else
        {
        /*    transform.position = Vector3.Lerp(transform.position, new Vector3 (ball.transform.position.x, transform.position.y, transform.position.z), t);
            Vector3 temp = transform.position;
            temp.x = Mathf.Clamp(temp.x, -4, 4);
            transform.position = temp; */
        }
            
     //   if (Mathf.Abs(transform.position.x - curCubeRefPos.x) > .2f)
       //     anim.SetTrigger("Sidestep");

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && Mathf.Abs(ball.transform.position.z) < Mathf.Abs(transform.position.z) && Mathf.Abs (ball.transform.position.z - transform.position.z) > 3 || ball.dribbler != null)
        {
            transform.LookAt(new Vector3(ball.transform.position.x, 0, ball.transform.position.z));
        }
        else if (Mathf.Abs(ball.transform.position.z) > Mathf.Abs(transform.position.z) || Mathf.Abs(ball.transform.position.z - transform.position.z) > 3)
            transform.eulerAngles = Vector3.zero;

        if (ballIsMine)
        {
            ball.transform.position = rightHand.position;
            ball.dribbler = null;
            ball.rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    Vector3 FindNextPos (Transform[] possiblePos)
    {
        Transform tMin = null;
        Vector3 nextPos = Vector3.zero;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = ball.transform.position;
        foreach (Transform t in possiblePos)
        {
            float dist = Mathf.Abs((currentPos.x - t.position.x));
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        nextPos = tMin.position;
        if (tMin == possiblePos[1])
        {
            if (Mathf.Abs(ball.transform.position.x) > Mathf.Abs(nextPos.x))
            {
                if ((Mathf.Abs(ball.transform.position.x) - Mathf.Abs(nextPos.x) >= 10))
                    nextPos -= 4 * Vector3.right;
                else
                    nextPos -= 6.5f * Vector3.right;
            }
            else
            {
                tMin = possiblePos[4];
                nextPos = tMin.position;
            }
        }
        else if (tMin == possiblePos[2])
        {
            if (Mathf.Abs(ball.transform.position.x) > Mathf.Abs(nextPos.x))
            {
                if ((Mathf.Abs(ball.transform.position.x) - Mathf.Abs(nextPos.x) >= 10))
                    nextPos += 4 * Vector3.right;
                else
                    nextPos += 6.5f * Vector3.right;
            }
            else
            {
                tMin = possiblePos[3];
                nextPos = tMin.position;
            }
        }
        curCubeRefPos = nextPos;
        return nextPos;
    }

    public IEnumerator Save (float waitTime, Vector3 dest)
    {
        yield return new WaitForSeconds(waitTime);
            if (Mathf.Abs (dest.x - transform.position.x) > .1f)
            {
                anim.SetTrigger("Dive");
                if (dest.x - transform.position.x < -.1f)
                    anim.SetTrigger("Left");
                else
                    anim.SetTrigger("Right");
            }
            else
            {
        //    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
         //       yield break;
                Vector3 temp = transform.position;
                temp.x = ball.transform.position.x;
                transform.position = temp;
                anim.SetTrigger("Catch");
            }
        //   GetComponent<Rigidbody>().AddForce(new Vector3(dest.x, dest.y, 0) * 100, ForceMode.Impulse);
        transform.position = new Vector3(ball.transform.position.x, transform.position.y, transform.position.z);
    }
    void AutoAdjustDive ()
    {
       transform.position = new Vector3(ball.transform.position.x, transform.position.y, transform.position.z);
    }

    public IEnumerator MoveToSaveDest (float timeToWait, Vector3 dest)
    {
        Vector3 startPos = transform.position;
        dest.x = Mathf.Clamp(dest.x, -4, 4);
        float t = 0f;
        while (t < timeToWait)
        {
            transform.position = Vector3.Lerp(startPos, new Vector3 (dest.x, startPos.y, startPos.z), t);
            t += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ThrowBallAnim ()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Throw");
    }
    void ThrowBall ()
    {
        ballIsMine = false;
        ball.rb.constraints = RigidbodyConstraints.None;
        ball.rb.AddForce((GameObject.FindWithTag("Player").transform.position - ball.transform.position).normalized * 30, ForceMode.Impulse);
    }
}
