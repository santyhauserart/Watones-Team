using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatResult : MonoBehaviour
//Esto es solo para testear (despues borrarlo)

{
    [SerializeField] private string prototypeScene = "PrototypeScene";
    //Es el nombre de la escena del mapa, la puerta por la que volvés.

    [SerializeField] private float fleeImmunitySeconds = 3f;
    //Es cuántos segundos duerme el enemigo después de que huís. float es un número que puede tener coma, y la f es solo la forma de escribirlo.

    [SerializeField] private Player player;
    //Es el jugador. Lo arrastrás desde el Inspector. Sirve para sacarle vida al huir.

    [SerializeField] private int fleeDamage = 1;
    //Es cuánta vida perdés por huir. int es un número entero, sin coma.

    private void Update()
    {
        //Esto es solo para testear (despues borrarlo)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lose();
        }
    }
    public void Win()
    {
        if (SceneFader.IsBusy) return;
        foreach (int id in GameState.encounterGuards)
        //Es recorrer la libretita de los guardias del combate: agarra los números de a uno, y lo de adentro se hace con cada uno.
        {
            if (!GameState.defeatedGuards.Contains(id))
            //Pregunta: “¿este guardia todavía NO está anotado como derrotado?”
            {
                GameState.defeatedGuards.Add(id);
                //Anota al guardia como derrotado. Así no vuelve a aparecer en el mapa.
            }
        }
        GameState.encounterGuards.Clear();
        //Borra la libretita del combate, porque ese combate ya terminó.
        SceneFader.Load(prototypeScene);
        //SceneManager.LoadScene(prototypeScene);
        //Te lleva de vuelta al mapa. Carga la escena cuyo nombre está en prototypeScene.
    }
    public void Lose()

    //Crea la función Lose (“perder el combate”). Es public porque Flee también la llama.
    {
        if (SceneFader.IsBusy) return;
        GameState.encounterGuards.Clear();
        //Borra la libretita del combate, porque el combate terminó.

        GameState.guardPositions.Clear();
        //Borra dónde estaba parado cada guardia, así todos vuelven a su lugar de siempre.

        GameState.guardWaypoints.Clear();
        //Borra a qué punto de la ruta iba cada guardia. Todos empiezan su recorrido de cero.

        if (GameState.hasSavePoint)
        //Pregunta: “¿el jugador ya pasó por un punto de guardado?” hasSavePoint es un sí o no (bool).
        {
            //nuevo:
            GameState.currentRoom = GameState.savedRoom;

            GameState.playerPosition = GameState.savePointPosition;
            //Manda al jugador al punto de guardado.

            GameState.hasSavedPosition = true;
            //Avisa que hay una posición para usar, así el jugador aparece ahí y no en el inicio del mapa.

            GameState.defeatedGuards = new List<int>(GameState.savedDefeatedGuards);
            //Devuelve la lista de guardias derrotados a como estaba en el punto de guardado. Los guardias derrotados después de guardar reviven. 
        }
        //nuevo
        else
        {
            GameState.currentRoom = "";
        }
        SceneFader.Load(prototypeScene);
        //SceneManager.LoadScene(prototypeScene);
        //Te lleva de vuelta al mapa.
    }
    public void Flee()
    //Crea la función Flee (“huir del combate”). La llama CombatMenu cuando elegís el botón Flee.
    {
        if (SceneFader.IsBusy) return;
        player.life -= fleeDamage;
        //Te saca vida por huir.

        GameState.playerLife = player.life;
        //Anota tu vida nueva en el cuaderno que no se borra, para que al volver al mapa sigas con esa vida.

        if (player.life <= 0)
        //Pregunta: “¿te quedaste sin vida por huir?” <= es “menor o igual que”.
        {
            Lose();
            //Si te quedaste sin vida, perdiste como si te hubieran vencido.

            return;
            //Corta la función acá. Lo que está abajo no se ejecuta, porque Lose() ya te mandó al mapa. return; sin nada es “terminé, me voy”. 
        }
        GameState.encounterGuards.Clear();
        //Borra la libretita del combate. Fijate que acá no anota al guardia como derrotado, así que sigue en el mapa.

        GameState.encounterImmuneUntil = Time.time + fleeImmunitySeconds;
        //Hace dormir al enemigo. Anota la hora hasta la que no puede atraparte.
        SceneFader.Load(prototypeScene);
        //SceneManager.LoadScene(prototypeScene);
        //Te lleva de vuelta al mapa.
    }
}

