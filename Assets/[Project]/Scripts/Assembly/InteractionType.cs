using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "Assembly/PartType", order = -100)]
public class InteractionType : ScriptableObject 
{
    [SerializeField] private bool consumable = true;

    public bool Consumable => consumable;
}
