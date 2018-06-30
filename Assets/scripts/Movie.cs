using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {
    public MovieTexture movie = null;

	// Use this for initialization
	void Start () {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = movie;

        GetComponent<AudioSource>().clip = movie.audioClip;
        GetComponent<AudioSource>().spatialBlend = 0;
        GetComponent<AudioSource>().loop = true;
        movie.loop = true;
        movie.Play();
        GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
