using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour{

    private FirstPersonMouseLookController mouseController;

    // Start is called before the first frame update.
    void Start(){

        mouseController = GetComponentInChildren<FirstPersonMouseLookController>();
        
    }

    // Update is called once per frame.
    void Update(){
        
    }

    /*
     * The function that gets called when the player is hit by a TSAI agent's bullet.
     * 
     * @param damageAmount a float value representing how much damage the player took.
     * @author Christopher Oehler.
     */
    void Damage(float damageAmount) {
        mouseController.FlinchHead(damageAmount);
    }
}
