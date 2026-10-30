using UnityEngine;
#region ¿Que hace este script?
// Clase Serializable -> me permite convertirlo a JSON luego
// guarda un vector3 para la posicion del jugador
// guarda un string para los limites del mapa
#endregion
[System.Serializable]

public class SaveData
{
    public Vector3 playerPosition; // playerPosition -> para guardar la posicion del jugador
    public string mapBoundary; // mapBoundary -> para limitar la data guardada por room (sin usar de momento, tengo que ver como funciona
}
