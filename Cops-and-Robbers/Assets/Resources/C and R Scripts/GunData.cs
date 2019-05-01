using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This class contains data about how a gun shoots and looks.
 * 
 * @author Christopher Oehler
 */
public class GunData : MonoBehaviour{

    public string displayName = "M4A1";

    // Data regarding raycast bullets.
    public float spreadCurrent = 0.0f;
    public float spreadMin = 0.3f;
    public float spreadMax = 1.3f;
    public float spreadPerShot = 0.1f;
    public float spreadRecovery = 0.5f;
    public float hipSpreadMin = 0.3f;
    public float hipSpreadMax = 1.3f;
    public float hipSpreadPerShot = 0.1f;
    public float hipSpreadRecovery = 1.5f;
    public float aimSpreadMin = 0.05f;
    public float aimSpreadMax = 0.3f;
    public float aimSpreadPerShot = 0.01f;
    public float aimSpreadRecovery = 1.5f;

    // Data regarding how the visual gun model appears.
    public float recoilAmount = 0.03f;
    public float recoilRecoverTime = 0.2f;
    public float holdHeight = -0.1f;
    public float holdSide = 0.2f;
    public float fireSpeed = 10.0f;
    public float racioHipHold = 1.0f;
    public float hipRotateSpeed = 0.05f;
    public float aimRotateSpeed = 0.01f;
    public int ammoInMagazine = 30;
    public int maxAmmoInMagazine = 30;
    public int ammoInReserve = 60;
    public float timeToReload = 1.0f;
    public float hipToAimSpeed = 0.1f;
    public float hipZoom = 60.0f;
    public float aimZoom = 40.0f;

    // Data regarding weapon FX.
    public ParticleSystem smokeSystem;
    public ParticleSystem flashSystem;
    public ParticleSystem shellSystem;
    public AudioSource gunAudioSource;
    public string shotClip = "M4A1_shot";
    public string reloadClip = "M4A1_reload";

}
