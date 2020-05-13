using System;
using System.Collections;
using UnityEngine;

namespace VRAssembly
{
    /// <summary>
    /// A step towards completing a task
    /// How the Step should be completed should be defined in a concrete version of this class
    /// </summary>
    public abstract class Step : MonoBehaviour
    {
        [Tooltip("Any Steps that must be complete before this is made Active"), SerializeField] private Slot[] requirements = new Slot[0];
        [Tooltip("Whether this is ANY or ALL requirements must be fulfilled"), SerializeField] private bool requireAny = false;

        private bool available = false;
        private bool complete = false;

        /// <summary>
        /// Indicates that this step is finished
        /// Therefore any other steps using this as a requirement know that their requirement has been fulfilled
        /// </summary>
        public bool Complete
        {
            get => complete;

            protected set
            {
                complete = value;

                if (Complete)
                {
                    OnSlotCompleted?.Invoke();
                }
            }
        }

        /// <summary>
        /// Indicates that the step is able to be completed
        /// If the step is Complete, then you wouldn't be able to complete it again
        /// </summary>
        public bool Available
        {
            get => available && !Complete;

            protected set
            {
                available = value;

                if (Available)
                {
                    OnSlotAvailable?.Invoke();
                }
            }
        }

        /// <summary>
        /// Event Invoked anytime when the step is set to complete
        /// </summary>
        public Action OnSlotCompleted;

        /// <summary>
        /// Event invoked anytime when the step is set to available
        /// </summary>
        public Action OnSlotAvailable;

        /// <summary>
        /// At startup, we hook up to events in any required steps so that we can calculate whether this step is available
        /// </summary>
        protected virtual IEnumerator Start()
        {
            foreach (Slot requirement in requirements)
            {
                requirement.OnSlotCompleted += OnRequirementCompleted;
            }

            yield return new WaitForSeconds(1.0f);

            // If we don't have any requirements, we are a base Slot.
            Available = (requirements.Length == 0);
        }

        /// <summary>
        /// When cleaning up this gameobject, we also want to remove our event listeners within the required steps
        /// Ensures that we can add/remove objects at runtime if need be
        /// </summary>
        protected virtual void OnDestroy()
        {
            foreach (Slot requirement in requirements)
            {
                requirement.OnSlotCompleted -= OnRequirementCompleted;
            }
        }

        /// <summary>
        /// Event handler for whenever any step is completed
        /// Instead of internally tracking completion step of all requirements in here
        /// Every time we complete any step, we evaluate all of their Completion status
        /// To check whether all out prerequisites have been met
        /// </summary>
        private void OnRequirementCompleted()
        {
            if (!Available && !Complete && !requireAny)
            {
                foreach (Slot requirement in requirements)
                {
                    // One of our requirements is not done
                    if (!requirement.Complete)
                    {
                        Available = false;
                        return;
                    }
                }

                // We can mark our self as available
                Available = true;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Debug function for calling within the Unity Editor
        /// Allows us to simulate completing the step (abiding by standard available rules)
        /// </summary>
        [ContextMenu("Complete")]
        private void Debug_Complete()
        {
            if (Available)
            {
                Complete = true;
            }
        }

        /// <summary>
        /// Debug function for calling within the Unity Editor
        /// Allows us to simulate completing the step (ignoring availability rules)
        /// </summary>
        [ContextMenu("Complete (Force)")]
        private void Debug_ForceComplete()
        {
            Available = true;
            Complete = true;
        }
#endif
    }

}