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

    // Start is called before the first frame update.
    void Start() {
        mouseController = GetComponentInParent(typeof(FirstPersonMouseLookController)) as FirstPersonMouseLookController; // Get the mouse controller script.
    }

    // Update is called once per frame.
    void LateUpdate() {

        currentGunbobX = Mathf.Sin(mouseController.headbobStepCounter) * gunbobAmountX * racioHipHold;
        currentGunbobY = Mathf.Cos(mouseController.headbobStepCounter * 2) * gunbobAmountY * -1 * racioHipHold;

        if (Input.GetKey(shootKeyCode)) { // The shoot key is being pressed.

            if (waitUntilNextFire <= 0.0f) { // A bullet has been chambered.

                // Point the gun in a random direction when fired.
                targetXRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, racioHipHold);
                targetYRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, racioHipHold);

                currentRecoilZPos -= recoilAmount; // Push the gun back to simulate recoil.

                // Emit muzzle flash and smoke from weapon muzzle.
                flashSystem.Emit(50);
                smokeSystem.Emit(10);

                shellSystem.Emit(1); // Emit an empty shell from the ejection port.

                waitUntilNextFire = 1.0f;

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
