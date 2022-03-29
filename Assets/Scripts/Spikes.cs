using UnityEngine;

public class Spikes : MonoBehaviour {

    PlayerController player;
    //AudioSource ow;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //ow = GetComponent<AudioSource>();
	}

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if(col.CompareTag("Player"))
        {
            player.Damage(2);

            // Determine direction of knockback
            float dist = player.transform.position.x - transform.position.x;
            if (dist < 1)
                player.knockFromRight = true;
            else
                player.knockFromRight = false;
            
            // Set knockback timer
            player.knockbackCount = 0.2f;
     //       ow.Play();
        }
        */
    }
}
