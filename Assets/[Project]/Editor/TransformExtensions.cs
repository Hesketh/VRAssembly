using UnityEditor;
using UnityEngine;
using System.Linq;

public static class TransformExtensions
{
    private static Vector3 CopiedPosition = Vector3.zero;
    private static Quaternion CopiedRotation = Quaternion.identity;

    [MenuItem("CONTEXT/Transform/Copy World Space Transform")]
    public static void CopyWorldSpaceTransform(MenuCommand command)
    {
        Transform target = (Transform)command.context;
        CopiedPosition = target.position;
        CopiedRotation = target.rotation;
    }

    [MenuItem("CONTEXT/Transform/Copy World Space Transform using Bounds Center")]
    public static void CopyWorldSpaceTransformUsingBoundsCenter(MenuCommand command)
    {
        Transform target = (Transform)command.context;
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        CopiedPosition = Vector3.zero;

        foreach(Renderer renderer in renderers)
        {
            CopiedPosition += renderer.bounds.center;
        }

        CopiedPosition /= renderers.Length;
        CopiedRotation = target.rotation;
    }

    [MenuItem("CONTEXT/Transform/Paste World Space Transform using Bounds Center")]
    public static void PasteWorldSpaceTransformUsingBoundsCenter(MenuCommand command)
    {
        Transform target = (Transform)command.context;
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        
        Vector3 center = Vector3.zero;
        foreach (Renderer renderer in renderers)
        {
            center += renderer.bounds.center;
        }
        center /= renderers.Length;

        target.position = CopiedPosition - (center - target.position);
        target.rotation = CopiedRotation;
    }

    [MenuItem("CONTEXT/Transform/Paste World Space Transform")]
    public static void PasteWorldSpaceTransform(MenuCommand command)
    {
        Transform target = (Transform)command.context;
        target.position = CopiedPosition;
        target.rotation = CopiedRotation;
    }
}
