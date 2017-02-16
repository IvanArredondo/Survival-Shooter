using UnityEngine;
using UnityEngine.UI;   //we need to include this in order to use the Slider,Text, Image, etc classes
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider; //we add all these public objects in unity by dragging and dropping to connect functionality
    public Image damageImage;
    public AudioClip deathClip; //when you lose the game
    public float flashSpeed = 5f;   //how quick it flashes red for ex when you get hurt
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); //mostly transparent and red


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;  //notice this is a reference to another script that we wrote ourselves
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake ()   //awake() always gets called right at the beginning when our game starts up
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (int amount) //notice how its public, other scripts are gonna call this when they deliver damage
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play ();        //that audio file of getting hurt that we added in unity 

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");    //this is how you invoke a trigger from the animation tab that we had set up 

        playerAudio.clip = deathClip;   //this is how you set a specific audio source
        playerAudio.Play ();

        playerMovement.enabled = false;  //disables a script from the player object
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
