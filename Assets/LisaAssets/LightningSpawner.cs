using UnityEngine;
using UnityEngine.VFX;

public class LightningSpawner : MonoBehaviour
{
    public ParticleSystem eclair;
    public AudioSource thunderSound;
    public float interval = 2f;
    public float thunderDelay = 1f;

    public VisualEffect vfx;
    public Collider lightningCollider; // Le collider de l’éclair
    public Collider playerCollider;    // Le collider du joueur

    private int lightningCount = 0;

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

        // Vérifie si le joueur est touché par l’éclair
        if (playerCollider != null && lightningCollider != null)
        {

        }

        if (lightningCount != 3 && (lightningCount < 3 || (lightningCount - 3) % 5 != 0))
        {
            Invoke("PlayThunder", thunderDelay);

                        if (lightningCollider.bounds.Intersects(playerCollider.bounds))
            {
                // Le joueur est touché !
                TriggerVFX();
            }
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

            // Remettre life à 0 après 20 secondes
            Invoke(nameof(ResetLife), 20f);

            // Remettre nombre à 0 encore 10 secondes plus tard
            Invoke(nameof(ResetNombre), 30f); // 20 + 10
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
