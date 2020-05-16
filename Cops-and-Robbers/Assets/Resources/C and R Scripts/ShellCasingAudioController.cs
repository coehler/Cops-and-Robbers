using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCasingAudioController : MonoBehaviour{

    private List<ParticleCollisionEvent> collisionEvents;
    private List<AudioClip> shellBounceClips;

    // Start is called before the first frame update
    void Start(){

        collisionEvents = new List<ParticleCollisionEvent>();
        shellBounceClips = new List<AudioClip> {
            Resources.Load<AudioClip>("C and R Original Assets/Audio/casing_1"),
            Resources.Load<AudioClip>("C and R Original Assets/Audio/casing_2"),
            Resources.Load<AudioClip>("C and R Original Assets/Audio/casing_3"),
            Resources.Load<AudioClip>("C and R Original Assets/Audio/casing_4"),
            Resources.Load<AudioClip>("C and R Original Assets/Audio/casing_5")
        };

    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnParticleCollision(GameObject other) {

        ParticlePhysicsExtensions.GetCollisionEvents(GetComponent<ParticleSystem>(), other, collisionEvents);

        foreach (ParticleCollisionEvent e in collisionEvents) {

            int clipIndex = Random.Range(0, 4);
            AudioSource.PlayClipAtPoint(shellBounceClips[0], e.intersection, 0.5f);

        }
    }
}
