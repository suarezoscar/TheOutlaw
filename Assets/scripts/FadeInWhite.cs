using UnityEngine;
using System.Collections;

public class FadeInWhite : MonoBehaviour
{
    [SerializeField]
    private Transform fadeOutPosition;

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    //void OnGUI()
    //{
    //    alpha += fadeDir * fadeSpeed * Time.deltaTime;

    //    alpha = Mathf.Clamp01(alpha);

    //    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
    //    GUI.depth = drawDepth;
    //    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    //}

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //BeginFade(1);
        if (collision.gameObject.tag == "Player")
        {
            GameObject fade = GameObject.FindGameObjectWithTag("FadeIn");
            fade.GetComponent<Animator>().SetTrigger("fade");
            collision.gameObject.transform.position = new Vector3(fadeOutPosition.position.x, fadeOutPosition.position.y, 0);
        }
        
    }

}
