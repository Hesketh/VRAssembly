using UnityEngine;
using TMPro;
using System.Collections;
using System;

namespace VRAssembly
{
    /// <summary>
    /// Used to track the amount of time taken from completed the required steps the final requirements
    /// Outputs to any number for TextMeshProUGUI text components
    /// </summary>
    public class Stopwatch : Step
    {
        [SerializeField] private Step[] requirementsToStop = new Step[0];

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI[] outputs = new TextMeshProUGUI[0];

        private float timeElapsed = 0.0f;

        /// <summary>
        /// Register event handler to all requirements to stop
        /// </summary>
        protected override IEnumerator Start()
        {
            foreach (Step step in requirementsToStop)
            {
                step.OnSlotCompleted += OnCompletionRequirementComplete;
            }

            yield return base.Start();
        }

        /// <summary>
        /// Each update if available we need to increment the amount of time spent
        /// We also update the text elements each frame too
        /// </summary>
        protected virtual void Update()
        {
            if (Available)
            {
                timeElapsed += Time.deltaTime;

                TimeSpan time = TimeSpan.FromSeconds(timeElapsed);
                string str = time.ToString(@"hh\:mm\:ss");

                foreach (TextMeshProUGUI output in outputs)
                {
                    output.text = str;
                }
            }
        }

        /// <summary>
        /// Event handler for any of the final steps being complete
        /// We will only mark ourselves aas complete if all the steps we are listening too are done
        /// </summary>
        private void OnCompletionRequirementComplete()
        {
            foreach (Step step in requirementsToStop)
            {
                if (!step.Complete)
                {
                    return;
                }
            }

            Complete = true;
        }
    }
}