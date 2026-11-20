using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;              // NUEVO
public class Player : MonoBehaviour
#region Qué hace el script
//Este script es el jugador (el corazoncito que se mueve y esquiva).
//Se mueve con los controles, pierde vida cuando lo tocan las balas, se puede curar,
//guarda su vida en GameState para no olvidarla al cambiar de escena, y avisa cuando muere.
#endregion
{
    #region Variables
    [SerializeField] private Animator animator; // el Animator del jugador (se arrastra en el Inspector)
    [SerializeField] private RestartLayout RestartLayout;
    #region Reiniciar el nivel
    //Es el script que sabe reiniciar el nivel cuando el jugador muere. Se arrastra desde el Inspector.
    #endregion
    [SerializeField] private Damage damage;
    #region Campo de daño sin usar
    //Es una cajita para un script Damage, que dice cuánto daño hace algo. Se arrastra desde el Inspector.
    //Ojo: en OnTriggerEnter2D se crea otra "damage" con el mismo nombre que la tapa,
    //así que esta cajita no se usa para nada. No rompe nada.
    #endregion
    public InputActionReference moveInput;
    #region Control de movimiento
    //Es el cable que conecta los controles con el jugador (las flechas, WASD o el joystick).
    //Se arrastra desde el Inspector.
    #endregion
    public int life;
    #region Vida actual
    //Es cuánta vida tiene el jugador ahora.
    //Cuando empieza la escena, se pisa con la vida guardada en GameState (ver Start).
    #endregion
    public float speed;
    #region Velocidad de caminar
    //Es qué tan rápido camina el jugador. Se pone en el Inspector.
    #endregion
    public Transform position;
    #region Posición sin usar
    //Es un lugar (posición, giro y tamaño de un objeto) que se puede arrastrar desde el Inspector.
    //Transform: el tipo, "dónde está algo". En este script no se usa para nada.
    #endregion
    public Vector2 input;
    #region Flechas apretadas ahora
    //Es hacia dónde está empujando el jugador los controles.
    //Vector2: una flechita con dos números (x: izquierda/derecha, y: arriba/abajo).
    //Ejemplo: (1, 0) es derecha, (0, -1) es abajo, (0, 0) es quieto.
    #endregion
    public Vector2 offset;
    #region Distancia hacia atrás
    //Es cuánto hay que correrse hacia atrás del jugador.
    //Se calcula en Update. Es una flechita (Vector2).
    #endregion
    public Vector2 targetPoint;
    #region Punto detrás del jugador
    //Es un lugar justo detrás del jugador (según hacia dónde mira).
    //Es una posición (Vector2). Se calcula en Update. Este script no lo usa más, pero otros podrían.
    #endregion
    public Vector2 lastDirection = Vector2.down;
    #region Última dirección mirada
    //Es hacia dónde miró el jugador por última vez: arriba, abajo, izquierda o derecha.
    //= Vector2.down: empieza mirando para abajo, que es (0, -1).
    #endregion
    public Rigidbody2D rb;
    #region Cuerpo para moverse
    //Es el "cuerpo físico" del jugador. Sirve para moverlo sin atravesar paredes.
    //Rigidbody2D: el tipo, un componente de física en 2D. Se arrastra desde el Inspector.
    #endregion
    private bool isMoving = false;
    #region ¿Se puede mover?
    //Es un cartelito de sí o no: "¿el jugador se puede mover ahora?".
    //private: solo este script lo cambia (a través de SetCanMove).
    //= false: empieza en no, pero Start lo pone en sí enseguida.
    #endregion
    [SerializeField] private float separation = 1.5f;
    #region Separación hacia atrás
    //Es qué tan lejos queda el punto de atrás del jugador (usado en Update).
    #endregion
    public int maxLife = 20;
    #region Vida máxima
    //Es lo máximo de vida que puede tener el jugador, como el tope de un vaso.
    //Heal lo usa para que no te cures de más. Tiene que valer lo mismo en las dos escenas.
    #endregion

    //nuevo
    public bool inCombat = false;
    #endregion
    [SerializeField] private SpriteRenderer spriteRenderer;   // NUEVO
    [SerializeField] private float blinkTime = 0.1f;          // NUEVO
    [SerializeField] private int blinkCount = 5;              // NUEVO
    private Coroutine blinkRoutine;                           // NUEVO
    private bool isInvincible = false;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip healSound;
    void Awake()                                   // NUEVO (método nuevo, va antes de Start)
    
    {
        Debug.Log("Awake life: " + life + " / GameState: " + GameState.playerLife);// NUEVO
        if (GameState.playerLife > 0)              // CAMBIADO de lugar (antes estaba en Start)
        {                                          // CAMBIADO de lugar
            life = GameState.playerLife;           // CAMBIADO de lugar
        }                                          // CAMBIADO de lugar
        else                                       // CAMBIADO de lugar
        {                                          // CAMBIADO de lugar
            GameState.playerLife = life;           // CAMBIADO de lugar
        }                                          // CAMBIADO de lugar
    }                                              // NUEVO
    void Start()
    {
        if (animator != null)
        {
            animator.SetFloat("DirX", lastDirection.x);
            animator.SetFloat("DirY", lastDirection.y);
        }
        isMoving = true;
        #region Dejar que se mueva
        //Cambia el cartelito a sí: el jugador ya se puede mover.
        #endregion

        //if (GameState.playerLife > 0)
        #region ¿Hay vida guardada?
        //Pregunta: "¿en el pizarrón (GameState) hay una vida anotada?".
        #endregion
        //{
        //    life = GameState.playerLife;
            #region Leer la vida guardada
            //Copia la vida del pizarrón a la vida del jugador.
            //Así, si perdiste vida en el combate, no la recuperás de golpe al cambiar de escena.
            #endregion
        //}
        //else
        //{
            //GameState.playerLife = life;
            #region Anotar la vida
            //Al revés que antes: copia la vida del jugador al pizarrón, para que quede anotada.
            #endregion
        //}
    }
    public Vector2 GetTargetPoint() // NUEVO: calcula el punto de atrás en este mismo instante
    {
        return (Vector2)transform.position - lastDirection * separation; // NUEVO
    }
    void Update()
    {
        //Debug.Log("input: " + input + " magnitud: " + input.magnitude);
        input = moveInput.action.ReadValue<Vector2>();
        #region Leer los controles
        //Mira los controles y anota hacia dónde está empujando el jugador.
        #endregion
        if (life <= 0 && !inCombat && !SceneFader.IsBusy)
        #region ¿Se quedó sin vida?
        //Pregunta: "¿la vida llegó a cero o menos?".
        #endregion
        {
            SetCanMove(false);
            DeathPlayer();
            #region Morir
            //Llama a la función que hace morir al jugador (más abajo).
            #endregion
        }
        if (input.magnitude > 0)
            #region ¿Se está empujando?
            //if (animator != null)
            //{
            //    // camina si se puede mover y está empujando una dirección
            //    bool caminando = isMoving && input.magnitude > 0;
            //    animator.SetBool("Caminando", caminando);

            //    // le pasa la última dirección (se mantiene al quedarse quieto, por eso el idle mira bien)
            //    animator.SetFloat("DirX", lastDirection.x);
            //    animator.SetFloat("DirY", lastDirection.y);
            //}
        //Pregunta: "¿el jugador está apretando alguna dirección?".
        #endregion
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            #region ¿Más a los costados?
            //Pregunta: "¿empuja más hacia los costados que hacia arriba o abajo?".
            #endregion
            {
                lastDirection = new Vector2(Mathf.Sign(input.x), 0);
                #region Mirar izquierda o derecha
                //Anota que ahora mira para un costado: izquierda (-1) o derecha (1).
                //new Vector2(a, b): crea una flechita nueva con dos números.
                //Mathf.Sign(input.x): da 1 si el número es positivo y -1 si es negativo.
                //El 0 de la derecha es porque no mira ni arriba ni abajo.
                #endregion
            }
            else
            {
                lastDirection = new Vector2(0, Mathf.Sign(input.y));
                #region Mirar arriba o abajo
                //Anota que ahora mira arriba (1) o abajo (-1).
                //El 0 de la izquierda es porque no mira ni a la izquierda ni a la derecha.
                //Esto hace que solo mire en las 4 direcciones y no en diagonal.
                #endregion
            }
        }
        if (animator != null)
        {
            // camina si se puede mover y está empujando una dirección
            bool caminando = isMoving && input.magnitude > 0;
            animator.SetBool("Caminando", caminando);

            // le pasa la última dirección (se mantiene al quedarse quieto, por eso el idle mira bien)
            animator.SetFloat("DirX", lastDirection.x);
            animator.SetFloat("DirY", lastDirection.y);
        }
        offset = -lastDirection * separation;
        #region Calcular el paso atrás
        //Calcula cuánto hay que correrse hacia atrás.
        //-lastDirection: el menos da vuelta la flechita (si mira abajo, esto apunta arriba).
        //*: multiplica. separation: qué tan lejos. Ejemplo: mira abajo (0,-1) y separación 1,5 da (0, 1,5).
        #endregion

        targetPoint = GetTargetPoint(); // CAMBIADO: antes era "(Vector2)transform.position + offset", ahora usa el método
        //targetPoint = (Vector2)transform.position + offset;
        #region Calcular punto de atrás
        //Calcula un punto detrás del jugador.
        //transform.position: dónde está el jugador. Es un Vector3, con tres números.
        //(Vector2): lo convierte a dos números (x e y), porque el juego es en 2D.
        //+ offset: le suma el paso atrás.
        #endregion
    }
    void FixedUpdate()
    #region Se ejecuta con la física
    //FixedUpdate es una función especial de Unity, como Update, pero se ejecuta a intervalos
    //parejos, a la medida de la física. Es el lugar correcto para mover cosas con Rigidbody.
    //void: no devuelve nada. (): no necesita que le des nada.
    #endregion
    {
        if (isMoving)
        #region ¿Se puede mover?
        //Pregunta: "¿el cartelito dice que sí se puede mover?".
        #endregion
        {
            rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
            #region Dar un paso
            //Mueve al jugador un pasito.
            //rb.MovePosition(...): "mové el cuerpo a este lugar".
            //rb.position: dónde está ahora. + input: hacia dónde empuja.
            //* speed: qué tan rápido. * Time.fixedDeltaTime: el tiempo que pasó entre cada paso de física.
            //Con eso camina igual de rápido en cualquier compu.
            #endregion
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    #region Algo entró en el jugador
    //Unity llama a esta función cuando algo entra en la zona del jugador (por ejemplo, una bala).
    //private: nadie de afuera la llama. void: no devuelve nada.
    //Collider2D collision: "lo que entró". Le llamamos collision.
    #endregion
    {
        if (collision.TryGetComponent<Damage>(out Damage damage))
        #region ¿Eso hace daño?
        //Pregunta: "¿lo que entró hace daño?".
        //collision.TryGetComponent<Damage>: "intentá sacarle el script Damage a lo que entró".
        //out Damage damage: "si lo encontrás, dejámelo en esta cajita llamada damage".
        //Esta damage es nueva y tapa a la de arriba (la del Inspector). Usa el script Damage que está en la bala.
        //Da true si lo encontró y false si no.
        #endregion
        {
            if (isInvincible) return;
            Debug.Log($"perdiste -{damage.damage} puntos de vida");
            life -= damage.damage;
            #region Perder vida
            //Le saca vida al jugador.
            #endregion

            GameState.playerLife = life;
            SfxPlayer.Play(hurtSound);
            #region Anotar la vida nueva
            //Copia la vida nueva al pizarrón (GameState), así no se pierde al cambiar de escena.
            #endregion
            Debug.Log($"Ahora tienes {life} puntos de vida");
            StartBlink();                                                     // NUEVO
        }
    }
    public void Heal(int amount)
    #region Curar al jugador
    //Es la función para curar.
    //public: otros scripts la pueden llamar (CombatMenu la usa con la poción).
    //void: no devuelve nada.
    //Heal: es el nombre. int amount: hay que decirle cuánta vida cura.
    #endregion
    {
        life = Mathf.Min(life + amount, maxLife);
        #region Curar sin pasarse
        //Suma la vida, pero sin pasarse del máximo.
        //life + amount: la vida de ahora más lo que cura.
        //Mathf.Min(a, b): elige el número más chico de los dos.
        //Es como un vaso: si querés echar más agua de la que entra, se queda lleno y no se desborda.
        #endregion
        GameState.playerLife = life;
        SfxPlayer.Play(healSound);
        #region Anotar la vida nueva
        //Copia la vida nueva al pizarrón (GameState).
        #endregion
    }
    void DeathPlayer()
    #region Función de morir
    //Es la función que hace morir al jugador.
    //void: no devuelve nada. DeathPlayer: es su nombre. (): no necesita que le des nada.
    //No dice public ni private, y en C# eso cuenta como private (solo este script la usa).
    #endregion
    {
        Debug.Log("El jugador murio");
        RestartLayout.RestartLevel();
        #region Reiniciar el nivel
        //Le pide al script RestartLayout (el de arriba) que reinicie el nivel.
        //RestartLevel() es una función de ese otro script.
        #endregion
    }
    public void SetCanMove(bool value)
    #region Permitir o frenar movimiento
    //Es la función para frenar o dejar mover al jugador.
    //public: otros scripts la pueden llamar (EnemyPatrol la usa cuando te atrapa).
    //void: no devuelve nada.
    //SetCanMove: es el nombre. bool value: hay que decirle true (que se mueva) o false (que se quede quieto).
    #endregion
    {
        isMoving = value;
        #region Cambiar el cartelito
        //Copia lo que le dijiste al cartelito isMoving.
        //Si le decís true, se puede mover. Si le decís false, queda quieto.
        #endregion
    }
    private void StartBlink()                              // NUEVO
    {
        isInvincible = true;     // NUEVO
        if (blinkRoutine != null)                          // NUEVO
        {                                                  // NUEVO
            StopCoroutine(blinkRoutine);                   // NUEVO
        }                                                  // NUEVO
        blinkRoutine = StartCoroutine(Blink());            // NUEVO
    }                                                      // NUEVO

    private IEnumerator Blink()                            // NUEVO
    {                                                      // NUEVO
        for (int i = 0; i < blinkCount; i++)               // NUEVO
        {                                                  // NUEVO
            spriteRenderer.enabled = false;                // NUEVO
            yield return new WaitForSeconds(blinkTime);    // NUEVO
            spriteRenderer.enabled = true;                 // NUEVO
            yield return new WaitForSeconds(blinkTime);    // NUEVO
        }
        isInvincible = false;     // NUEVO
        blinkRoutine = null;                               // NUEVO
    }                                                      // NUEVO

    private void OnDisable()                               // NUEVO
    {                                                      // NUEVO
        if (blinkRoutine != null)                          // NUEVO: ¿había un parpadeo andando?
        {                                                  // NUEVO
            StopCoroutine(blinkRoutine);                   // NUEVO: cortalo
            blinkRoutine = null;                           // NUEVO
        }                                                  // NUEVO
        isInvincible = false;                              // NUEVO: limpiá la cajita
        if (spriteRenderer != null)                        // NUEVO: por si el sprite ya no existe (cambio de escena)
        {                                                  // NUEVO
            spriteRenderer.enabled = true;                 // NUEVO: que el corazón nunca quede invisible
        }                                                  // NUEVO
    }
}
