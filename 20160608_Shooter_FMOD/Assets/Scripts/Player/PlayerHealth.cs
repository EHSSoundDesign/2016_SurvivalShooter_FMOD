using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;

    public FModController PlayerAudioController;
    EventInstance Player_Hit;
    EventInstance Player_Dead;
    public string p_Sound_Health;
    public GameOverManager MusicParameterProvider;
    float val_NormHealth;
    float debugVal;

    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;

    StudioEventEmitter playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;

    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
        val_NormHealth = (float)currentHealth / (float)startingHealth;
        Debug.Log(val_NormHealth);

        if (PlayerAudioController != null)
        {
            Player_Hit = RuntimeManager.CreateInstance(PlayerAudioController.SND_PlayerHit);
            Player_Dead = RuntimeManager.CreateInstance(PlayerAudioController.SND_PlayerDeath);
        }




    }

    void Start()
    {
        if (MusicParameterProvider != null)
        {
            MusicParameterProvider.MusicEvent.setParameterValue(p_Sound_Health, val_NormHealth);
            MusicParameterProvider.MusicEvent.getParameterValue(p_Sound_Health, out debugVal);
            Debug.Log(debugVal);
            MusicParameterProvider.AmbienceEvent.setParameterValue(p_Sound_Health, val_NormHealth);
        }


    }

    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        val_NormHealth = (float)currentHealth / (float)startingHealth;
        MusicParameterProvider.MusicEvent.setParameterValue(p_Sound_Health, val_NormHealth);
        MusicParameterProvider.AmbienceEvent.setParameterValue(p_Sound_Health, val_NormHealth);
        MusicParameterProvider.MusicEvent.getParameterValue(p_Sound_Health, out debugVal);
        Debug.Log(debugVal);
        Player_Hit.start();

        healthSlider.value = currentHealth;
        
        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        Player_Dead.start();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
