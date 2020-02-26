using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ResetIfLost : MonoBehaviour
{
    private new Rigidbody rigidbody = null;
    private Vector3 initialPosition = Vector3.zero;

    protected virtual void Awake()
    {
        TryGetComponent<Rigidbody>(out rigidbody);

        initialPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (transform.position.y < -10)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.position = initialPosition;
        }
    }
}
