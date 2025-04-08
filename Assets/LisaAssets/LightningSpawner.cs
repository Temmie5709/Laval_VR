using UnityEngine;
using UnityEngine.VFX;

public class LightningSpawner : MonoBehaviour
{
    public ParticleSystem eclair;
    public AudioSource thunderSound;
    public float interval = 2f;
    public float thunderDelay = 1f;

    public VisualEffect vfx;
    public Collider lightningCollider; // Le collider de l’éclair (isKinematic)
    public Collider playerCollider;    // Le collider du joueur (CharacterController ou autre)

    private int lightningCount = 0;
    private bool vfxActive = false;

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

        // Vérifie la collision dès que l’éclair apparaît
        if (playerCollider != null && lightningCollider != null)
        {
            if (lightningCollider.bounds.Intersects(playerCollider.bounds) && !vfxActive)
            {
                TriggerVFX();
            }
        }

        // Si on est dans les bonnes conditions pour un son de tonnerre
        if (lightningCount != 3 && (lightningCount < 3 || (lightningCount - 3) % 5 != 0))
        {
            Invoke("PlayThunder", thunderDelay);
        }
    }

    void PlayThunder()
    {
        if (thunderSound != null)
        {
            thunderSound.Play();
        }
    }

    void TriggerVFX()
    {
        if (vfx != null)
        {
            vfx.SetFloat("Nombre", 600);
            vfx.SetFloat("life", 10f);
            vfxActive = true;

            // Reset VFX valeurs après un moment
            Invoke(nameof(ResetLife), 10f);
            Invoke(nameof(ResetNombre), 20f);
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
        vfxActive = false;
    }

#if UNITY_EDITOR
    // Pour voir le collider dans l'éditeur
    void OnDrawGizmos()
    {
        if (lightningCollider != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(lightningCollider.bounds.center, lightningCollider.bounds.size);
        }
    }
#endif
}
