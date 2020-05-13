using UnityEngine;

namespace VRAssembly
{
    /// <summary>
    /// Component to provide a visual response/state to the Slot that it is attached too
    /// </summary>
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

        /// <summary>
        /// Obtain necessary components
        /// </summary>
        protected virtual void Awake()
        {
            TryGetComponent(out slot);
            TryGetComponent(out audioSource);

            renderers = GetComponentsInChildren<Renderer>();
        }

        /// <summary>
        /// Registers to all events and set default state
        /// </summary>
        protected virtual void Start()
        {
            slot.OnSlotAvailable += OnSlotAvailable;
            slot.OnSlotCompleted += OnSlotCompleted;

            foreach (Renderer renderer in renderers)
            {
                renderer.material = unavailableMat;
            }
        }

        /// <summary>
        /// Event handler for a Slot becoming available for completion
        /// Sets the materials to the one to represent ready to complete steps
        /// </summary>
        private void OnSlotAvailable()
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = availableMat;
            }
        }

        /// <summary>
        /// Event handler for a Slot being completed
        /// Plays an audio cue and changes to the completed material
        /// </summary>
        private void OnSlotCompleted()
        {
            audioSource.PlayOneShot(completeClip);

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }
}