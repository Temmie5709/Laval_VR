using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class CheckSphere1 : MonoBehaviour
{
    public GameObject[] targetObjects;  // Liste des objets à vérifier
    public GameObject correctTarget;    // L'objet correct à vérifier
    public float distanceThreshold = 3f;
    public VisualEffect visualEffect;   // Référence au Visual Effect avec la texture SDF
    public Texture3D sdfSphere;
    public Texture3D sdfArbre;
    public Texture3D sdfArbreTemp; // Nouvelle texture pour l'effet temporaire
    public string variable; 
    private bool isCorrectTargetInPlace = false;
    private bool isCoroutineRunning = false; // Contrôle de la coroutine
    private float groundY; // Variable pour stocker la hauteur du sol

    void Start()
    {
        // On suppose que le socle est situé à une certaine hauteur (ex. 0 sur l'axe Y)
        groundY = transform.position.y;
    }

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
                    // On fixe l'objet à la position du socle, en maintenant la hauteur du sol
                    target.transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
                }
                else
                {
                    // Déplace les autres objets tout en maintenant la hauteur du sol
                    target.transform.position = new Vector3(transform.position.x, groundY, transform.position.z);

                    if (!isCoroutineRunning)
                    {
                        StartCoroutine(ChangeTextureTemporarily());  // Lance la coroutine une seule fois
                    }
                }
            }
            else
            {
                if (target != correctTarget && isCoroutineRunning)
                {
                    StopCoroutine(ChangeTextureTemporarily()); // Arrête la coroutine si l'objet incorrect est retiré
                    isCoroutineRunning = false; // La coroutine n'est plus en cours
                }
            }
        }

        // Mise à jour de la texture du Visual Effect selon l'état de l'objet correct
        if (correctTargetInRange && !isCorrectTargetInPlace)
        {
            isCorrectTargetInPlace = true;
            UpdateTexture(true);  // Met à jour la texture à la bonne valeur
        }
        else if (!correctTargetInRange && isCorrectTargetInPlace)
        {
            isCorrectTargetInPlace = false;
            UpdateTexture(false);  // Remet la texture à "None"
        }

        // Si l'objet correct est en dehors de la zone, on rétablit la texture en sphère
        if (!correctTargetInRange && isCorrectTargetInPlace)
        {
            isCorrectTargetInPlace = false;
            UpdateTexture(false);  // Remet la texture à "None" (ou à la sphère si tu préfères)
        }
    }

    void UpdateTexture(bool isCorrect)
    {
        if (visualEffect != null)
        {
            // Si l'objet correct est en place, on change la texture dans le VisualEffect
            if (isCorrect)
            {
                visualEffect.SetTexture(variable, sdfArbre);  // Texture finale (l'arbre)
            }
            else
            {
                // Remet la texture à la sphère (ou la texture par défaut)
                visualEffect.SetTexture(variable, sdfSphere);  // Texture de retour (la sphère)
            }
        }
    }

    // Coroutine pour gérer le changement de texture temporaire
    IEnumerator ChangeTextureTemporarily()
    {
        isCoroutineRunning = true; // Indiquer que la coroutine est en cours
        visualEffect.SetTexture(variable, sdfArbreTemp);  // Change la texture à sdfArbreTemp

        // Attends pendant 2 secondes
        yield return new WaitForSeconds(2f);

        visualEffect.SetTexture(variable, sdfSphere);  // Après 2 secondes, on remet sdfSphere
        isCoroutineRunning = false; // La coroutine est terminée
    }
}
