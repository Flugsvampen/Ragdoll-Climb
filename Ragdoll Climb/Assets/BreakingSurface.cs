﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingSurface : MonoBehaviour
{
    [SerializeField] float breakTime = 5f;
    [SerializeField] float pushOutForce = 100f;
    [SerializeField] GameObject breakingParticleSystems;

    bool isBreaking = false;

    List<CheckGrip> grabbingHands = new List<CheckGrip>();

    ParticleSystem[] particleSystems;

    BreakingPart[] parts;

    soundManager soundManager;


    private void Start()
    {
        parts = GetComponentsInChildren<BreakingPart>();

        foreach (BreakingPart part in parts)
        {
            Instantiate(breakingParticleSystems, part.transform);
        }

        particleSystems = GetComponentsInChildren<ParticleSystem>();

        //soundManager = GameObject.Find("music and sound").GetComponent<soundManager>();
    }


    public void AddGrabbingHand(CheckGrip hand)
    {
        grabbingHands.Add(hand);

        if (!isBreaking)
            StartCoroutine(BreakApart());

        isBreaking = true;
    }


    public void RemoveGrabbingHand(CheckGrip hand)
    {
        grabbingHands.Remove(hand);
    }


    public void ReleaseHands()
    {
        CheckGrip[] handArray = new CheckGrip[grabbingHands.Count];
        handArray = grabbingHands.ToArray();

        foreach (CheckGrip hand in handArray)
        {
            hand.transform.root.GetComponent<PlayerController>().ReleaseGrip(hand.leftHand, false);
        }
    }


    private void PlayPartSystems()
    {
        foreach(ParticleSystem system in particleSystems)
        {
            system.Play();
        }
    }


    IEnumerator BreakApart()
    {
        //soundManager.PlaySoundRandPitch("crack");
        PlayPartSystems();

        yield return new WaitForSeconds(breakTime / 2);

        foreach (CheckGrip hand in grabbingHands)
        {
            hand.transform.root.GetComponent<VibrationManager>().VibrateTimed(0.5f, 0.1f, 9);
        }

        //soundManager.PlaySoundRandPitch("crack");
        PlayPartSystems();

        yield return new WaitForSeconds(breakTime / 2);

        foreach (BreakingPart part in parts)
        {
            part.Break(pushOutForce);

            ReleaseHands();
        }

        //soundManager.PlaySoundRandPitch("break");
        PlayPartSystems();

        yield return null;
    }
}
