using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public SimpleShoot gun;

    public float shootCoolDownTime = 1f;
    private float shootCoolDownTimer;
    public int shootCapacity = 7;
    private int load;
    private float reload_x = 0.3f;
    private bool isload;
    private Quaternion offset;

    WirelessMotionController wirelessMotionController;
    // Start is called before the first frame update
    void Start()
    {
        wirelessMotionController = this.GetComponent<WirelessMotionController>();
        load = 7;
        isload = false;
        offset = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Controller isTrigger: " + wirelessMotionController.isTrigger);
        Debug.Log("Controller Quaternion: " + wirelessMotionController.quaternion.x + " " + wirelessMotionController.quaternion.y + " " +
            wirelessMotionController.quaternion.z + " " + wirelessMotionController.quaternion.w);
        Quaternion rotation = wirelessMotionController.quaternion * Quaternion.Inverse(offset);
        this.transform.rotation = rotation;
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            offset = wirelessMotionController.quaternion;
        }
        if (wirelessMotionController.quaternion.x >= reload_x && !isload){
            load = shootCapacity;
            isload = true;
        }
        else if(wirelessMotionController.quaternion.y < reload_x){
            isload = false;
        }
        if (shootCoolDownTimer <= 0f && wirelessMotionController.isTrigger && load >= 1){
            shootCoolDownTimer = shootCoolDownTime;
            load -= 1;
            Debug.Log("load: " + load);
            gun.GetComponent<Animator>().SetTrigger("Fire");
        }
        else{
            shootCoolDownTimer = shootCoolDownTimer - Time.deltaTime;
        }
    }
}
