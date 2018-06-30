using UnityEngine;
using System.Collections;

public class DialogueHolder : MonoBehaviour
{

    public string dialogue;
    public string nombreImagen;
    public bool dialogueWithOptions;
    public GameObject bubbleTip;

    private DialogueManager dMan;

    bool inRange = false;

    // Use this for initialization
    void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
        bubbleTip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Tecla E pulsada");
            dMan.ShowBox(dialogue, nombreImagen, dialogueWithOptions);
        }
    }

    // Walk up to the door
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            bubbleTip.SetActive(true);
            Debug.Log("en el rango");
        }
    }

    // Walk away from the door
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bubbleTip.SetActive(false);
            inRange = false;
            Debug.Log("fuera de rango");
        }

    }
}
