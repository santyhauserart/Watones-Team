using UnityEngine;
public class Enemy : MonoBehaviour
#region Qué hace el script
//Este script es un enemigo que dispara balas mientras el jugador está dentro de su zona.
#endregion
{
    [SerializeField] private GameObject bulletPrefab;
    #region El molde de la bala
    //Es el molde de la bala que dispara. Se arrastra desde el Inspector.
    #endregion
    [SerializeField] private Player player;
    #region A quién le dispara
    //Es el jugador al que apunta. Se arrastra desde el Inspector.
    #endregion
    [SerializeField] private float fireRate = 1f;
    #region Cada cuánto dispara
    //Es cuántos segundos pasan entre una tanda y la siguiente.
    #endregion
    [SerializeField] private int maxBullets = 3;
    #region Balas por tanda
    //Es cuántas balas salen en cada tanda.
    #endregion
    [SerializeField] private int degrees = 30;
    #region Separación entre balas
    //Es cuánto se separa una bala de la otra, medido en grados. Más grados, más abierto el abanico.
    #endregion
    private float nextShootTime;
    #region Reloj de la próxima tanda
    //Es la hora a la que le toca disparar de nuevo. Es como un reloj despertador.
    #endregion
    private void OnTriggerStay2D(Collider2D collision)
    #region Algo está en la zona
    //Unity llama a esta función todo el tiempo, mientras algo se queda dentro de la zona del enemigo.
    #endregion
    {
        if (collision.GetComponent<Player>() != null && Time.time >= nextShootTime)
        #region ¿Es el jugador y toca disparar?
        //Pregunta: "¿lo que está adentro es el jugador, y además ya llegó la hora de disparar?".
        #endregion
        {
            Shoot();
            #region Disparar una tanda
            //Llama a la función de disparar (la que está más abajo).
            #endregion
            nextShootTime = Time.time + fireRate;
            #region Poner el reloj
            //Pone el reloj para la próxima tanda: la hora de ahora más fireRate segundos.
            #endregion
        }
    }
    private void Shoot()
    #region La función de disparar
    //Es la función que dispara una tanda de balas.
    #endregion
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        #region Flechita hacia el jugador
        //Calcula la flechita que va del enemigo al jugador, para saber hacia dónde apuntar.
        #endregion
        float bulletCenter = (0 + maxBullets - 1) / 2f;
        #region Encontrar la bala del medio
        //Calcula cuál es la bala del medio de la tanda.
        //Las balas se numeran 0, 1, 2... y esta cuenta encuentra el centro. Con 3 balas da 1. Con 4 balas da 1,5.
        #endregion
        for (int i = 0; i < maxBullets; i++)
        #region Repetir por cada bala
        //Es un repetidor: hace lo de adentro una vez por cada bala de la tanda.
        #endregion
        {
            float angle = (i - bulletCenter) * degrees;
            #region Cuánto girar esta bala
            //Calcula cuántos grados hay que girar esta bala respecto de la flechita hacia el jugador.
            #endregion
            Vector2 turned = Quaternion.Euler(0, 0, angle) * playerDirection;
            #region Girar la flechita
            //Gira la flechita hacia el ángulo de esta bala. El resultado es la dirección final de la bala.
            #endregion
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            #region Fabricar una bala
            //Fabrica una bala nueva copiando el molde y la guarda en una cajita llamada newBullet.
            #endregion
            newBullet.GetComponent<BulletEnemy>().SetDirection(turned);
            #region Decirle hacia dónde ir
            //Le dice a la bala hacia dónde viajar.
            #endregion
        }
    }
}
