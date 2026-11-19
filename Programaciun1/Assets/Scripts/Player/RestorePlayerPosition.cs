using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class RestorePlayerPosition : MonoBehaviour
#region Qué hace el script
//Este script le devuelve al jugador el lugar donde estaba parado cuando vuelve de un combate.
//Es como un cartelito que dice "vos estabas acá", y al empezar la escena te lleva de vuelta a ese punto.
#endregion
{
    [SerializeField] private CinemachineCamera cam;
    //La cámara. Se arrastra desde el Inspector.

    [SerializeField] private PolygonCollider2D[] rooms;
    //Todas las salas del mapa. Se arrastran desde el Inspector, y cada una necesita un nombre distinto.

    private void Start()
    {
        if (GameState.hasSavedPosition)
        {
            Vector3 oldPosition = transform.position;
            //Anota dónde estaba el jugador antes de moverlo.

            transform.position = GameState.playerPosition;
            GameState.hasSavedPosition = false;

            if (!string.IsNullOrEmpty(GameState.currentRoom))
            //Solo toca la cámara si hay una sala anotada.
            {
                StartCoroutine(RestoreCamera(oldPosition));
            }
        }
    }

    private IEnumerator RestoreCamera(Vector3 oldPosition)
    {
        PolygonCollider2D room = null;
        foreach (PolygonCollider2D r in rooms)
        //Recorre las salas de a una.
        {
            if (r.name == GameState.currentRoom)
            {
                room = r;
                break;
            }
        }
        if (room == null)
        {
            yield break;
            //Si no encontró la sala, no hace nada.
        }

        CinemachineConfiner2D confiner = cam.GetComponent<CinemachineConfiner2D>();
        float dampingOriginal = confiner.Damping;
        float slowingOriginal = confiner.SlowingDistance;

        confiner.Damping = 0f;
        confiner.SlowingDistance = 0f;
        confiner.BoundingShape2D = room;
        confiner.InvalidateBoundingShapeCache();
        cam.OnTargetObjectWarped(transform, transform.position - oldPosition);

        yield return null;

        confiner.Damping = dampingOriginal;
        confiner.SlowingDistance = slowingOriginal;
    }
}

