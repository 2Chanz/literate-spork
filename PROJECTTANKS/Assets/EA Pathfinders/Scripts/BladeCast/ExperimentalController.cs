﻿using UnityEngine;
using BladeCast;
public class ExperimentalController : MonoBehaviour {


    public int playerindex = 1;

    // Use this for initialization
    public float TankHP = 1;
    public float maxSpeed = 10;  //public variable for movement speed.
    public Transform tankGraphics;    //variable to represent the "graphics" child object in the editor.
    bool grounded = false;       //bool to check if the sprite is touching the ground.
    public Transform groundCheck;//variable to assign an empty object used for checking the ground. 
    public float fuel;
    public float jumpforce = 30;
    public float bulletPower = 0;
    public float maxbulletpower;
    public float guncooldown = 1;
    public GameObject bullet;
    float basespeed;
    float force = 0.0f;
    float angle = 0.0f;
    public Transform firepoint;
    public LayerMask whatisGround; //Layer settings to adjust which objects in which layers are considered ground.
    BladeCast.BCSender sender; 

    void Start()
    {
        Application.runInBackground = true;
        BladeCast.BCListener listener = GetComponent<BladeCast.BCListener>();
        sender = GetComponent<BladeCast.BCSender>();
        listener.Add("controls", 0, "applycontrols");
        //tankGraphics = tankGraphics.transform.FindChild("Tank");
        bulletPower = 0;
        basespeed = bullet.GetComponent<FireBullet>().bulletspeeds;
    }

    // Update is called once per frame
    void Update()
    {

        if ((fuel > 0) && Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpforce));

            fuel--;
        }

        if (Input.GetMouseButton(0))
        {

            if (bulletPower <= maxbulletpower)
            {
                bulletPower += Time.deltaTime;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            bullet.GetComponent<FireBullet>().bulletspeeds *= bulletPower;
            //FireBullet();
            bullet.GetComponent<FireBullet>().bulletspeeds = basespeed;
            bulletPower = 0;

        }
    }

    void applycontrols(ControllerMessage msg)
    {
        bool s = msg.Payload.GetField("up");
        Debug.Log(msg.Payload);
        Debug.Log(s);
        if (msg.Payload.GetField("up").ToString() == "true")
        {
            Debug.Log("Call move -1");
            Move(-1);
        }
        else {
            Debug.Log("Call move 0");
            Move(0);
        }
    }

    void Move(float move)

    {
            //float move = Input.GetAxis("Horizontal");// Variable used to store the movement double when moving horizontally.

            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        //Debug.Log(move);
    }


    void FireBullet(ControllerMessage msg)
    {
        if (msg.ControllerSource == playerindex)
        {
            GameObject bullets = (GameObject)Instantiate(bullet, firepoint.position, firepoint.rotation);
            //Instantiate(bullet, firepoint.position, firepoint.rotation);
        }
    }
    public void DamageTank()
    {
        TankHP -= 999999;
        if (TankHP <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            DamageTank();
        }
    }
}