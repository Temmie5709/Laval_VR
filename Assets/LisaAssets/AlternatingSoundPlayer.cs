using UnityEngine;

public class AlternatingLoopPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound1;
    public AudioClip sound2;

    [Range(0f, 1f)] public float volume1 = 1f;
    [Range(0f, 1f)] public float volume2 = 1f;

    private bool useFirst = true;

    void Start()
    {
        if (audioSource != null && sound1 != null && sound2 != null)
        {
            PlayNext(); // Démarre la boucle
        }
    }

    void Update()
    {
        // Quand le son actuel est terminé, on joue le suivant
        if (!audioSource.isPlaying)
        {
            PlayNext();
        }
    }

    void PlayNext()
    {
        if (useFirst)
        {
            audioSource.clip = sound1;
            audioSource.volume = volume1;
        }
        else
        {
            audioSource.clip = sound2;
            audioSource.volume = volume2;
        }

        audioSource.Play();
        useFirst = !useFirst;
    }
}
