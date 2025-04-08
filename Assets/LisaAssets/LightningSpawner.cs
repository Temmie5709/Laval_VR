using UnityEngine;

public class LightningSpawner : MonoBehaviour
{
    public ParticleSystem eclair;
    public float interval = 2f;  // Temps entre chaque éclair (en secondes)

    void Start()
    {
        if (eclair != null)
        {
            // Appel répété toutes les "interval" secondes, avec un délai initial de 2 secondes
            InvokeRepeating("PlayParticle", 2f, interval);
        }
    }

    void PlayParticle()
    {
        eclair.Play();
    }
}
