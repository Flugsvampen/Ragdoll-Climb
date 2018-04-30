﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinScout : MonoBehaviour
{
    Penguin penguin;

	void Start ()
    {
        penguin = transform.parent.GetComponent<Penguin>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (penguin.state == Penguin.PenguinStates.Scout && other.tag == "Player")
        {
            penguin.PrepareLaunch(other.transform);
        }
    }
}
