using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]                                               // NUEVO: esto vuelve a la clase una ficha que se llena en el Inspector
public class AttackPattern                                          // NUEVO
//Es una ficha de ataque: de dónde sale, cuántas balas y cada cuánto.
{                                                                   // NUEVO
    public string patternName = "Pattern";                          // NUEVO: solo una etiqueta para vos
    public Transform[] shootPoints;                                 // NUEVO: los puntos de donde salen las balas (objetos vacíos de la escena)
    public int bulletsPerBurst = 3;                                 // NUEVO: balas por tanda, por cada punto
    public int degrees = 30;                                        // NUEVO: qué tan abierto el abanico
    public float fireRate = 1f;                                     // NUEVO: segundos entre tanda y tanda
    public float bulletSize = 1f;
}
public class EnemyCombatAttack : MonoBehaviour
//Es el enemigo que dispara balas en su turno, y que también sabe barrer las balas cuando termina.
{
    [SerializeField] private GameObject bulletPrefab;
    //Es el molde de la bala. Desde el Inspector arrastrás ahí la bala que querés que dispare.
    //private es que nadie de afuera lo toca, y [SerializeField] es que igual aparece en el Inspector.

    [SerializeField] private Player player;
    //Es a quién le dispara: el jugador. Lo arrastrás desde el Inspector. Sirve para saber dónde está para apuntarle.

    [SerializeField] private float fireRate = 1f;
    //Es cada cuántos segundos dispara. Empieza en 1, o sea, una tanda por segundo. (float es un número con coma, y la f es solo la forma de escribirlo).

    [SerializeField] private int maxBullets = 3;
    //Es cuántas balas salen en cada tanda. int es un número entero, sin coma.

    [SerializeField] private int degrees = 30;
    //Es cuánto se separan las balas entre sí, medido en grados. Más grados, más abierto el abanico.

    private float nextShootTime;
    //Es el reloj de la próxima tanda: anota a qué hora le toca disparar de nuevo.

    private List<GameObject> bullets = new List<GameObject>();
    //Es la libretita de las balas: ahí se anota cada bala que sale, para después poder barrerlas. Arranca vacía.

    [SerializeField] private AttackPattern[] patterns;              // NUEVO: la lista de fichas de ataque
    private AttackPattern current;
    [SerializeField] private AudioSource audioSource;              // BORRAR (de acá)
    [SerializeField] private AudioClip shootSound;

    private void Awake()                                            // NUEVO (método entero)
    {                                                               // NUEVO
        if (patterns == null || patterns.Length == 0)               // NUEVO: ¿no cargaste ninguna ficha?
        {                                                           // NUEVO
            current = new AttackPattern();                          // NUEVO: armá una ficha con los valores de siempre
            current.patternName = "Default";                        // NUEVO
            current.shootPoints = new Transform[] { transform };    // NUEVO: dispara desde este mismo objeto, como antes
            current.bulletsPerBurst = maxBullets;                   // NUEVO
            current.degrees = degrees;                              // NUEVO
            current.fireRate = fireRate;                            // NUEVO
            return;                                                 // NUEVO: listo, no sigas
        }
        int index = Mathf.Clamp(GameState.attackPattern, 0, patterns.Length - 1);   // NUEVO: la notita, sin pasarse del largo de la lista
        current = patterns[index];                                  // NUEVO: sacá esa ficha
        Debug.Log("Attack pattern: " + current.patternName + " (index " + index + ")");   // NUEVO: para ver en la Console cuál tocó

        foreach (AttackPattern pattern in patterns)                 // NUEVO: primero apagá los puntos de TODAS las fichas
        {                                                           // NUEVO
            foreach (Transform point in pattern.shootPoints)        // NUEVO
            {
                if (point != transform)                             // NUEVO: "si el punto NO es este mismo objeto" (como bullet != null)
                {                                                   // NUEVO
                    point.gameObject.SetActive(false);              // IGUAL (movida adentro del if)
                }            // NUEVO
                /*point.gameObject.SetActive(false);    */              // NUEVO
            }                                                       // NUEVO
        }                                                           // NUEVO
        foreach (Transform point in current.shootPoints)            // NUEVO: después prendé solo los de la ficha elegida
        {                                                           // NUEVO
            point.gameObject.SetActive(true);                       // NUEVO
        }
    }
    private void OnEnable()
    //Es una función especial de Unity que se ejecuta cada vez que el script se enciende, o sea, cuando empieza el turno del enemigo.
    {
        nextShootTime = Time.time + 1f;
        //Es poner el reloj para dentro de 1 segundo. Time.time es “la hora de ahora” (los segundos desde que empezó el juego).
        //Así, la primera tanda sale 1 segundo después de empezar el turno, para que el jugador se prepare.
    }
    private void Update()
    {
        if (Time.time >= nextShootTime)
        //Pregunta: “¿ya llegó la hora de disparar?”. >= es “mayor o igual”.
        {
            Shoot();
            //Si ya llegó la hora, llama a la función de disparar (la que está más abajo).
            nextShootTime = Time.time + current.fireRate;           // CAMBIADO: antes fireRate; ahora el de la ficha
            //nextShootTime = Time.time + fireRate;
            //Después de disparar, pone el reloj para la próxima tanda: la hora de ahora más fireRate segundos.
        }
    }
    private void Shoot()
    //Crea la función Shoot (“disparar una tanda”). void es que no devuelve nada, solo hace su trabajo.
    {
        if (audioSource != null && shootSound != null)              // NUEVO
        {                                                           // NUEVO
            audioSource.PlayOneShot(shootSound);                    // NUEVO: un solo sonido por tanda, aunque haya varios puntos de disparo
        }
        foreach (Transform point in current.shootPoints)            // NUEVO: una tanda por cada punto de disparo
        {                                                           // NUEVO
            Vector2 playerDirection = (player.transform.position - point.position).normalized;   // CAMBIADO: mide desde el punto, antes desde transform.position

            float bulletCenter = (0 + current.bulletsPerBurst - 1) / 2f;     // CAMBIADO: antes maxBullets

            for (int i = 0; i < current.bulletsPerBurst; i++)       // CAMBIADO: antes maxBullets
            {                                                       // IGUAL
                float angle = (i - bulletCenter) * current.degrees;          // CAMBIADO: antes degrees
                Vector2 turned = Quaternion.Euler(0, 0, angle) * playerDirection;   // IGUAL

                GameObject newBullet = Instantiate(bulletPrefab, point.position, Quaternion.identity);   // CAMBIADO: nace en el punto, antes transform.position
                newBullet.transform.localScale = bulletPrefab.transform.localScale * current.bulletSize;
                newBullet.GetComponent<BulletEnemy>().SetDirection(turned);   // IGUAL
                bullets.Add(newBullet);                             // IGUAL
            }                                                       // IGUAL
        }
        //Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        ////Es la flechita que va del enemigo al jugador. Resta las posiciones para saber hacia dónde mirar,
        ////y .normalized hace la flechita de tamaño 1, para que solo diga la dirección y no la distancia.

        //float bulletCenter = (0 + maxBullets - 1) / 2f;
        ////Es cuál es la bala del medio. Las balas se numeran 0, 1, 2... y esta cuenta encuentra el centro. Con 3 balas da 1.
        ////Con 4 balas da 1,5 (por eso el 2f con coma).

        //for (int i = 0; i < maxBullets; i++)
        ////Es un repetidor: repite lo de adentro una vez por cada bala. i es el número de la bala (0, 1, 2...), empieza en 0 y sube de a uno hasta llegar a maxBullets.
        //{
        //    float angle = (i - bulletCenter) * degrees;
        //    //Es cuánto hay que girar esta bala. La del medio no gira (da 0), una gira para un lado y otra para el otro. Con 3 balas y 30 grados: -30, 0 y 30.

        //    Vector2 turned = Quaternion.Euler(0, 0, angle) * playerDirection;
        //    //Es girar la flechita hacia el ángulo de esta bala. Quaternion.Euler(0, 0, angle) es “un giro de tantos grados”, y multiplicarlo
        //    //por la flechita la hace girar. El resultado, turned, es la dirección final de esta bala.

        //    GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); 
        //    //Es fabricar una bala nueva copiando el molde. La pone donde está el enemigo (transform.position), y Quaternion.identity es “sin girar”.
        //    //La guarda en newBullet para poder darle órdenes después.

        //    newBullet.GetComponent<BulletEnemy>().SetDirection(turned); //Es decirle a la bala hacia dónde ir.
        //                                                                //Busca el script BulletEnemy de la bala y le pasa la dirección ya girada.

        //    bullets.Add(newBullet); //Es anotar la bala en la libretita, para después poder barrerla.
        //}
    }
    public void ClearBullets() //Crea la función ClearBullets (“barrer las balas”). Es public para que otros scripts la puedan llamar (como TurnManager).
    {
        foreach (GameObject bullet in bullets) 
            //Es recorrer la libretita: agarra las balas anotadas de a una, y lo de adentro se hace con cada una.
            //Mientras la recorre, a la bala del momento le llama bullet.
        {
            if (bullet != null) 
            //Pregunta: “¿esta bala todavía existe?”. != es “no es igual a”, y null es “nada”. Algunas se rompieron solas al salir de la pantalla,
            //y en ese caso en la libretita queda un renglón vacío.
            {
                Destroy(bullet);
            }
        }
        bullets.Clear(); //Borra todo lo anotado en la libretita, así queda limpia para la próxima tanda.
    }
}
