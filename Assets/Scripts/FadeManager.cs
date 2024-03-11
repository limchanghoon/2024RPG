using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeSpeed;

    public IEnumerator Fade(bool isOut)
    {
        float t = isOut ? 0f : 1f;
        float sign = isOut ? fadeSpeed : -fadeSpeed;

        while (true)
        {
            yield return null;
            t += sign * Time.deltaTime;
            canvasGroup.alpha = t;
            if (isOut && 1f <= t) yield break;
            if (!isOut && t <= 0f)  yield break;
        }
    }
}
