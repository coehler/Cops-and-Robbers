using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmunitionReadoutController : MonoBehaviour{

    public Text currentAmmoReadout;
    public Text reserveAmmoReadout;
    public FirstPersonGunController firstPersonGunController;

    // Start is called before the first frame update.
    void Start(){
        
    }

    // Update is called once per frame.
    void Update(){

        currentAmmoReadout.text = firstPersonGunController.ammoInMagazine.ToString();
        reserveAmmoReadout.text = firstPersonGunController.ammoInReserve.ToString();

    }
}
