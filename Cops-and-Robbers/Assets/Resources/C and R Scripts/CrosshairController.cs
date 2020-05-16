using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour{
    
    public GameObject player;

    private RaycastBulletController bulletController;
    private RectTransform top, bottom, left, right;
    private Image interaction;

    private readonly float maxLocation = 100.0f;
    private readonly float minLocation = 10.0f;

    // Start is called before the first frame update.
    void Start(){

        // Get all four crosshair reticles.
        top = this.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        right = this.gameObject.transform.GetChild(1).GetComponent<RectTransform>();
        bottom = this.gameObject.transform.GetChild(2).GetComponent<RectTransform>();
        left = this.gameObject.transform.GetChild(3).GetComponent<RectTransform>();
        interaction = this.gameObject.transform.GetChild(4).GetComponent<Image>();

        bulletController = player.GetComponentInChildren<RaycastBulletController>(); // Get the raycast bullet controller so we can get spread values.

        interaction.fillClockwise = false;

    }

    // Update is called once per frame.
    void Update(){

        top.anchoredPosition = new Vector2(0, minLocation + (bulletController.spreadCurrent * maxLocation));
        bottom.anchoredPosition = new Vector2(0, -minLocation - (bulletController.spreadCurrent * maxLocation));
        left.anchoredPosition = new Vector2(-minLocation - (bulletController.spreadCurrent * maxLocation), 0);
        right.anchoredPosition = new Vector2(minLocation + (bulletController.spreadCurrent * maxLocation), 0);

        if (bulletController.gunController.aiming) {

            ToggleCrosshair(false);

            if (bulletController.gunController.state == FirstPersonGunController.GunState.Reloading) {

                interaction.fillAmount = bulletController.gunController.currentTimeToReload / bulletController.gunController.timeToReload;

            } else if (bulletController.gunController.state == FirstPersonGunController.GunState.Switching) {

                interaction.fillAmount = bulletController.gunController.currentSwitchTime / bulletController.gunController.switchTime;

            } else {

                interaction.fillAmount = 0.0f;

            }

        } else {

            ToggleCrosshair(true);

            if (bulletController.gunController.state == FirstPersonGunController.GunState.Reloading) {

                ToggleCrosshair(false);

                interaction.fillAmount = bulletController.gunController.currentTimeToReload / bulletController.gunController.timeToReload;

            } else if (bulletController.gunController.state == FirstPersonGunController.GunState.Switching) {

                ToggleCrosshair(false);

                interaction.fillAmount = bulletController.gunController.currentSwitchTime / bulletController.gunController.switchTime;

            } else {

                interaction.fillAmount = 0.0f;

            }

        }

    }

    /*
     * This function enables and disables the crosshairs.
     * 
     * @param toggle sets the crosshairs to active when true and not active when false.
     * @author Christopher Oehler.
     */
    private void ToggleCrosshair(bool toggle) {

        top.gameObject.SetActive(toggle);
        bottom.gameObject.SetActive(toggle);
        left.gameObject.SetActive(toggle);
        right.gameObject.SetActive(toggle);

    }
}
