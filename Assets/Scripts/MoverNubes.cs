using UnityEngine;

public class MoverNubes : MonoBehaviour
{
    public float velocidad = 2f;

    void Update()
    {
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);

        // Apenas salga un poco
        if (transform.position.x < -10f)
        {
            // Reaparece cerca al borde derecho
            transform.position = new Vector3(
                10f,
                transform.position.y,
                transform.position.z
            );
        }
    }
}