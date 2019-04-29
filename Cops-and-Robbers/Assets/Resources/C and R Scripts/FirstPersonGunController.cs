using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonGunController : MonoBehaviour {

    public GameObject firstPersonCamera;
    private FirstPersonMouseLookController mouseController;
    public KeyCode shootKeyCode = KeyCode.Mouse0;
    public KeyCode aimKeyCode = KeyCode.Mouse1;
    public KeyCode reloadKeyCode = KeyCode.R;
    public float recoilAmount = 0.03f;
    public float recoilRecoverTime = 0.2f;
    public float holdHeight = -0.3f;
    public float holdSide = 0.4f;
    public float fireSpeed = 15.0f;
    public float racioHipHold = 1.0f;
    public float racioHipHoldV;
    public float hipRotateSpeed = 0.05f;
    public float aimRotateSpeed = 0.01f;
    public float currentRotateSpeed;
    public ParticleSystem smokeSystem;
    public ParticleSystem flashSystem;
    public ParticleSystem shellSystem;
    public int ammoInMagazine = 30;
    public int maxAmmoInMagazine = 30;
    public int ammoInReserve = 60;
    public float timeToReload = 1.0f;
    public float hipToAimSpeed = 0.1f;
    public float hipZoom = 60.0f;
    public float aimZoom = 40.0f;
    public float currentZoom;
    public enum GunState { Shooting, Idle, Reloading, MagazineEmpty};

    [HideInInspector]
    public GunState state;
    [HideInInspector]
    public float currentTimeToReload = 0.0f;
    [HideInInspector]
    public bool aiming = false;

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

        if (ammoInMagazine == 0 && state != GunState.Reloading) { // If there is no ammunition in the magazine and the player is not reloading, set the gun's state to MagazineEmpty.

            state = GunState.MagazineEmpty;

        }

        if (Input.GetKey(reloadKeyCode)) { // If the reload key has been pressed, enter a reloading state.

            if (state != GunState.Reloading) {

                if (ammoInReserve > 0) {
                    currentTimeToReload = timeToReload;
                    state = GunState.Reloading;
                }
                
            }

        }

        if (state == GunState.Reloading) { // If the player is in the process of reloading, countdown to the end of the reload.

            currentTimeToReload -= Time.deltaTime;

        }

        if (state == GunState.Reloading && currentTimeToReload <= 0) { // If the player was reloading, but is now done.

            int ammoToLoad = maxAmmoInMagazine - ammoInMagazine;

            if (ammoInReserve >= ammoToLoad) { // If there is enough reserve ammunition to completely fill a new magazine, do so.
 
                ammoInMagazine = maxAmmoInMagazine;
                ammoInReserve -= ammoToLoad;

            } else { // If there is not enough reserve ammunition to completely fill a new magazine, use the remaining reserve ammunition to partialy fill a magazine.

                ammoInMagazine += ammoInReserve;
                ammoInReserve = 0;

            }

            currentTimeToReload = timeToReload;
            state = GunState.Idle; // Exit the reloading state.

        }

        if (Input.GetKey(aimKeyCode)) { // If the aim down sights key has been pressed, set the appropriate values.

            racioHipHold = Mathf.SmoothDamp(racioHipHold, 0, ref racioHipHoldV, hipToAimSpeed);
            currentRotateSpeed = aimRotateSpeed;
            currentZoom = aimZoom;
            aiming = true;

        } else {

            racioHipHold = Mathf.SmoothDamp(racioHipHold, 1, ref racioHipHoldV, hipToAimSpeed);
            currentRotateSpeed = hipRotateSpeed;
            currentZoom = hipZoom;
            aiming = false;

        }
        
        if (Input.GetKey(shootKeyCode)) { // If the shoot key is being pressed, begin to check if the gun can shoot. 

            if (waitUntilNextFire <= 0.0f && (state != GunState.MagazineEmpty && state != GunState.Reloading)) { // If the gun has a bullet chambered, is not empty, and is not reloading.

                state = GunState.Shooting; // Change gun's state to shooting.

                // Point the gun in a random direction when fired to visually simulate spread.
                targetXRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, racioHipHold);
                targetYRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, racioHipHold);

                currentRecoilZPos -= recoilAmount; // Push the gun back to visually simulate recoil.

                // Emit a muzzle flash and smoke from the gun's muzzle.
                flashSystem.Emit(50);
                smokeSystem.Emit(10);

                shellSystem.Emit(1); // Emit an empty shell from the gun's ejection port.

                bulletController.ShootRaycastBullet(10.0f); // Use a raycast bullet class to shoot the real bullet that interacts with the world.

                waitUntilNextFire = 1.0f;

                ammoInMagazine--; // Remove a bullet from the gun's magazine.

            }

        } else { // The player has not pulled the trigger.

            if (state != GunState.Reloading && state != GunState.MagazineEmpty) { // If the gun is not empty or reloading, the gun must be idle.
                state = GunState.Idle;
            }

        }

        waitUntilNextFire -= Time.deltaTime * fireSpeed; // Countdown until the next bullet is chambered.

        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref currentRecoilZPosV, recoilRecoverTime); // Recover from recoil, if any.

        // To have the gun smoothly follow the camera, we use the SmoothDamp function to calculate X and Y rotation targets while taking into conideration simulated spread and recoil.
        targetXRotation = Mathf.SmoothDamp(targetXRotation, -mouseController.mouseLook.y, ref targetXRotationV, currentRotateSpeed);
        targetYRotation = Mathf.SmoothDamp(targetYRotation, mouseController.mouseLook.x, ref targetYRotationV, currentRotateSpeed);

        transform.position = firstPersonCamera.transform.position + (Quaternion.Euler(0, targetYRotation, 0) * new Vector3(holdSide * racioHipHold + currentGunbobX, holdHeight * racioHipHold + currentGunbobY, 0) + Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(0, 0, currentRecoilZPos)); // Move the gun into position.

        transform.rotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);

    }
}
