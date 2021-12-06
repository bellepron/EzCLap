using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip boatStack, boatExplosion, boatDrop, confetti, jump, land, diamond, humanPain;

    private void Awake()
    {
        Instance = this;
    }

    public void BoatStack()
    {
        audioSource.PlayOneShot(boatStack, 1);
    }

    public void BoatExplosion()
    {
        audioSource.PlayOneShot(boatExplosion, 1);
    }

    public void BoatDrop()
    {
        audioSource.PlayOneShot(boatDrop, 1);
    }

    public void Confetti()
    {
        audioSource.PlayOneShot(confetti, 1);
    }

    public void Jump()
    {
        audioSource.PlayOneShot(jump, 1);
    }

    public void Land()
    {
        audioSource.PlayOneShot(land, 1);
    }

    public void Diamond()
    {
        audioSource.PlayOneShot(diamond, 0.7f);
    }

    public void Pain()
    {
        audioSource.PlayOneShot(humanPain, 1);
    }
}