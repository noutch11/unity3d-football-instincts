using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
	public float speedStat, turnSpeed, animMovAccel, currentShotPower, stamina = 100, maxShotPower;
    Vector3 kickDir;
    [HideInInspector]
    public bool kickStarted, kicking;

    Vector2 input;
    float angle;
    Quaternion targetRot;

    [HideInInspector]
    public bool autoMove;

    public Ball ball;

	public Animator anim;
    [HideInInspector]
	public float anim_mov;

    public Image staminaBar;

    public GoalkeeperAI gk;

	// Use this for initialization
	void Start () {
        ball = GameObject.FindObjectOfType<Ball>();
        Physics.IgnoreCollision(GetComponent<Collider>(), gk.GetComponent<Collider>());

    }
	
	// Update is called once per frame
	void Update () {
        StaminaControl();
        ShotPowerControl();
        if (Input.GetMouseButton (0) && ball.dribbler == this.transform && !kicking)
        {
            kickStarted = true;
            currentShotPower++;
        }

        foreach (Transform c in transform)
        {
            if (c.GetComponent<Camera>() != null)
            {
                float y = c.eulerAngles.y; float z = c.eulerAngles.z;
                Vector3 t = c.eulerAngles;t.x = Camera.main.transform.eulerAngles.x;

                c.eulerAngles = new Vector3(t.x, y, z);
            }
        }

        if (kicking)
            return;

        #region movement
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        List<Collider> cols = new List<Collider>(Physics.OverlapSphere(ball.transform.position, 3));
        if (cols.Contains(this.GetComponent<Collider>()) && ball.dribbler == null && input != Vector2.zero)
        {
            #region fov checker
            Vector3 frontFOV = transform.Find("Front FOV").GetComponent<Camera>().WorldToViewportPoint(ball.transform.position);
            bool frontFOVBool = frontFOV.z > 0 && frontFOV.x > 0 && frontFOV.x < 1 && frontFOV.y > 0 && frontFOV.y < 2.6f;
            Vector3 rightFOV = transform.Find("Right FOV").GetComponent<Camera>().WorldToViewportPoint(ball.transform.position);
            bool rightFOVBool = rightFOV.z > 0 && rightFOV.x > 0 && rightFOV.x < 1 && rightFOV.y > 0 && rightFOV.y < 2.6f;
            Vector3 backFOV = transform.Find("Back FOV").GetComponent<Camera>().WorldToViewportPoint(ball.transform.position);
            bool backFOVBool = backFOV.z > 0 && backFOV.x > 0 && backFOV.x < 1 && backFOV.y > 0 && backFOV.y < 2.6f;
            Vector3 leftFOV = transform.Find("Left FOV").GetComponent<Camera>().WorldToViewportPoint(ball.transform.position);
            bool leftFOVBool = leftFOV.z > 0 && leftFOV.x > 0 && leftFOV.x < 1 && leftFOV.y > 0 && leftFOV.y < 2.6f;
            #endregion
            bool lookAid = false;

            if ((input.y == 1 && frontFOVBool) || (input.y == -1 && backFOVBool) || (input.x == 1 && rightFOVBool) || (input.x == -1 && leftFOVBool))
                lookAid = true;

            if (lookAid)
            {
                transform.LookAt(ball.transform);
                Vector3 tempRot = transform.eulerAngles;
                tempRot.z = 0; tempRot.x = 0;
                transform.eulerAngles = tempRot;
            }
        }

        if (input == Vector2.zero) {
            anim_mov = Mathf.Lerp(anim_mov, 0, animMovAccel * Time.deltaTime);
            targetRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            if (!Input.GetMouseButton(1))
                transform.rotation = Quaternion.Slerp (transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            stamina += 0.75f;
            anim.SetFloat("Movement", anim_mov);
            return;
        }

        CalculateDir();
        Move();
        #endregion
    }
    void CalculateDir() {
        angle = Mathf.Atan2(input.x, input.y);
        angle *= Mathf.Rad2Deg;
        angle += Camera.main.transform.eulerAngles.y;

        targetRot = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }
    void Move() {
        //if we're holdind the "walk" key we'll walk
        if (Input.GetAxis("Walk") != 0)
        {
            if (anim_mov == 0)
                anim_mov = Mathf.Lerp(anim_mov, 1, animMovAccel * Time.deltaTime * 10);
            else
                anim_mov = Mathf.Lerp(anim_mov, 1, animMovAccel * Time.deltaTime);
            stamina += 0.5f;
        }
        //if we're holding the "srpint" key we'll sprint
        else if (Input.GetAxis("Sprint") != 0)
        {
            if (stamina > 0)
            {
                anim_mov = Mathf.Lerp(anim_mov, 3, animMovAccel * Time.deltaTime);
                stamina -= 0.05f;
            }
            else
            {
                anim_mov = Mathf.Lerp(anim_mov, 2, animMovAccel * Time.deltaTime);
            }
        }
        //if we aren't holding the "walk" or "sprint" key, then we'll jog
        else
        {
            anim_mov = Mathf.Lerp(anim_mov, 2, animMovAccel * Time.deltaTime);
            stamina += 0.25f;
        }

        anim.SetFloat("Movement", anim_mov);
     /*   if (ball.dribbler == this.transform)
            anim.SetBool("Dribbling", true);
        else
            anim.SetBool("Dribbling", false); */
    }

    private void FixedUpdate()
    {

        if (Input.GetMouseButtonUp(0) && kickStarted && !kicking)
        {
            Ray ray = Camera.main.ScreenPointToRay(GameObject.Find("Canvas/Crosshair").transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                kickStarted = false;
                kicking = true;
                if (currentShotPower <= 10)
                    currentShotPower = maxShotPower - currentShotPower;

                ball.kicker = this.transform;
                #region fov checker
                Vector3 frontFOV = transform.Find("Front FOV").GetComponent<Camera>().WorldToViewportPoint(hit.point);
                bool frontFOVBool = frontFOV.z > 0 && frontFOV.x > 0 && frontFOV.x < 1 && frontFOV.y > 0 && frontFOV.y < 1;
                Vector3 rightFOV = transform.Find("Right FOV").GetComponent<Camera>().WorldToViewportPoint(hit.point);
                bool rightFOVBool = rightFOV.z > 0 && rightFOV.x > 0 && rightFOV.x < 1 && rightFOV.y > 0 && rightFOV.y < 1;
                Vector3 backFOV = transform.Find("Back FOV").GetComponent<Camera>().WorldToViewportPoint(hit.point);
                bool backFOVBool = backFOV.z > 0 && backFOV.x > 0 && backFOV.x < 1 && backFOV.y > 0 && backFOV.y < 1;
                Vector3 leftFOV = transform.Find("Left FOV").GetComponent<Camera>().WorldToViewportPoint(hit.point);
                bool leftFOVBool = leftFOV.z > 0 && leftFOV.x > 0 && leftFOV.x < 1 && leftFOV.y > 0 && leftFOV.y < 1;

                /*      if (backFOVBool)
                          ball.kickCompleteWaitTime = 0.3f;
                      else
                          ball.kickCompleteWaitTime = 0.1f; */
                #endregion

                if (hit.transform.tag == "Ground")
                {
                    ball.rb.constraints = RigidbodyConstraints.FreezePosition;
                    Vector3 temp = ball.transform.position;
                    kickDir = hit.point - ball.transform.position;
                    ball.rb.constraints = RigidbodyConstraints.None;
                    kickDir.y = 0;

                    float f = Mathf.Abs(kickDir.normalized.magnitude * currentShotPower);
                    float d = Mathf.Abs(gk.transform.position.z - temp.z);
                    float m = ball.rb.mass;
                    float t = Mathf.Sqrt(d / (0.5f * (f / m))); ;
                    gk.StartCoroutine(gk.MoveToSaveDest(t, hit.point));

                    ball.Kick(15, kickDir.normalized);
                    kicking = false;
                }
                else
                {
                    kickDir = hit.point;
                    if (backFOVBool || rightFOVBool || leftFOVBool)
                        transform.LookAt(new Vector3(kickDir.x, transform.position.y, kickDir.z));
                    float ang = Mathf.Atan2(input.x, input.y);
                    ang *= Mathf.Rad2Deg;
                    if (currentShotPower > 50)
                        anim.SetTrigger("Hard Shot");
                    else
                        anim.SetTrigger("Soft Shot");
                    if (ang >= 0)
                        anim.SetTrigger("Shooting Right");
                    else
                        anim.SetTrigger("Shooting Left");
                }
            }
            else
                print("no hit");//currentShotPower = 0;
        }
    }

    public void KickAnim ()
    {
        ball.rb.constraints = RigidbodyConstraints.FreezePosition;
        Vector3 temp = ball.transform.position;
        Vector3 hitPt = kickDir;
        kickDir -= ball.transform.position;
        ball.rb.constraints = RigidbodyConstraints.None;
        if (currentShotPower < 30) kickDir.y *= 3;

        float f = Mathf.Abs(kickDir.normalized.magnitude * currentShotPower);
        float d = Mathf.Abs(gk.transform.position.z - temp.z);
        float m = ball.rb.mass;
        float t = Mathf.Sqrt(d / (0.5f * (f / m)));
        gk.StartCoroutine(gk.MoveToSaveDest(t, hitPt));

        ball.Kick(currentShotPower, kickDir.normalized);
        kicking = false;
        currentShotPower = 0;
    }
    void StaminaControl ()
    {
        if (Input.GetKeyDown("k"))
            stamina = 100;

        if (stamina > 100)
            stamina = 100;
        if (stamina < 0)
            stamina = 0;
        staminaBar.fillAmount = stamina / 100;
    }
    void ShotPowerControl()
    {
        if (currentShotPower > maxShotPower)
            currentShotPower = maxShotPower;
    }

}
