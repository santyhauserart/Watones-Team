using UnityEngine;
using System.IO;
public class SaveController : MonoBehaviour
#region Qué hace el script
//Este script es el que guarda y carga la partida, como un cuaderno donde anotás dónde estaba el jugador.
//Al empezar la escena busca el cuaderno (un archivo en la compu): si existe, te lleva al lugar anotado; si no, lo crea.
#endregion
{
    private string saveLocation;
    #region Dónde está el archivo
    //Es la dirección del archivo donde se guarda la partida, como la dirección de una casa.
    //No tiene valor inicial: se llena en Start. No aparece en el Inspector.
    #endregion
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        #region Armar la dirección
        //Arma la dirección completa del archivo de guardado y la anota en saveLocation.
        //= (un solo igual) significa "poné este valor".
        //Application.persistentDataPath: una carpeta que Unity elige en cada compu para guardar cosas que no se borran al cerrar el juego.
        //"saveData.json": el nombre del archivo. .json es un formato de texto, fácil de leer.
        //Path.Combine(a, b): pega la carpeta y el nombre con la barra que corresponde, así no hay que escribirla a mano.
        //Ejemplo: "carpeta/saveData.json".
        #endregion

        LoadGame();
    }
    public void SaveGame()
    {
        print("partida guardada");
        SaveData saveData = new SaveData {playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position};
        #region Armar la hoja a guardar
        //Arma una hoja nueva con lo que hay que anotar (la posición del jugador) y la guarda en la cajita saveData.
        //SaveData (la primera): el tipo, es otro script tuyo, una clase que dice qué datos se guardan. No lo vi, pero tiene que tener la variable playerPosition.
        //saveData: es el nombre de la cajita.
        //new SaveData { ... }: "creá una hoja nueva y, de paso, llenale esto". Lo que va entre llaves { } es lo que se llena.
        //playerPosition = ...: llena el campo playerPosition con el valor que sigue.
        //GameObject.FindGameObjectWithTag("Player"): "buscá en la escena el objeto que tenga la etiqueta (tag) Player".
        //.transform.position: dónde está ese objeto ahora. Es un Vector3, una flechita con tres números (x, y, z).
        //Si ningún objeto tiene el tag Player, esta línea da error.
        #endregion
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        #region Escribir el archivo
        //Escribe el archivo de guardado con la hoja que armamos.
        //File.WriteAllText(a, b): "escribí el texto b en el archivo que está en la dirección a". Si el archivo ya existía, lo pisa.
        //saveLocation: la dirección del archivo.
        //JsonUtility.ToJson(saveData): convierte la hoja en texto en formato JSON. Ejemplo: {"playerPosition":{"x":1.0,"y":2.0,"z":0.0}}.
        #endregion
    }
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        #region ¿Existe el archivo?
        //Pregunta: "¿ya hay un archivo de guardado en esa dirección?".
        //if: "si pasa esto...".
        //File.Exists(...): da true (sí) si el archivo existe y false (no) si no existe.
        #endregion
        {
            print("partida cargada");
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            #region Leer la hoja guardada
            //Lee el archivo y lo convierte otra vez en una hoja con datos, que guarda en la cajita saveData.
            //File.ReadAllText(saveLocation): "leé todo el texto del archivo".
            //JsonUtility.FromJson<SaveData>(...): "convertí este texto JSON en una hoja de tipo SaveData". Es lo contrario de ToJson.
            //<SaveData>: le dice qué tipo de hoja tiene que armar.
            #endregion
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            #region Llevar al jugador
            //Pone al jugador en el lugar que estaba anotado en la hoja.
            //GameObject.FindGameObjectWithTag("Player"): busca el objeto con el tag Player.
            //.transform.position: dónde está. = (un solo igual): "ponelo en...".
            //saveData.playerPosition: el lugar que se leyó del archivo.
            #endregion
        }
        else
        {
            SaveGame();
            #region Crear el archivo
            //Llama a la función de guardar, que crea el archivo con la posición actual del jugador.
            //Así, la próxima vez que empiece el juego, ya va a existir un archivo para cargar.
            #endregion
        }
    }
}
