using UnityEngine;
using UnityEngine.VFX;

public class LightningSpawner : MonoBehaviour
{
    public ParticleSystem eclair;
    public AudioSource thunderSound;
    public float interval = 2f;
    public float thunderDelay = 1f;

    public VisualEffect vfx;
    public Collider lightningCollider; // Trigger
    public Collider playerCollider;    // Celui du joueur (non trigger, probablement CharacterController)

    private int lightningCount = 0;
    private bool canTrigger = true; // Pour éviter plusieurs déclenchements

    void Start()
    {
        if (eclair != null)
        {
            InvokeRepeating("PlayParticle", 2f, interval);
        }
    }

    void PlayParticle()
    {
        eclair.Play();
        lightningCount++;

        if (lightningCount != 3 && (lightningCount < 3 || (lightningCount - 3) % 5 != 0))
        {
            Invoke("PlayThunder", thunderDelay);
        }

        canTrigger = true; // Réactiver la possibilité de détecter une nouvelle collision
    }

    void PlayThunder()
    {
        if (thunderSound != null)
        {
            thunderSound.Play();
        }
    }

    // Cette méthode est appelée automatiquement quand un objet entre dans le trigger
    void OnTriggerEnter(Collider other)
    {
        if (canTrigger && other == playerCollider)
        {
            Debug.Log("Le joueur a été touché par l’éclair !");
            TriggerVFX();
            canTrigger = false; // Empêcher que ça se déclenche plusieurs fois de suite
        }
    }

    void TriggerVFX()
    {
        if (vfx != null)
        {
            vfx.SetFloat("Nombre", 600);
            vfx.SetFloat("life", 10f);

            // Reset progressif
            Invoke(nameof(ResetLife), 20f);
            Invoke(nameof(ResetNombre), 30f); // 10 sec après reset de life
        }
    }

    void ResetLife()
    {
        if (vfx != null)
        {
            vfx.SetFloat("life", 0f);
        }
    }

    void ResetNombre()
    {
        if (vfx != null)
        {
            vfx.SetFloat("Nombre", 0);
        }
    }
}
