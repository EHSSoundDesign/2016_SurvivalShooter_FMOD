using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 5f;
    [EventRef]
    public string MusicString;

    public EventInstance MusicEvent
    {
        get;
        private set;
    }

    [EventRef]
    public string AmbienceString;
    public EventInstance AmbienceEvent;

    Animator anim;
	float restartTimer;

    void Awake()
    {
        MusicEvent = RuntimeManager.CreateInstance(MusicString);
        AmbienceEvent = RuntimeManager.CreateInstance(AmbienceString);
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        MusicEvent.start();
        AmbienceEvent.start();
    }

    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            MusicEvent.stop(STOP_MODE.ALLOWFADEOUT);
            AmbienceEvent.stop(STOP_MODE.ALLOWFADEOUT);
            anim.SetTrigger("GameOver");

			restartTimer += Time.deltaTime;

			if (restartTimer >= restartDelay) 
			{
				Application.LoadLevel (Application.loadedLevel);
			}
        }
    }
}
