using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Nivel1()
    {
        SceneManager.LoadScene("Nivel_1");
    }
    public void Nivel2()
    {
        SceneManager.LoadScene("Nivel_2");
    }

    public void galeria()
    {
        SceneManager.LoadScene("Galeria");
    }

    public void volverMenu()
    {
        SceneManager.LoadScene("Menu_Principal");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
