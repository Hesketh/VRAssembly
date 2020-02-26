using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Player))]
public class Locomotion : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Vector2 moveBody = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("MoveBody");
    [SerializeField] private float speed = 1.0f;

    private Player player = null;
    private Vector3 movement = Vector3.zero;

    protected virtual void Awake()
    {
        TryGetComponent(out player);
    }

    protected virtual void Start()
    {
        moveBody.onUpdate += MoveBody_OnUpdate;
    }

    protected virtual void Update()
    {
        Quaternion rotation = Quaternion.Euler(0.0f, player.hmdTransform.localRotation.eulerAngles.y, 0.0f);
        transform.Translate(rotation * movement * Time.deltaTime * speed);
    }

    private void MoveBody_OnUpdate(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        movement.x = axis.x;
        movement.z = axis.y;
    }
}
