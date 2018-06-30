using UnityEngine;
using System.Collections;

public class TitleFadeOut : MonoBehaviour {

    // Use this for initialization
    Animator anim;
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
    }

    void OnAwake()
    {
        FadeToImage();
    }

    void FadeToImage()
    {
        anim.SetTrigger("fade");
    }
}
