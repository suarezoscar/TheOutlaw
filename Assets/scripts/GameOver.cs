using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Camera camara;

    [SerializeField]
    Player player;

    [SerializeField]
    private GameObject GO;

    [SerializeField]
    AudioSource audio;

    [SerializeField]
    AudioClip sonidoGO;

    [SerializeField]
    GameObject p1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        camara.GetComponent<AudioSource>().Pause();
        audio.PlayOneShot(sonidoGO);
        p1.SetActive(false);
    }

    //void Awake()
    //{
    //    player.GetComponent<GameObject>().SetActive(false);
    //    GameObject.FindGameObjectWithTag("Player2").SetActive(false);
    //}

    public void onReplay()
    {
        p1.SetActive(true);
        camara.GetComponent<AudioSource>().UnPause();
        //GameObject.FindGameObjectWithTag("Player2").SetActive(true);
        //player.GetComponent<GameObject>().SetActive(true);
        player.Death();
        GameManager.Instance.Restart = true;
        GameManager.Instance.Lives = 3;
        GameManager.Instance.Puntuacion = 0;
        GameManager.Instance.CollectedCoins = 0;
        GO.SetActive(false);
    }
}
