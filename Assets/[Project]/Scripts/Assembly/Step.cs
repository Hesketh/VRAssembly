using System;
using System.Collections;
using UnityEngine;

public abstract class Step : MonoBehaviour
{
    [SerializeField] private Slot[] requirements = new Slot[0];
    [SerializeField] private bool requireAny = false;

    private bool available = false;
    private bool complete = false;

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

    public Action OnSlotCompleted;
    public Action OnSlotAvailable;

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

    protected virtual void OnDestroy()
    {
        foreach (Slot requirement in requirements)
        {
            requirement.OnSlotCompleted -= OnRequirementCompleted;
        }
    }

    private void OnRequirementCompleted()
    {
        if (!Available && !Complete)
        {
            if (!requireAny)
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
            }

            // We can mark ourself as available
            Available = true;
        }
    }

    [ContextMenu("Complete")]
    private void Debug_Complete()
    {
        if (Available)
        {
            Complete = true;
        }
    }
}
