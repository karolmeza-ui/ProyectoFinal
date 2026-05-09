using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement
    private Rigidbody2D rb2D; // Reference to the Rigidbody2D component
    private float move; // Variable to store movement input
    public float jumpForce = 4f; // Force applied for jumping
    private bool IsGrounded; // Flag to check if the player is on the ground
    public Transform groundCheck; // Transform to check for ground collision
    public float groundRadius = 0.1f; // Radius for ground check
    public LayerMask groundLayer; // Layer mask to identify ground objects
    private Animator animator; // Reference to the Animator component
    private int coins;
    public TMP_Text textCoins;
    public AudioSource audioSource;
    public AudioClip coinClip;
    public AudioClip barrelClip;
    public TMP_Text textLife;

    // ❤️ SISTEMA DE VIDAS
    public int lives = 3;
    public TMP_Text textLives; // opcional

    // ⏱️ INVULNERABILIDAD
    private bool isInvulnerable = false;
    public float invulnerableTime = 1f;

    public QuestionManager questionManager;

    


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        animator = GetComponent<Animator>(); // Get the Animator component attached to the player
                                             // Inicializar UI de vidas (opcional)
        if (textLives != null)
        {
            textLives.text = "Vidas: " + lives;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (questionManager != null && questionManager.panelPregunta.activeSelf)
        {
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            return;
        }
        move = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrow keys)
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y); // Set the velocity of the Rigidbody2D based on input and speed
        

        if (move != 0) 
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1); // Flip the player sprite based on movement direction
        }

        if(Input.GetButtonDown("Jump") && IsGrounded) // Check if the jump button is pressed and the player is grounded
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); // Apply an impulse force for jumping
        }

        animator.SetFloat("Speed", Mathf.Abs(move)); // Set the "Speed" parameter in the Animator to control animations based on movement speed
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y); // Set the "VerticalVelocity" parameter in the Animator to control animations based on vertical movement)
        animator.SetBool("IsGrounded", IsGrounded); // Set the "isGrounded" parameter in the Animator to control animations based on whether the player is grounded


    }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer); // Check if the player is grounded by checking for collisions with the ground layer
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(coinClip); // Play the coin collection sound effect
            Destroy(collision.gameObject);
            GameManager.instance.SumarMoneda(1);
           // coins = GameManager.instance.monedas;
           // textCoins.text = coins.ToString();
        }

        if (collision.transform.CompareTag("Fresa"))
        {
            if (questionManager != null)
            {
                questionManager.MostrarPregunta(collision.gameObject);
            }
            else
            {
                Debug.LogError("QuestionManager no está asignado en el Inspector.");
            }
        }

        if (collision.transform.CompareTag("Spikes"))
        {
            GameManager.instance.ReiniciarProgreso();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.transform.CompareTag("Final"))
        {
            if (GameManager.instance.PuedePasar())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (GameManager.instance.PuedeIntercambiar())
            {
                Debug.Log("Puedes intercambiar 10 monedas para pasar");
                // luego aquí puedes poner un panel de confirmación
                GameManager.instance.Intercambiar();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.Log("No puedes pasar, te faltan preguntas o monedas");
            }
        }

        if (collision.transform.CompareTag("Barrel"))
        {
            audioSource.PlayOneShot(barrelClip); // Play the barrel collision sound effect
            Vector2 knockbackDir = (rb2D.position - (Vector2)collision.transform.position).normalized;
            rb2D.linearVelocity = Vector2.zero; // Detener el movimiento actual del jugador
            rb2D.AddForce(knockbackDir * 3, ForceMode2D.Impulse); // Aplicar una fuerza de retroceso al jugador

            TakeDamage(); 

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>(); // Obtener los BoxColliders del barril

            foreach (BoxCollider2D col in colliders)
            {
                col.enabled = false; // Desactivar los BoxColliders del barril para evitar colisiones adicionales
            }
            collision.GetComponent<Animator>().enabled = true;
            Destroy(collision.gameObject, 0.5f); // Destruir el barril después de un breve retraso para permitir que la animación de destrucción se reproduzca
        }
       
    }
  
    // ❤️ FUNCIÓN DE DAÑO (AGREGADO)
    void TakeDamage()
    {
        if (isInvulnerable) return;

        lives--;

        // Actualizar UI
        if (textLives != null)
        {
            textLives.text = "Vidas: " + lives;
        }

        if (lives <= 0)
        {
            GameManager.instance.ReiniciarProgreso();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
    }
    // ⏱️ INVULNERABILIDAD (AGREGADO)
    System.Collections.IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            for (int i = 0; i < 5; i++)
            {
                sr.enabled = false;
                yield return new WaitForSeconds(0.1f);
                sr.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
    }
   
}

