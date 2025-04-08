using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class VFXControl : MonoBehaviour
{
    public GameObject bigLightObject;
    public GameObject explosionObject;
    public GameObject Nuage;
    public GameObject Eclair;
    public VisualEffect bigLightVFX;
    public VisualEffect explosionVFX;
    public VisualEffect joueurExplodVFX; // Nouveau VFX pour le joueur
    public GameObject joueurObject; // Le joueur à activer
    public Camera mainCamera;
    public CheckSphere Ruby;
    public CheckSphere1 Ruby1;
    public CheckSphere2 Ruby2;
    public CheckSphere3 Ruby3;
    public CheckSphere4 Ruby4;

    public float startRadius = 30f;
    public float endRadius = 0f;
    public float duration = 10f;

    private bool joueurSequenceStarted = false;
private bool pass = false;
void Start()
{
    // Désactivation du bigLightObject dès le départ
    if (bigLightObject != null)
        bigLightObject.SetActive(false);

    if (bigLightVFX != null)
        bigLightVFX.SetFloat("Radius", startRadius);
    else
        Debug.LogError("BigLight VisualEffect n'est pas assigné !");

    if (explosionObject != null)
        explosionObject.SetActive(false);

    if (joueurObject != null)
        joueurObject.SetActive(false);
}


    void Update()
    {
        // Vérification des conditions Ruby avant de lancer la séquence
        if (CheckRubyConditions())
        {
            if (pass==false){
                    Final();
            }
            
        }
    }

    bool CheckRubyConditions()
    {
        // Vérifier si toutes les conditions Ruby sont vraies
        return Ruby.isCorrectTargetInPlace && 
               Ruby1.isCorrectTargetInPlace1 &&
               Ruby2.isCorrectTargetInPlace2 &&
               Ruby3.isCorrectTargetInPlace3 &&
               Ruby4.isCorrectTargetInPlace4;
    }

    public void Final()
    {
        pass= true;
        StartCoroutine(HandleVFXSequence());
    }

    IEnumerator HandleVFXSequence()
    {
        bigLightObject.SetActive(true);
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
        float explosionDuration = 1f;
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

        // Désactivation des objets après avoir atteint 25000
        bigLightObject.SetActive(false);
        explosionObject.SetActive(false);
        Nuage.SetActive(false);
        Eclair.SetActive(false);

        // Descente de Nombre de 25000 à 0 en 3 secondes
        float rampDownDuration = 3f;
        t = 0f;
        while (t < rampDownDuration)
        {
            float value = Mathf.Lerp(25000f, 0f, t / rampDownDuration);
            joueurExplodVFX.SetFloat("Nombre", value);
            t += Time.deltaTime;
            yield return null;
        }
        joueurExplodVFX.SetFloat("Nombre", 0f);

        // Descente de "life" de 10 à 0 en 3 secondes
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

        // Désactivation des objets après la fin de la séquence
        yield return new WaitForSeconds(10f);
        if (joueurObject != null)
            joueurObject.SetActive(false);
        if (joueurExplodVFX != null)
            joueurExplodVFX.gameObject.SetActive(false);
    }
}
