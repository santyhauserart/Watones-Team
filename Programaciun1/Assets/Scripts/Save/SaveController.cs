using UnityEngine;
using System.IO;
#region ¿Que hace este script?
// Guarda la partida -> la posicion del jugador en un archivo JSON
// Tambien carga la partida guardada
#endregion
public class SaveController : MonoBehaviour
{
    private string saveLocation;
    #region ¿Que hace start?
    // le asigna un archivo JSON a saveLocation
    // llama a LoadGame
    #endregion
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }
    #region ¿Que hace SaveGame?
    // Sobreescribe la posicion del jugador y la guarda en el archivo JSON
    #endregion
    public void SaveGame()
    {
        print("partida guardada");
        SaveData saveData = new SaveData {playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position};
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }
    #region ¿Que hace LoadGame?
    // Si, al iniciar la partida, ya existe un archivo de guardado -> lo lee y lo carga (extrae la info del JSON y la asigna a la posicion del player)
    // Si no existe un archivo de guardado, lo crea en ese instante
    #endregion
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            print("partida cargada");
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
        }
        else
        {
            SaveGame();
        }
    }
}
