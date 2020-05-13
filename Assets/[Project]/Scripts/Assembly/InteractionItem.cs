using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace VRAssembly
{
    /// <summary>
    /// An interaction item is an item that is held by the VR player and should be used to complete a Slot
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Throwable))]
    public class InteractionItem : MonoBehaviour
    {
        [SerializeField] private InteractionType type = null;

        public InteractionType Type => type;

        private new Rigidbody rigidbody = null;
        private Throwable throwable = null;
        private Collider[] colliders = null;

        /// <summary>
        /// Retrieve relevant components that are attached to the gameObject
        /// </summary>
        protected virtual void Awake()
        {
            TryGetComponent(out rigidbody);
            TryGetComponent(out throwable);

            colliders = GetComponentsInChildren<Collider>();
        }
        
        /// <summary>
        /// If we are instructed to be consumed, we must start the routine to move towards the target consumption point and detach ourselves from the VR Player
        /// After calling this we don't expect any further User interaction with the item as it is due to no longer be used
        /// </summary>
        /// <param name="position">The target transform</param>
        public void Consume(Transform position)
        {
            if (type.Consumable)
            {
                Debug.Log("Consuming: " + gameObject.name);

                throwable?.interactable?.attachedToHand?.DetachObject(gameObject);

                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }

                rigidbody.isKinematic = true;

                StartCoroutine(LerpToPosition(position));
            }
        }

        /// <summary>
        /// Simple routine to move towards the target position ready to be consumed
        /// </summary>
        /// <param name="target">The target transform we will be consumed at</param>
        private IEnumerator LerpToPosition(Transform target)
        {
            const float duration = 0.2f;
            float time = 0.0f;

            Vector3 initialPosition = transform.position;
            Quaternion initialRotation = transform.rotation;

            while (time < duration)
            {
                float progress = time / duration;

                transform.position = Vector3.Lerp(initialPosition, target.position, progress);
                transform.rotation = Quaternion.Slerp(initialRotation, target.rotation, progress);

                yield return new WaitForEndOfFrame();

                time += Time.deltaTime;
            }

            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}