using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody), typeof(Throwable))]
public class InteractionItem : MonoBehaviour
{
    [SerializeField] private InteractionType type = null;
    
    public InteractionType Type => type;
    
    private new Rigidbody rigidbody = null;
    private Throwable throwable = null;
    private Collider[] colliders = null;

    protected virtual void Awake()
    {
        TryGetComponent(out rigidbody);
        TryGetComponent(out throwable);

        colliders = GetComponentsInChildren<Collider>();
    }

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

    private IEnumerator LerpToPosition(Transform target)
    {
        const float duration = 0.2f;
        float time = 0.0f;

        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        while(time < duration)
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
