using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonGunController : MonoBehaviour {

    public GameObject firstPersonCamera;
    private FirstPersonMouseLookController mouseController;
    public KeyCode shootKeyCode = KeyCode.Mouse0;
    public KeyCode reloadKeyCode = KeyCode.R;
    public float recoilAmount = 0.08f;
    public float recoilRecoverTime = 0.2f;
    public float holdHeight = 0.0f;
    public float holdSide = 0.0f;
    public float fireSpeed = 15.0f;
    public ParticleSystem smokeSystem;
    public ParticleSystem flashSystem;
    public ParticleSystem shellSystem;

    public int ammoInMagazine = 30;
    public int sizeOfMagazine = 30;
    public int ammoInReserve = 60;
    public float timeToReload = 1.0f;
    public float currentReloadTime = 0.0f;

    public enum GunState { Shooting, Idle, Reloading, MagazineEmpty};
    //[HideInInspector]
    public GunState state;
    


    private float waitUntilNextFire = 0.0f;
    private float currentRecoilZPos = 0.0f;
    private float currentRecoilZPosV = 0.0f;
    private float targetXRotation = 0.0f;
    private float targetYRotation = 0.0f;
    private float targetXRotationV = 0f;
    private float targetYRotationV = 0f;
    private float gunbobAmountX = 0.01f;
    private float gunbobAmountY = 0.01f;
    private float currentGunbobX = 0.0f;
    private float currentGunbobY = 0.0f;
    private float racioHipHold = 1.0f;
    private float racioHipHoldV;
    private readonly float shootAngleRandomizationNotAiming = 5.0f;
    private readonly float shootAngleRandomizationAiming = 15.0f;
    private RaycastBulletController bulletController;

    // Start is called before the first frame update.
    void Start() {
        mouseController = GetComponentInParent(typeof(FirstPersonMouseLookController)) as FirstPersonMouseLookController; // Get the mouse controller script.
        bulletController = transform.parent.GetComponentInChildren<RaycastBulletController>();
    }

    // Update is called once per frame.
    void LateUpdate() {

        currentGunbobX = Mathf.Sin(mouseController.headbobStepCounter) * gunbobAmountX * racioHipHold;
        currentGunbobY = Mathf.Cos(mouseController.headbobStepCounter * 2) * gunbobAmountY * -1 * racioHipHold;

        if (ammoInMagazine == 0 && state != GunState.Reloading) { // If there is no ammo in the magazine, set the gun's state to MagazineEmpty as long as it is not in the process of reloading.

            state = GunState.MagazineEmpty;

        }

        if (Input.GetKey(reloadKeyCode)) {

            if (state != GunState.Reloading) { // We are entering a reload cycle.

                if (ammoInReserve > 0) {
                    currentReloadTime = timeToReload;
                    state = GunState.Reloading;
                }
                
            }

        }

        if (state == GunState.Reloading) { // We are in the process of reloading, so countdown to the reload's end.

            currentReloadTime -= Time.deltaTime;

        }

        if (state == GunState.Reloading && currentReloadTime <= 0) { // We where reloading, but are now done.

            int ammoToLoad = sizeOfMagazine - ammoInMagazine; // Calculate how much ammo needs to be loaded into the magazine.

            if (ammoInReserve >= ammoToLoad) { // We have enough reserve ammo to fill a magazine.

                // Replenish the magazine and deplete reserve ammo.
                ammoInMagazine = sizeOfMagazine;
                ammoInReserve -= ammoToLoad;

            } else { // We don't have enough ammo to fill a magazine.

                // Fill the magazine with the remaining ammo in reserve.
                ammoInMagazine += ammoInReserve;
                ammoInReserve = 0;

            }

            currentReloadTime = timeToReload;
            state = GunState.Idle;

        }

        if (Input.GetKey(shootKeyCode)) { // The shoot key is being pressed.

            if (waitUntilNextFire <= 0.0f && (state != GunState.MagazineEmpty && state != GunState.Reloading)) { // A bullet has been chambered and the gun is not empty or reloading.

                state = GunState.Shooting; // Change gun state to shooting.

                // Point the gun in a random direction when fired.
                targetXRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, racioHipHold);
                targetYRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, racioHipHold);

                currentRecoilZPos -= recoilAmount; // Push the gun back to simulate recoil.

                // Emit muzzle flash and smoke from weapon muzzle.
                flashSystem.Emit(50);
                smokeSystem.Emit(10);

                shellSystem.Emit(1); // Emit an empty shell from the ejection port.

                bulletController.ShootRaycastBullet(10.0f); // Shoot a raycast bullet.

                waitUntilNextFire = 1.0f;

                ammoInMagazine--; // Remove a bullet from the magazine.

            }

        } else {

            if (state != GunState.Reloading && state != GunState.MagazineEmpty) {
                state = GunState.Idle; // Change gun state to not shooting.
            }

        }

        waitUntilNextFire -= Time.deltaTime * fireSpeed;

        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref currentRecoilZPosV, recoilRecoverTime); // Recover from recoil.

        // To have the gun smoothly follow the camera, use the SmoothDamp function to calculate X and Y rotation targets.
        targetXRotation = Mathf.SmoothDamp(targetXRotation, -mouseController.mouseLook.y, ref targetXRotationV, 0.05f);
        targetYRotation = Mathf.SmoothDamp(targetYRotation, mouseController.mouseLook.x, ref targetYRotationV, 0.05f);

        transform.position = firstPersonCamera.transform.position + (Quaternion.Euler(0, targetYRotation, 0) * new Vector3(holdSide * racioHipHold + currentGunbobX, holdHeight * racioHipHold + currentGunbobY, 0) + Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(0, 0, currentRecoilZPos)); // Move the gun into position.

        transform.rotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);

    }
}
