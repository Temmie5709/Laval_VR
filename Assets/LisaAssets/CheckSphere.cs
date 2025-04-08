using UnityEngine;
using UnityEngine.VFX;
using System.Collections;  // Nécessaire pour utiliser les coroutines

public class CheckSphere : MonoBehaviour
{
    public GameObject[] targetObjects;  // Liste des objets à vérifier
    public GameObject correctTarget;    // L'objet correct à vérifier
    public float distanceThreshold = 3f;
    public VisualEffect visualEffect;   // Référence au Visual Effect avec la texture SDF
    public Texture3D sdfSphere;
    public Texture3D sdfArbre;
    public Texture3D sdfArbreTemp; // Nouvelle texture pour l'effet temporaire

    private bool isCorrectTargetInPlace = false;
    private bool isCoroutineRunning = false; // Ajout de cette variable pour contrôler la coroutine

    void Update()
    {
        bool correctTargetInRange = false;

        foreach (GameObject target in targetObjects)
        {
            if (target == null) continue;

            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Si un objet est dans la zone de distance et c'est le bon
            if (distance < distanceThreshold)
            {
                if (target == correctTarget)
                {
                    correctTargetInRange = true;
                    target.transform.position = transform.position;
                    Debug.Log("Le bon objet est dans la zone, il se déplace !");
                }
                else
                {
                    target.transform.position = transform.position;  // Déplace les autres objets
                    Debug.Log("Un objet incorrect est dans la zone : " + target.name);

                    // Si la coroutine n'est pas déjà en cours, lance-la
                    if (!isCoroutineRunning)
                    {
                        StartCoroutine(ChangeTextureTemporarily());  // Lance la coroutine une seule fois
                    }
                }
            }
            else
            {
                // Si un objet incorrect est retiré de la zone
                if (target != correctTarget && isCoroutineRunning)
                {
                    StopCoroutine(ChangeTextureTemporarily()); // Arrête la coroutine si l'objet incorrect est retiré
                    isCoroutineRunning = false; // La coroutine n'est plus en cours
                    Debug.Log("L'objet incorrect a quitté la zone. La coroutine est arrêtée.");
                }
            }
        }

        // Met à jour la texture du Visual Effect selon l'état de l'objet correct
        if (correctTargetInRange && !isCorrectTargetInPlace)
        {
            isCorrectTargetInPlace = true;
            UpdateTexture(true);  // Met à jour la texture à la bonne valeur
            Debug.Log("Le bon objet est maintenant en place. Mise à jour de la texture.");
        }
        else if (!correctTargetInRange && isCorrectTargetInPlace)
        {
            isCorrectTargetInPlace = false;
            UpdateTexture(false);  // Remet la texture à "None"
            Debug.Log("L'objet correct est retiré, remise à zéro de la texture.");
        }
    }

    void UpdateTexture(bool isCorrect)
    {
        if (visualEffect != null)
        {
            // Si l'objet correct est en place, on change la texture dans le VisualEffect
            if (isCorrect)
            {
                visualEffect.SetTexture("SDF", sdfArbre);  // Si correct, texture finale
                Debug.Log("Texture changée en sdfArbre (correct)");
            }
            else
            {
                // Pas besoin de lancer la coroutine ici car elle est déjà gérée plus tôt
                Debug.Log("Texture changée en sdfArbreTemp (incorrect)");
            }
        }
    }

    // Coroutine pour gérer le changement de texture temporaire
    IEnumerator ChangeTextureTemporarily()
    {
        isCoroutineRunning = true; // Indiquer que la coroutine est en cours
        visualEffect.SetTexture("SDF", sdfArbreTemp);  // Change la texture à sdfArbreTemp
        Debug.Log("Texture temporaire changée en sdfArbreTemp");

        // Attends pendant 2 secondes
        yield return new WaitForSeconds(2f);

        visualEffect.SetTexture("SDF", sdfSphere);  // Après 2 secondes, on remet sdfSphere
        Debug.Log("Retour à la texture sdfSphere après 2 secondes");

        isCoroutineRunning = false; // La coroutine est terminée
    }
}
