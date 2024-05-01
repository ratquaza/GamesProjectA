using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DarknessTile : MonoBehaviour
{
    public GameObject darkness;
    public float transitionDuration = 0.5f; // Duration of the transition in seconds
    private bool isTransitioning = false;
    private Coroutine transitionCoroutine;
    [SerializeField] private Color overlayTargetColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StopTransition();

        if (other.CompareTag("Player"))
        {
            transitionCoroutine = StartCoroutine(TransitionDarken(true));
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        StopTransition();

        if (other.CompareTag("Player"))
        {
            transitionCoroutine = StartCoroutine(TransitionDarken(false));
        }
    }

    private IEnumerator TransitionDarken(bool darken)
    {
        Image darknessImage = darkness.GetComponent<Image>();
        Color startColor = darknessImage.color;
        Color targetColor = darken ? overlayTargetColor : Color.clear;

        float timer = 0f;
        while (timer < transitionDuration)
        {
            float t = timer / transitionDuration;
            darknessImage.color = Color.Lerp(startColor, targetColor, t);
            timer += Time.deltaTime;
            yield return null;
        }

        darknessImage.color = targetColor;
    }

    // Stop the ongoing transition if another trigger event occurs
    private void StopTransition()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            isTransitioning = false;
        }
    }
}
