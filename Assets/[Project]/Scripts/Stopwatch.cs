using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class Stopwatch : Step
{
    [SerializeField] private Step[] requirementsToStop = new Step[0];
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI[] outputs = new TextMeshProUGUI[0];

    private float timeElapsed = 0.0f;

    protected override IEnumerator Start()
    {
        foreach (Step step in requirementsToStop)
        {
            step.OnSlotCompleted += OnCompletionRequirementComplete;
        }

        yield return base.Start();
    }

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
