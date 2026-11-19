using UnityEngine;

public class BulletEnemy : MonoBehaviour
#region Qué hace el script
//Este script es la bala del enemigo: una pelotita que viaja derechito en la dirección que le dicen.
//Cuando sale de la pantalla y nadie la ve, se rompe sola.
//EnemyCombatAttack es el que fabrica las balas y les dice hacia dónde ir con SetDirection.
#endregion
{
    private Vector2 direction;
    #region Hacia dónde va
    //Es la flechita que dice hacia dónde viaja la bala.
    #endregion
    [SerializeField] private float speed = 5f;
    #region Velocidad de la bala
    //Es qué tan rápido viaja la bala. Se puede cambiar en el Inspector.
    #endregion
    public void SetDirection(Vector2 newDirection)
    #region Decirle hacia dónde ir
    //Es la función que usa el enemigo para decirle a la bala hacia dónde viajar.
    #endregion
    {
        direction = newDirection;
        #region Guardar la dirección
        //Copia la flechita que le dieron a la variable direction, para usarla después en Update.
        #endregion
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        #region Avanzar un pasito
        //Mueve la bala un pasito en su dirección. Como Update se repite muchísimo, los pasitos juntos hacen que la bala viaje.
        #endregion
    }
    private void OnBecameInvisible()
    #region Salió de la pantalla
    //Es una función especial de Unity: se ejecuta cuando la bala deja de verse en cámara (se fue de la pantalla).
    //OnBecameInvisible: es el nombre que Unity reconoce ("se volvió invisible").
    //Necesita que la bala tenga algo que se dibuje (un Sprite Renderer), porque sin dibujo Unity no sabe si se ve o no.
    #endregion
    {
        Destroy(gameObject);
    }
}
