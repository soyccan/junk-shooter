using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public SimpleShoot gun;

    public float shootCoolDownTime = 1f;
    private float shootCoolDownTimer;

    WirelessMotionController wirelessMotionController;
    // Start is called before the first frame update
    void Start()
    {
        wirelessMotionController = this.GetComponent<WirelessMotionController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Controller isTrigger: " + wirelessMotionController.isTrigger);
        Debug.Log("Controller Quaternion: " + wirelessMotionController.quaternion);
        this.transform.rotation = wirelessMotionController.quaternion;
        if (shootCoolDownTimer <= 0f && wirelessMotionController.isTrigger){
            shootCoolDownTimer = shootCoolDownTime;
            gun.GetComponent<Animator>().SetTrigger("Fire");
        }
        else{
            shootCoolDownTimer = shootCoolDownTimer - Time.deltaTime;
        }
    }
}
