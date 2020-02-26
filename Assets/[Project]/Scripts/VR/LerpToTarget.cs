using UnityEngine;

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
