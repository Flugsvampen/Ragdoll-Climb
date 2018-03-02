﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
   public bool isStunned;
    float timer;

    public ParticleSystem stars;


    void Update()
    { 
        //If the player activate the stun and start the timer.
        if(isStunned)
        {
            timer -= Time.deltaTime;

            //Remove stun effect from the player.
            if (timer <= 0 && isStunned)
            {
                UnStun();
            }
        }
    }

    public void Stun(float stunTime)
    {
        //If the player is not stunned. Stun the player.
        if(!isStunned)
        {
            //Add time to timer.
            timer = stunTime;

            //Start playing particle effect.
            stars.Play();

            isStunned = true;

            //Remove player mobility.
            GetComponent<PlayerController>().canMove = false;
            GetComponent<PlayerController>().ReleaseGrip(true, false);
            GetComponent<PlayerController>().ReleaseGrip(false, false);
        }
    }

    public void UnStun()
    {
        //If the stun time is 0 or less. Remove stun effect.
        if (!GetComponent<FreezePlayerPowerUp>().isFrozen)
        {
            isStunned = false;
            //Start playing particle effect.
            stars.Stop();
            //Give player back mobility.
            GetComponent<PlayerController>().canMove = true;
        }
    }

}
