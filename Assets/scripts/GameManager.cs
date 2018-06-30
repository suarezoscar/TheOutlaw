using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private Text coinTxt;

    [SerializeField]
    private Text livesTxt;

    [SerializeField]
    private Text puntuacionTxt;

    [SerializeField]
    private GameObject GameOver;

    private int puntuacion;

    private int actualLives=3;

    private int collectedCoins;

    private bool restart;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
                return instance;
        }
    }

    public GameObject CoinPrefab
    {
        get
        {
            return coinPrefab;
        }
    }

    public int CollectedCoins
    {
        get
        {
            return collectedCoins;
        }

        set
        {
            string aux = value.ToString("000");
            string aux2 = "";
            foreach (char item in aux)
            {
                aux2 += item + " ";
            }
            coinTxt.text = aux2;
            collectedCoins = value;
        }
    }

    public int Lives
    {
        get
        {
            return actualLives;
        }
        set
        {
            actualLives = value;
            livesTxt.text = value.ToString();
        }
    }

    public int Puntuacion
    {
        get
        {
            return puntuacion;
        }

        set
        {
            puntuacion = value;
            puntuacionTxt.text = value.ToString("000000");
        }
    }

    public bool Restart
    {
        get
        {
            return restart;
        }

        set
        {
            restart = value;
        }
    }

    public void OnGameOver()
    {
        if (Lives < 0)
        {
            GameOver.SetActive(true);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
