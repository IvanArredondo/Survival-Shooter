using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay = new Ray();   //notice most of the below are components we added in the inspector
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) //fire1 is built in as click
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))  //out gives us info on what we hit, also the last param makes it so that the ray only hits things in that layer
        {//this goes in here if the rays hit something
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();  //whatevr we hit, get the enemyhealth script
            if(enemyHealth != null) //since above will return null if it doesnt have an enemyhealth script
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point); 
            }
            gunLine.SetPosition (1, shootHit.point);    //whataver we hit, we now have a line from 1 to the point we hit
        }
        else    //if we dont hit something
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);  //draws a line far
        }
    }
}
