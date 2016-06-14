using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    public FModController SND_Controller_Gun;
    EventInstance SND_Gun;
    [EventRef]
    public string SND_ReloadString;
    public EventInstance SND_Reload;

    public string p_ScoreValue;
    public int ScoreDelta;
    public ScoreManager scoreManagerProvider;
    int scoreCurr=0;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;

    private PLAYBACK_STATE _state;

    Light gunLight;
    float effectsDisplayTime = 0.2f;


    
    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();

		gunLight = GetComponent<Light> ();

		if (SND_Controller_Gun != null)
		{
			SND_Gun = RuntimeManager.CreateInstance(SND_Controller_Gun.SND_PlayerShoot);
		}

    }

    void Start()
    {
        SND_Reload = RuntimeManager.CreateInstance(SND_ReloadString);
        if (ScoreDelta == 0) {
            ScoreDelta = 10;
        }
    }

    private int _scoreValue = 0;
    private int ScoreValue
    {
        get
        {
            return _scoreValue;
        }

        set
        {
            if (value != _scoreValue)
            {
                //update fmod & sound start
                _scoreValue = value;
                SND_Reload.start();
            }
        }

    }


    void Update ()
    {
        timer += Time.deltaTime;
        scoreCurr = ScoreManager.score;
        ScoreValue = scoreCurr / ScoreDelta;

        FMOD.RESULT res = SND_Reload.getPlaybackState(out _state);
        if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0 && res == FMOD.RESULT.OK && _state != PLAYBACK_STATE.PLAYING)
        {
            
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        SND_Gun.start ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
