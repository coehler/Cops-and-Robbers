using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmunitionReadoutController : MonoBehaviour{

    public GameObject player;
    private Text currentAmmoReadout;
    private Text reserveAmmoReadout;
    private Text currentGunDisplayName;
    private FirstPersonGunController firstPersonGunController;

    // Start is called before the first frame update.
    void Start(){

        currentAmmoReadout = GameObject.Find("Current Ammo").GetComponent<Text>();
        reserveAmmoReadout = GameObject.Find("Reserve Ammo").GetComponent<Text>();
        currentGunDisplayName = GameObject.Find("Reserve Ammo").GetComponent<Text>();
        firstPersonGunController = player.GetComponentInChildren<FirstPersonGunController>();

    }

    // Update is called once per frame.
    void Update(){

        currentAmmoReadout.text = firstPersonGunController.ammoInMagazine.ToString();
        reserveAmmoReadout.text = firstPersonGunController.ammoInReserve.ToString();

    }
}
