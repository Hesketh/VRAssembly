using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Slot : Step
{
    [Header("Type")]
    [SerializeField] private InteractionType partType = null;

    [Header("Time")]
    [SerializeField] private float timeToComplete = 0f;
    [SerializeField] private bool progressResets = false;

    [Header("Rotation Check")]
    [SerializeField] private float tolerance = 0.05f;
    [Space(8)]
    [SerializeField] private bool up = false;
    [SerializeField] private bool right = false;
    [SerializeField] private bool forward = false;
    [Space(8)]
    [SerializeField] private bool upSymmetrical = false;
    [SerializeField] private bool rightSymmetrical = false;
    [SerializeField] private bool forwardSymmetrical = false;

    [Header("Position Check")]
    [SerializeField] private float maxDistance = 0.025f;

    public Action<float> OnProgressChanged;

    private List<InteractionItem> trackedParts = new List<InteractionItem>();
    private float currentTimeToComplete = 0.0f;

    protected void OnTriggerEnter(Collider other)
    {
        InteractionItem part = other.gameObject.GetComponentInParent<InteractionItem>();
        if (part && part.Type == partType)
        {
            trackedParts.Add(part);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        InteractionItem part = other.gameObject.GetComponentInParent<InteractionItem>();
        if (part && part.Type == partType)
        {
            trackedParts.Remove(part);
        }
    }

    protected void Update()
    {
        if (Available)
        {
            foreach (InteractionItem part in trackedParts)
            {
                PerformComparison(part);
            }
        }
    }

    protected virtual void OnValidate()
    {
        GetComponent<Rigidbody>().isKinematic = true;

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.isTrigger = true;
        }
    }

    private void PerformComparison(InteractionItem part)
    {
        Transform other = part.transform;

        Debug.Log(Vector3.Dot(other.forward, transform.forward));

        // If the alignment isnt correct
        if (((Vector3.Distance(other.position, transform.position) > maxDistance)
            || ((up) && 1.0f - (upSymmetrical ? Mathf.Abs(Vector3.Dot(other.up, transform.up)) : Vector3.Dot(other.up, transform.up)) > tolerance))
            || ((right) && 1.0f - (rightSymmetrical ? Mathf.Abs(Vector3.Dot(other.right, transform.right)) : Vector3.Dot(other.right, transform.right)) > tolerance)
            || ((forward) && 1.0f - (forwardSymmetrical ? Mathf.Abs(Vector3.Dot(other.forward, transform.forward)) : Vector3.Dot(other.forward, transform.forward)) > tolerance))
        {
            if (progressResets)
            {
                currentTimeToComplete = 0.0f;
            }

            return;
        }

        if (currentTimeToComplete >= timeToComplete)
        {
            Complete = true;
            part.Consume(transform);
        }

        currentTimeToComplete += Time.deltaTime;
        OnProgressChanged?.Invoke(currentTimeToComplete);
    }
}