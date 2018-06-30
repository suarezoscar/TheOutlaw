using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    [SerializeField]
    Camera camara;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private Transform explosionPosition;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip sonido;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Explosion").GetComponent<AudioSource>().PlayOneShot(sonido);
            Invoke("Destruir", 0.5f);
            
        }
    }
    void Destruir()
    {
        Instantiate(explosionPrefab, explosionPosition.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

}
