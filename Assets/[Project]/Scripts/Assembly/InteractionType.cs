using UnityEngine;

namespace VRAssembly
{
    /// <summary>
    /// Simple class to indicate the type of part (determined by the actual asset and asset name)
    /// Also indicates whether this object is consumed
    /// </summary>
    [CreateAssetMenu(fileName = "Part", menuName = "Assembly/PartType", order = -100)]
    public class InteractionType : ScriptableObject
    {
        [SerializeField] private bool consumable = true;

        /// <summary>
        /// Whether the part is consumable
        /// Consumable is whether it is safe to destroy the part after it has been used to complete a step
        /// For example, a nail is consumable but a hammer is not. A hammer can be used an infinite amount of time, a nail is lost once inserted
        /// </summary>
        public bool Consumable => consumable;
    }
}