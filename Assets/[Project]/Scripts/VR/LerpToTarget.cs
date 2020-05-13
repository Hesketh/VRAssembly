using UnityEngine;

namespace VRAssembly
{
    /// <summary>
    /// Script that will smoothly and constantly move the attached transform towards a target transform
    /// Used mainly to smooth the PC camera to the VR perspective (leading to a less nausea inducing viewing experience)
    /// </summary>
    public class LerpToTarget : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField] private float lerpSpeed = 5.0f;

        protected virtual void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * lerpSpeed);
        }
    }
}