using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject panelIntercambio;
    public QuestionManager questionManager;

    public void AceptarIntercambio()
    {
        GameManager.instance.Intercambiar();

        panelIntercambio.SetActive(false);

        questionManager.MostrarPreguntaBonus();
    }

    public void CancelarIntercambio()
    {
        panelIntercambio.SetActive(false);

        Time.timeScale = 1f;
    }
}
