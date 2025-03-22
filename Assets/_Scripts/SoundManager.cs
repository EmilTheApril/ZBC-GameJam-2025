using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private GameObject soundPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

    public void PlaySound(AudioClip sound)
    {
        GameObject soundObj = Instantiate(soundPrefab, transform.position, Quaternion.identity);
        soundObj.GetComponent<AudioSource>().PlayOneShot(sound);
        Destroy(soundObj, sound.length);
    }
}
