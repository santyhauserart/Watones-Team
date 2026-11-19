using UnityEngine;

public class EnemyCombat : MonoBehaviour
//guarda la vida del enemigo de combate
{
    public int life = 50;
    public int maxLife;               // NUEVO

    private void Awake()              // NUEVO
    {                                 // NUEVO
        maxLife = life;               // NUEVO
    }                                 // NUEVO
}
