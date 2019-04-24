using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBulletController : MonoBehaviour{

    private FirstPersonGunController gunModelController;
    private RaycastHit hit;
    private int playerLayer;
    


    // Start is called before the first frame update.
    void Start(){
        gunModelController = transform.parent.GetComponentInChildren<FirstPersonGunController>();
        playerLayer = 1 << 10;
        playerLayer = ~playerLayer;
    }

    // Update is called once per frame.
    void Update(){

    }

    public void ShootRaycastBullet(float damageAmount) {

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, playerLayer)) {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); // The raycast bullet hit something.

            hit.transform.parent.SendMessageUpwards("Damage", 10.0f, SendMessageOptions.DontRequireReceiver); // When hitting a GameObject, broadcast a message upwards indicating damage.

            if (hit.transform.gameObject.CompareTag("HitBox")) {
                Instantiate(Resources.Load<GameObject>("C and R Prefabs/FX/BloodSplat_FX"), hit.transform.position, transform.rotation);
            } else{
                Instantiate(Resources.Load<GameObject>("C and R Prefabs/FX/Miss_Bullet_FX"), hit.transform.position, transform.rotation);
            }
            

            Debug.Log("Did hit.");

        } else {

            Debug.Log("Did not hit.");

        }
    }
}
