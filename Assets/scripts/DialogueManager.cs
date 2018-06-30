using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    Stat healthStat;

    [SerializeField]
    AudioSource audio;

    [SerializeField]
    AudioClip graciasSound;

    public GameObject dBox;
    public Text dText;
    public Image dImg;
    public bool dialogueActive;
    bool dialogueWithOptions;
    public GameObject player;

    public GameObject imgSi;
    public GameObject imgNo;
    public GameObject textWithOptions;
    public static bool answer { set; get; }


    // Use this for initialization
    void Start()
    {
        
        textWithOptions.SetActive(false);
        answer = false;
        imgNo.SetActive(true);
        imgSi.SetActive(false);
        Debug.Log("opciones por defecto: respuesta = "+answer);
    }

    // Update is called once per frame
    void Update()
    {

        if (dialogueWithOptions)
        {
            Debug.Log("opciones de dialogo Activada");
            textWithOptions.SetActive(true);
        }

        if (dialogueWithOptions && dialogueActive)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("flecha derecha pulsada");
                answer = false;
                imgNo.SetActive(true);
                imgSi.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("flecha izquierda pulsada");
                imgNo.SetActive(false);
                imgSi.SetActive(true);
                answer = true;
            }

        }

        if (dialogueActive)
        {
            player.SetActive(false);
        }

        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            dBox.SetActive(false);
            dialogueActive = false;
            player.SetActive(true);

            if (dialogueWithOptions)
            {

                if (answer)
                {
                    //si la respuesta es si
                    if (GameManager.Instance.CollectedCoins >= 20)
                    {
                        healthStat.CurrentVal = healthStat.MaxVal;
                        GameManager.Instance.CollectedCoins -= 20;
                        textWithOptions.SetActive(false);
                        this.ShowBox("Gracias Bicha mia", "McCreeDialogue", false);
                        audio.PlayOneShot(graciasSound);
                    }
                    else
                    {
                        textWithOptions.SetActive(false);
                        this.ShowBox("No trabajo gratis vaquero", "camarera", false);
                    }
                        
                }
                else
                {
                    //si la respuesta es no
                    textWithOptions.SetActive(false);
                    this.ShowBox("Vuelve cuando quieras!", "camarera", false);
                }
            }

        }

    }

    public void ShowBox(string txt, string nombreImagen, bool options)
    {
        Debug.Log("opcion: " + options);

        dialogueWithOptions = options;
        dBox.SetActive(true);
        dialogueActive = true;
        dText.text = txt;
        dImg.sprite = Resources.Load<Sprite>("Recursos/" + nombreImagen);
    }
}
