using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VideoToTitle : MonoBehaviour {
    [SerializeField]
    float time = 0;
    [SerializeField]
    AudioClip sonido = null;
    AudioSource audioSource = null;
    Animator anim;
    //Transform posicion = null;
    // Use this for initialization
    void Start () {
        //posicion = transform;
        anim = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) || time >= 111)
        {
            FadeToBlack();
            Invoke("Siguiente", 1f);
            

        }
        time += Time.deltaTime;

    }

    void Siguiente()
    {

        audioSource.PlayOneShot(sonido);
        SceneManager.LoadScene("Title");
    }

    void FadeToBlack()
    {
        anim.SetTrigger("fade");
    }
}
