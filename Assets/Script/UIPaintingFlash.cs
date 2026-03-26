using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPaintingFlash : MonoBehaviour
{
    [System.Serializable]
    public class FlashStep
    {
        public Image painting;
        public Color color;
        public float duration;
    }

    public FlashStep[] sequence;
    public float resetDelay = 0.1f;
    public float loopDelay = 1.5f; //  delay after full sequence

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        while (true)
        {
            foreach (FlashStep step in sequence)
            {
                // flash color
                step.painting.color = step.color;

                yield return new WaitForSeconds(step.duration);

                // reset to white
                step.painting.color = Color.white;

                yield return new WaitForSeconds(resetDelay);
            }

            //  wait before restarting sequence
            yield return new WaitForSeconds(loopDelay);
        }
    }
}