using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPaintingFlash : MonoBehaviour
{
    [System.Serializable]
    public class FlashStep
    {
        public Image painting;
        public Sprite flashSprite;
        public float duration;
    }

    public FlashStep[] sequence;
    public float resetDelay = 0.1f;
    public float loopDelay = 1.5f;

    private Sprite[] originalSprites;

    void Start()
    {
        // store original sprites at start
        originalSprites = new Sprite[sequence.Length];

        for (int i = 0; i < sequence.Length; i++)
        {
            if (sequence[i].painting != null)
            {
                originalSprites[i] = sequence[i].painting.sprite;
            }
        }

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        while (true)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                FlashStep step = sequence[i];

                if (step.painting == null) continue;

                // flash custom image
                step.painting.sprite = step.flashSprite;

                yield return new WaitForSeconds(step.duration);

                // restore original image
                step.painting.sprite = originalSprites[i];

                yield return new WaitForSeconds(resetDelay);
            }

            yield return new WaitForSeconds(loopDelay);
        }
    }
}