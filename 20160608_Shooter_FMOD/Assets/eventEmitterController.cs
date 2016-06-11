using UnityEngine;
using System.Collections;
using FMODUnity;

public class eventEmitterController : MonoBehaviour {

    StudioEventEmitter emitter;

	// Use this for initialization
	void Start () {
        emitter = GetComponent<StudioEventEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            StartAndStopEmitter();

        if (Input.GetKeyDown(KeyCode.A))
            emitter.Event = "event:/Enemies/Hellephant_Death";

        if (Input.GetKeyDown(KeyCode.S))
            emitter.Event = "event:/Enemies/Hellephant_Hit";

    }

    public void StartAndStopEmitter() {
        //emitter.Event = "event:/Ambience/Ambience";
        
        emitter.Play();
        //if (!emitter.IsPlaying())
        //    emitter.Play();
        //else
        //    emitter.Stop();


    }
}
