using UnityEngine;

namespace VRAssembly
{
    /// <summary>
    /// Helper for rigidbodies within the scene
    /// Cache the starting position of the attached transform and reset to it if we fall below a certain y level (off map)
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ResetIfLost : MonoBehaviour
    {
        private new Rigidbody rigidbody = null;
        private Vector3 initialPosition = Vector3.zero;

        /// <summary>
        /// The lowest Y level that the attached transform can go to before resetting
        /// </summary>
        private const float MinY = -10.0f;

        /// <summary>
        /// Cache the attached transforms starting position and retrieve relevant components
        /// </summary>
        protected virtual void Awake()
        {
            TryGetComponent<Rigidbody>(out rigidbody);

            initialPosition = transform.position;
        }

        /// <summary>
        /// Check the y position of the attached transform and if it is below a certain y level reset to the cached start position 
        /// </summary>
        protected virtual void Update()
        {
            if (transform.position.y < MinY)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                transform.position = initialPosition;
            }
        }
    }
}