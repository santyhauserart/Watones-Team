using UnityEngine;
[System.Serializable]
public class SaveData
#region Qué hace el script
//Este script es la hoja del cuaderno de guardado: dice qué datos se anotan cuando guardás la partida.
//No hace nada por sí solo: no se pega a ningún objeto. SaveController lo usa para armar la hoja, escribirla en el archivo y leerla después.
//public: otros scripts lo pueden usar. class: es un molde, como una hoja con renglones vacíos.
//SaveData: es su nombre ("datos de guardado"). No tiene : MonoBehaviour, porque no se pega a ningún objeto.
#endregion
{
    public Vector3 playerPosition;
    #region Dónde estaba el jugador
    //Es el renglón de la hoja donde se anota el lugar donde estaba parado el jugador.
    //public: tiene que ser public para que JsonUtility lo pueda guardar y leer. Si fuera private, no se guardaría.
    //Vector3: el tipo, una flechita con tres números (x: izquierda/derecha, y: arriba/abajo, z: adelante/atrás).
    //Ejemplo: (3, 2, 0) es "3 pasos a la derecha y 2 hacia arriba". En un juego 2D, z casi siempre es 0.
    //playerPosition: es el nombre del renglón. SaveController lo llena en SaveGame y lo lee en LoadGame.
    #endregion
    public string mapBoundary;
    #region Nombre de la zona
    //Es otro renglón de la hoja, pensado para anotar en qué zona del mapa estaba el jugador.
    //public: para que JsonUtility lo pueda guardar y leer.
    //string: el tipo, un texto escrito con letras. Ejemplo: "ZonaBosque".
    //mapBoundary: es el nombre del renglón ("límite del mapa"). Acá no tiene valor inicial, así que empieza vacío.
    //Ojo: en el SaveController que vi, nadie llena ni lee este renglón. Por ahora está de adorno.
    #endregion
}
