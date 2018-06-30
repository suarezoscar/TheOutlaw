using UnityEngine;
using System.Collections;

public class McreeUlti : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    public bool activateUlti=false;

    private GameObject[] enemies;

    [SerializeField]
    private Camera camara;

    [SerializeField]
    AudioSource audio;

    [SerializeField]
    AudioClip sound;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (activateUlti)
        {
            Ulti();
        }
	}

    void Ulti()
    {
        //player.GetComponentInChildren<Rigidbody2D>().velocity = Vector3.zero;
        
        Debug.Log("Ulti");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        foreach (GameObject item in enemies)
        {
            if (item.GetComponent<SpriteRenderer>().isVisible && item.name!="Boss")
            {
                Destroy(item);
            }
            
        }
        //Invoke("Activar", 10f);
    }
    void Activar()
    {
        player.SetActive(true);
    }
    void Desactivar()
    {
        player.SetActive(false);
    }
    void PlaySound()
    {
        audio.PlayOneShot(sound);
    }
}
