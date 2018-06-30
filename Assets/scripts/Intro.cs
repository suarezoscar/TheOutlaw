using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

    [SerializeField]
    GameObject p1;

    [SerializeField]
    Animator anim;

    float time=0;
	// Use this for initialization
	void Start () {
        anim.SetTrigger("intro");
        p1.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= 15f)
        {
            p1.SetActive(true);
            gameObject.SetActive(false);
        }
	}
}
