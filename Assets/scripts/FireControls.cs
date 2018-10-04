using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControls : NetworkBehaviour
{
    public Transform firepoint;
    public GameObject projectile;
    public float launchforce = 100;
    public int numShots;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }
	}

    [Command]
    void CmdFire()
    {
        numShots++;
        GameObject newProjectile = GameObject.Instantiate(projectile, firepoint.position, firepoint.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * launchforce);

        NetworkServer.Spawn(newProjectile);
    }

    [ClientRpc]
    void RpcShowHit()
    {
        print("i hath been smote!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;
        if(collision.gameObject.tag == "Projectile")
        {
            RpcShowHit();
        }
    }
}
