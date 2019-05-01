using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunReadoutController : MonoBehaviour{

    public GameObject player;
    private Text currentGunDisplayName;
    private FirstPersonGunController firstPersonGunController;

    // Start is called before the first frame update.
    void Start() {

        currentGunDisplayName = GameObject.Find("Current Gun").GetComponent<Text>();
        firstPersonGunController = player.GetComponentInChildren<FirstPersonGunController>();

    }

    // Update is called once per frame.
    void Update() {

        currentGunDisplayName.text = firstPersonGunController.displayName;

    }
}
