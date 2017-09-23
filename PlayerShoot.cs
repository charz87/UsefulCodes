using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYERTAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn)
            return;
        
        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }

        }
        
    }
    // Is called on Server when a player Shoots
    [Command]
    void CmdOnShoot ()
    {
        RpcDoShootEffect();
    }

    //Is Called on all Clients when we need to do a Shoot Effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    //Is called on the server when we hit something
    //Takes in the hitpoint and the normal of the surface
    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }
    //Is called on all Clients
    //Here we can spawn in cool effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2);
    }

    [Client]//Only in Client
    void Shoot()
    {
        Debug.Log("Shoot");

        if(!isLocalPlayer)
        {
            return;
        }
        //We are shooting, call the OnShoot method on Server
        CmdOnShoot();

        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,currentWeapon.range,mask))
        {
            if(hit.collider.tag == PLAYERTAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
            //We hit something call OnHit method on Server
            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]//On the Server
    void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot");

        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
        
    }
}
