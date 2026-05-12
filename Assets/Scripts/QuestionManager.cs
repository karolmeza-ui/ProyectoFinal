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
    public GameObject panelIntercambio;
    private bool preguntaBonus = false;

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

    public void MostrarPreguntaBonus()
    {
        preguntaBonus = true;

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
        bool correcta =
        inputRespuesta.text.Trim().ToLower()
        == respuestaCorrecta.Trim().ToLower();

        // ✅ RESPUESTA CORRECTA
        if (correcta)
        {
            textoResultado.text = "Correcto";

            // 🔊 SONIDO CORRECTO
            if (audioSourceJugador != null && sonidoCorrecto != null)
            {
                audioSourceJugador.PlayOneShot(sonidoCorrecto);
            }

            // SI ES BONUS
            if (preguntaBonus)
            {
                StartCoroutine(PasarDeNivel());
                return;
            }

            preguntas.RemoveAt(indiceActual);
            respuestas.RemoveAt(indiceActual);

            preguntasRespondidas++;

            GameManager.instance.PreguntaCorrecta();

            if (fresaActual != null)
            {
                Destroy(fresaActual);
            }

            if (preguntasRespondidas >= 5)
            {
                StartCoroutine(PasarDeNivel());
                return;
            }
        }

        // ❌ RESPUESTA INCORRECTA
        else
        {
            textoResultado.text = "Incorrecto";

            // 🔊 SONIDO INCORRECTO
            if (audioSourceJugador != null && sonidoIncorrecto != null)
            {
                audioSourceJugador.PlayOneShot(sonidoIncorrecto);
            }

            // SI FALLA BONUS → REINICIA
            if (preguntaBonus)
            {
                StartCoroutine(ReiniciarNivel());
                return;
            }

            preguntas.RemoveAt(indiceActual);
            respuestas.RemoveAt(indiceActual);

            if (fresaActual != null)
            {
                Destroy(fresaActual);
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

    IEnumerator ReiniciarNivel()
    {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;

        GameManager.instance.ReiniciarProgreso();

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    IEnumerator CerrarPregunta()
    {
        yield return new WaitForSecondsRealtime(2f);
        panelPregunta.SetActive(false);
        Time.timeScale = 1f;
    }


}
