using System.Collections.Generic;
using UnityEngine;

namespace VRAssembly
{
    /// <summary>
    /// An extension of a Step that must be completed that makes up the project
    /// A Slot is a visual Step where an InteractionItem must be placed at the correct angle and position to complete
    /// </summary>
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

        [Header("Position Check")] [SerializeField]
        private float maxDistance = 0.025f;

        private List<InteractionItem> trackedParts = new List<InteractionItem>();
        private float currentTimeToComplete = 0.0f;

        /// <summary>
        /// When anything moves into the trigger for this slot, we need to add it from our collection of InteractionItems that we maintain
        /// That we process in the update if need be
        /// </summary>
        /// <param name="other">The Collider that is intersecting this slot</param>
        protected void OnTriggerEnter(Collider other)
        {
            InteractionItem part = other.gameObject.GetComponentInParent<InteractionItem>();
            if (part && part.Type == partType)
            {
                trackedParts.Add(part);
            }
        }

        /// <summary>
        /// When anything moves out of the trigger for this slot, we need to remove it from our collection of InteractionItems that we maintain
        /// </summary>
        /// <param name="other">The Collider that was intersecting this slot</param>
        protected void OnTriggerExit(Collider other)
        {
            InteractionItem part = other.gameObject.GetComponentInParent<InteractionItem>();
            if (part && part.Type == partType)
            {
                trackedParts.Remove(part);
            }
        }

        /// <summary>
        /// Each update if we are available to be completed we want to perform comparisons with all parts
        /// that are currently intersecting this slot 
        /// </summary>
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

        /// <summary>
        /// Perform some validation checks on components attached to the same gameobject
        /// We have certain assumptions about these components that must be met for things to work as intended
        /// </summary>
        protected virtual void OnValidate()
        {
            // Just for sanity, ensure that the rigidbody is kinematic as this is what the Slot expects
            // Slots should not be items that move around or do anything with real physic interactions
            GetComponent<Rigidbody>().isKinematic = true;

            // We also expect all Slots to be triggers
            // So for sanity, ensure they all are
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.isTrigger = true;
            }
        }

        /// <summary>
        /// Compares the position and rotation of a part with this slot
        /// If they are close enough together, and close enough in terms of matching rotation
        /// Then if they are held in that position for long enough
        /// We can mark the slot as filled / complete
        /// </summary>
        /// <param name="part">The part that we are performing our comparison with</param>
        private void PerformComparison(InteractionItem part)
        {
            Transform other = part.transform;

            Debug.Log(Vector3.Dot(other.forward, transform.forward));

            // If the alignment isnt correct
            if (((Vector3.Distance(other.position, transform.position) > maxDistance)
                 || ((up) && 1.0f - (upSymmetrical
                     ? Mathf.Abs(Vector3.Dot(other.up, transform.up))
                     : Vector3.Dot(other.up, transform.up)) > tolerance))
                || ((right) && 1.0f - (rightSymmetrical
                    ? Mathf.Abs(Vector3.Dot(other.right, transform.right))
                    : Vector3.Dot(other.right, transform.right)) > tolerance)
                || ((forward) &&
                    1.0f - (forwardSymmetrical
                        ? Mathf.Abs(Vector3.Dot(other.forward, transform.forward))
                        : Vector3.Dot(other.forward, transform.forward)) > tolerance))
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
        }
    }
}