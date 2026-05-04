using UnityEngine;
using TMPro;
using System.Collections;

public class QuestionManager : MonoBehaviour
{
    public GameObject panelPregunta;
    public TMP_Text textoPregunta;
    public TMP_Text textoResultado;
    public TMP_InputField inputRespuesta;

    private string respuestaCorrecta;
    private GameObject fresaActual;

    void Start()
    {
        panelPregunta.SetActive(false);
    }


    // Aquí registras tus preguntas y respuestas
    private string[] preguntas = {
        "¿Cuánto es 2 + 3?",
        "¿Capital de Colombia?",
        "¿Cuánto es 10 - 4?"
    };

    private string[] respuestas = {
        "5",
        "Bogotá",
        "6"
    };

    public void MostrarPregunta(GameObject fresa)
    {
        fresaActual = fresa;

        Collider2D col = fresa.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        int indice = Random.Range(0, preguntas.Length);

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
            
            
                textoResultado.text = "✅ Correcto";

                GameManager.instance.PreguntaCorrecta();

                if (fresaActual != null)
                {
                    Destroy(fresaActual);
                }
            
        }
        else
        {
            textoResultado.text = "❌ Incorrecto";

            if (fresaActual != null)
            {
                Collider2D col = fresaActual.GetComponent<Collider2D>();
                if (col != null)
                {
                    col.enabled = true;
                }
            }
        }

        StartCoroutine(CerrarPregunta());
    }

    IEnumerator CerrarPregunta()
    {
        yield return new WaitForSecondsRealtime(2f);

        panelPregunta.SetActive(false);
        Time.timeScale = 1f;
    }
}
