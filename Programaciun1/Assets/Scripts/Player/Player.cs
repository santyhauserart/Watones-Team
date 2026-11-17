using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private RestartLayout RestartLayout;
    [SerializeField] private Damage damage;
    public InputActionReference moveInput;
    public int life;
    public float speed;
    public Transform position;
    public Vector2 input;
    public Vector2 offset;
    public Vector2 targetPoint;
    public Vector2 lastDirection = Vector2.down;
    public Rigidbody2D rb;
    public int[] keys;
    private bool isMoving = false;
    [SerializeField] private float separation = 1.5f;
    public int maxLife = 20;


    #endregion
    void Start()
    {
        isMoving = true;
        if (GameState.playerLife > 0)
        {
            life = GameState.playerLife;
        }
        else
        {
            GameState.playerLife = life;
        }
    }
    void Update()
    {
        input = moveInput.action.ReadValue<Vector2>();
        if (life <= 0)
        {
            DeathPlayer();
        }
        if (input.magnitude > 0)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                lastDirection = new Vector2(Mathf.Sign(input.x), 0);
            }
            else
            {
                lastDirection = new Vector2(0, Mathf.Sign(input.y));
            }
        }
        offset = -lastDirection * separation;
        targetPoint = (Vector2)transform.position + offset;
    }
    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Damage>(out Damage damage))
        {
            print($"perdiste -{damage.damage} puntos de vida");
            life -= damage.damage;
            GameState.playerLife = life;
            print($"Ahora tienes {life} puntos de vida");
        }
    }
    public void Heal(int amount)
    {
        life = Mathf.Min(life + amount, maxLife);
        GameState.playerLife = life;
    }
    void DeathPlayer()
    {
        print("El jugador murio");
        RestartLayout.RestartLevel();
    }
    public void SetCanMove(bool value)
    {
        isMoving = value;
    }
}
