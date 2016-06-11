using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FModController : MonoBehaviour {
    [EventRef]
    public string SND_PlayerDeath = "event:/Player/Player_Death";
    [EventRef]
    public string SND_PlayerHit = "event:/Player/Player_Hit";
    [EventRef]
    public string SND_PlayerShoot = "event:/Player/Player_Shoot";
    EventInstance SND_ControlledEvent; // istanza dell'evento
    float SND_param1; // variabile di appoggio per lettura del valore del pitch dell'istanza audio3

    void Start() {
 
        //SND_ControlledEvent = RuntimeManager.CreateInstance(SND_Ambience);

        //SND_ControlledEvent.start();
    }
}