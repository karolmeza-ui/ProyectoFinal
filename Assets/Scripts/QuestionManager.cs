using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioSource audioSourceJugador; // Arrastra al puerquito aquí en el Inspector
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    public GameObject panelPregunta;
    public TMP_Text textoPregunta;
    public TMP_Text textoResultado;
    public TMP_InputField inputRespuesta;

    private int indiceActual;



    private string respuestaCorrecta;
    private GameObject fresaActual;
    private int preguntasRespondidas = 0; // Para contar los aciertos

    void Start()
    {
        panelPregunta.SetActive(false);
    }


    
    public List<string> preguntas;
    public List<string> respuestas;
    
    public void MostrarPregunta(GameObject fresa)
    {
        if (preguntas.Count == 0) return;

        fresaActual = fresa;
        int indice = Random.Range(0, preguntas.Count);
        indiceActual = indice;

        textoPregunta.text = preguntas[indice];
        respuestaCorrecta = respuestas[indice];

        

        inputRespuesta.text = "";
        textoResultado.text = "";
        panelPregunta.SetActive(true);
        Time.timeScale = 0f;
    }

    
    public void VerificarRespuesta()
    {
        if (inputRespuesta.text.Trim().ToLower() == respuestaCorrecta.Trim().ToLower())
        {
            textoResultado.text = "Correcto";

            if (audioSourceJugador != null && sonidoCorrecto != null)
            {
                audioSourceJugador.PlayOneShot(sonidoCorrecto);
            }

            preguntas.RemoveAt(indiceActual);
            respuestas.RemoveAt(indiceActual);
            preguntasRespondidas++;

            GameManager.instance.PreguntaCorrecta();

          
            if (preguntasRespondidas >= 5)
            {
                StartCoroutine(PasarDeNivel());
                return;
            }
            // --- QUITA EL BLOQUE DE PASARDENIVEL DE AQUÍ ---
        }
        else
        {
            textoResultado.text = "Incorrecto";
            if (audioSourceJugador != null && sonidoIncorrecto != null)
            {
                audioSourceJugador.PlayOneShot(sonidoIncorrecto);
            }
            if (fresaActual != null)
            {
                Collider2D col = fresaActual.GetComponent<Collider2D>();
                if (col != null) col.enabled = true;
            }
        }
        if (fresaActual != null)
        {
            Destroy(fresaActual);
        }

        StartCoroutine(CerrarPregunta());

        
    }

    // --- PONLO AQUÍ, FUERA DE LAS OTRAS LLAVES ---
    IEnumerator PasarDeNivel()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator CerrarPregunta()
    {
        yield return new WaitForSecondsRealtime(2f);
        panelPregunta.SetActive(false);
        Time.timeScale = 1f;
    }
}
