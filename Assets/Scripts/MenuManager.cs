using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelGuia; // 👈 NUEVO

    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Salir()
    {
        Application.Quit();
    }

    // 👇 NUEVO
    public void AbrirGuia()
    {
        panelGuia.SetActive(true);
    }

    public void CerrarGuia()
    {
        panelGuia.SetActive(false);
    }

}
