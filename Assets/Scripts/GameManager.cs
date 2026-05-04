using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int monedas = 0;
    public int preguntasCorrectas = 0;
    public int preguntasNecesarias = 5;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarMoneda(int cantidad)
    {
        monedas += cantidad;
    }

    public void PreguntaCorrecta()
    {
        preguntasCorrectas++;
    }

    public bool PuedePasar()
    {
        return preguntasCorrectas >= preguntasNecesarias;
    }

    public bool PuedeIntercambiar()
    {
        return monedas >= 10;
    }

    public void Intercambiar()
    {
        monedas -= 10;
    }
    public void ReiniciarProgreso()
    {
        monedas = 0;
        preguntasCorrectas = 0;
    }
}
