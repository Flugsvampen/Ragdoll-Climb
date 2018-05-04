﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float growthSpeed = 0.1f;
    public float stunTime = 2f;
    [SerializeField] GameObject icicleShatterEffect;

    internal bool instantiated = false;

    bool growing = true;
    bool firstColl = false;

    float targetScale;
    float minScale = 0;
    float scale;
    
    Transform bottomObj;

    Vector3 startPos;
    Vector3 particleOffset = new Vector3(0f, -1f, 0f);

    soundManager soundManager;

    Rigidbody rb;


	void Start ()
    {
        startPos = transform.localPosition;

        targetScale = transform.localScale.x;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        scale = transform.localScale.x;

        soundManager = GameObject.Find("music and sound").GetComponent<soundManager>();

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        bottomObj = GameObject.FindGameObjectWithTag("BottomObj").transform;
    }
	
	void Update ()
    {
		if (growing)
        {
            scale += growthSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale >= targetScale)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                growing = false;
            }
        }

        if (transform.position.y <= bottomObj.position.y)
            Destroy(gameObject);
	}
    


    private void OnCollisionEnter(Collision other)
    {
        if (!firstColl)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider.gameObject.GetComponent<Collider>());

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            firstColl = true;

            print("Igmore coll: " + other.gameObject.name);
        }
        else if (!growing)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.root.GetComponent<PlayerStun>().Stun(stunTime);
                FindObjectOfType<soundManager>().PlaySound("icicle"); // sound on player hit
            }

            Instantiate(icicleShatterEffect, transform.position + particleOffset, Quaternion.identity);
            // FindObjectOfType<soundManager>().PlaySound("icicle");  // sound  on a hit

            print("Collision: " + other.gameObject.name);

            if (instantiated)
                Destroy(gameObject);
            else
            {
                scale = minScale;
                transform.localScale = new Vector3(minScale, minScale, minScale);
                transform.localPosition = startPos;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                rb.useGravity = false;
                growing = true;
            }
        }
    }
}
