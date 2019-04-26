using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBulletController : MonoBehaviour{

    private FirstPersonGunController gunModelController;
    private RaycastHit hit;
    private int playerLayer;
    private float z = 10.0f;

    public float spreadCurrent = 0.0f;
    public float spreadMin = 0.3f;
    public float spreadMax = 1.3f;
    public float spreadPerShot = 0.1f;
    public float spreadRecovery = 0.5f;
    
    // Start is called before the first frame update.
    void Start(){
        gunModelController = transform.parent.GetComponentInChildren<FirstPersonGunController>();
        playerLayer = 1 << 10;
        playerLayer = ~playerLayer;
        spreadCurrent = spreadMin;
    }

    void Update() {

        if (gunModelController.state != FirstPersonGunController.GunState.Shooting) {
            spreadCurrent -= spreadRecovery * Time.deltaTime;
            if (spreadCurrent < spreadMin) {
                spreadCurrent = spreadMin;
            }
        }
        
    }

    public void ShootRaycastBullet(float damageAmount) {


        // Calculate the random angle of the raycast bullet.
        float randomRadius = Random.Range(0, spreadCurrent);
        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        // Calculate a random normalized direction based on the random angle and radius from the player.
        Vector3 raycastBulletTarget = new Vector3(randomRadius * Mathf.Cos(randomAngle), randomRadius * Mathf.Sin(randomAngle), z);
        raycastBulletTarget = transform.TransformDirection(raycastBulletTarget.normalized);

        // After everyshot, increase the spread amount, but not past spreadMax.
        spreadCurrent += spreadPerShot;
        if (spreadCurrent > spreadMax) {
            spreadCurrent = spreadMax;
            
        }

        
        // Fire the raycast bullet.
        if (Physics.Raycast(transform.position, raycastBulletTarget, out hit, Mathf.Infinity, playerLayer)) {

            Debug.DrawRay(transform.position, raycastBulletTarget * hit.distance, Color.yellow); // The raycast bullet hit something.

            hit.transform.parent.SendMessageUpwards("Damage", 10.0f, SendMessageOptions.DontRequireReceiver); // When hitting a GameObject, broadcast a message upwards indicating damage.

            if (hit.transform.gameObject.CompareTag("HitBox")) { // If the raycast hit an agent.

                GameObject bloodFX = Instantiate(Resources.Load<GameObject>("C and R Prefabs/FX/BloodSplat_FX"), hit.transform.position, transform.rotation);
                Destroy(bloodFX, 10.0f);

            } else { // If the raycast didn't hit an agent, it hit the environment.

                GameObject bulletholeFX = Instantiate(Resources.Load<GameObject>("C and R Prefabs/FX/Bullethole_Quad"), hit.point, transform.rotation);

                bulletholeFX.transform.forward = hit.normal * -1.0f; // Point bullethole away from the collider's normal.

                // TODO: Add random rotation code to bulletholeFX.
                // TODO: Select bullethole Quad from random selection.

                // Randomly scale bullethole.
                float randomScale = Random.Range(0.2f, 0.3f);
                bulletholeFX.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                Destroy(bulletholeFX, 10.0f); // Destroy bullethole decal after 10 seconds.

            }
            
        } else {

            // Did not hit.

        }
    }
}
