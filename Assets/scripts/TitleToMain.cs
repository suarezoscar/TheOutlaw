using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleToMain : MonoBehaviour {
    [SerializeField]
    AudioSource sonido;
    Transform posicion;

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
    // Use this for initialization
    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public void LoadGameScene()
    {
        posicion = transform;
        sonido.Play();
        
        Invoke("CambioEscena", 0.5f);
    }
    void CambioEscena()
    {
        SceneManager.LoadScene("Main");
    }
}
