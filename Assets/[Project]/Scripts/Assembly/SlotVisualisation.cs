using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Slot), typeof(AudioSource))]
public class SlotVisualisation : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Material unavailableMat = null;
    [SerializeField] private Material availableMat = null;

    [Header("Audio")]
    [SerializeField] private AudioClip completeClip = null;

    private Slot slot = null;
    private Renderer[] renderers = null;
    private AudioSource audioSource = null;

    protected virtual void Awake()
    {
        TryGetComponent(out slot);
        TryGetComponent(out audioSource);

        renderers = GetComponentsInChildren<Renderer>();
    }

    protected virtual void Start()
    {
        slot.OnSlotAvailable += OnSlotAvailable;
        slot.OnSlotCompleted += OnSlotCompleted;
        
        foreach (Renderer renderer in renderers)
        {
            renderer.material = unavailableMat;
        }
    }

    private void OnSlotAvailable()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = availableMat;
        }
    }

    private void OnSlotCompleted()
    {
        audioSource.PlayOneShot(completeClip);

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
