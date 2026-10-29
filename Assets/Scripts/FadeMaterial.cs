using System.Collections;
using UnityEngine;

public class FadeMaterial : MonoBehaviour {

    public static FadeMaterial instance;

    public Color normalEmissionColor = Color.black;
    public Color blinkEmissionColor = Color.cyan * 2f;
    public float blinkSpeed = 1f;

    private Material mat;
    private bool blinking = false;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        Renderer rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", normalEmissionColor);
    }
    private void Start() {
        StartBlink();
    }
   
    public void StartBlink() {
        if (!blinking) {
            blinking = true;
            StartCoroutine(BlinkCoroutine());
        }
    }

    public void StopBlink() {
        if (blinking) {
            blinking = false;
            StopAllCoroutines();
            mat.SetColor("_EmissionColor", normalEmissionColor);
        }
    }

    private IEnumerator BlinkCoroutine() {
        while (blinking) {
            float transitionBLink = (Mathf.Sin(Time.time * blinkSpeed * Mathf.PI * 2f) + 1f) / 2f;
            Color color = Color.Lerp(normalEmissionColor, blinkEmissionColor, transitionBLink);
            mat.SetColor("_EmissionColor", color);
            yield return null;
        }
    }
}

