using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class VFXControl : MonoBehaviour
{
    public GameObject bigLightObject;
    public GameObject explosionObject;
    public VisualEffect bigLightVFX;
    public VisualEffect explosionVFX;
    public VisualEffect joueurExplodVFX; // Nouveau VFX pour le joueur
    public GameObject joueurObject; // Le joueur à activer
    public Camera mainCamera;

    public float startRadius = 30f;
    public float endRadius = 0f;
    public float duration = 10f;

    private bool joueurSequenceStarted = false;

    void Start()
    {
        if (bigLightObject != null)
            bigLightObject.SetActive(true);

        if (bigLightVFX != null)
            bigLightVFX.SetFloat("Radius", startRadius);
        else
            Debug.LogError("BigLight VisualEffect n'est pas assigné !");

        if (explosionObject != null)
            explosionObject.SetActive(false);

        if (joueurObject != null)
            joueurObject.SetActive(false);

        StartCoroutine(HandleVFXSequence());
    }

    IEnumerator HandleVFXSequence()
    {
        // Réduction du radius
        float timePassed = 0f;
        while (timePassed < duration)
        {
            float radiusValue = Mathf.Lerp(startRadius, endRadius, timePassed / duration);
            bigLightVFX.SetFloat("Radius", radiusValue);
            timePassed += Time.deltaTime;
            yield return null;
        }

        bigLightVFX.SetFloat("Radius", endRadius);
        yield return new WaitForSeconds(2f);

        // Explosion
        explosionObject.SetActive(true);
        timePassed = 0f;
        float explosionDuration = 0.5f;
        while (timePassed < explosionDuration)
        {
            float sphereValue = Mathf.Lerp(0f, 0.7f, timePassed / explosionDuration);
            explosionVFX.SetFloat("RadiusSphere", sphereValue);
            timePassed += Time.deltaTime;
            yield return null;
        }
        explosionVFX.SetFloat("RadiusSphere", 0.7f);

        // Expansion progressive de RadiusCircle
        timePassed = 0f;
        float circleDuration = 3f;
        while (timePassed < circleDuration)
        {
            float circleValue = Mathf.Lerp(0f, 10f, timePassed / circleDuration);
            explosionVFX.SetFloat("RadiusCircle", circleValue);

            // Lancer la séquence joueur quand RadiusCircle >= 8
            if (!joueurSequenceStarted && circleValue >= 8f)
            {
                joueurSequenceStarted = true;
                StartCoroutine(HandleJoueurSequence());
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
        explosionVFX.SetFloat("RadiusCircle", 10f);



    }

IEnumerator HandleJoueurSequence()
{
    // Activer l'objet Joueur
    if (joueurObject != null)
        joueurObject.SetActive(true);

    // Faire monter Nombre de 0 à 25000 en 5 secondes
    float t = 0f;
    float rampUpDuration = 5f;
    while (t < rampUpDuration)
    {
        float value = Mathf.Lerp(0f, 25000f, t / rampUpDuration);
        joueurExplodVFX.SetFloat("Nombre", value);
        t += Time.deltaTime;
        yield return null;
    }
    joueurExplodVFX.SetFloat("Nombre", 25000f);
        bigLightObject.SetActive(false);
        explosionObject.SetActive(false);
    float rampDownDuration = 3f;
    t = 0f;
    while (t < rampDownDuration)
    {
        float value = Mathf.Lerp(25000f, 0f, t / rampDownDuration);
        joueurExplodVFX.SetFloat("Nombre", value);
        t += Time.deltaTime;
        yield return null;
    }
    rampDownDuration = 3f;
    t = 0f;
    while (t < rampDownDuration)
    {
        float value = Mathf.Lerp(10f, 0f, t / rampDownDuration);
        joueurExplodVFX.SetFloat("life", value);
        t += Time.deltaTime;
        yield return null;
    }
    joueurExplodVFX.SetFloat("life", 0f);
        yield return new WaitForSeconds(10f);
    // Maintenant on désactive proprement
    if (joueurObject != null)
        joueurObject.SetActive(false);
    if (joueurExplodVFX != null)
        joueurExplodVFX.gameObject.SetActive(false);
}

}
