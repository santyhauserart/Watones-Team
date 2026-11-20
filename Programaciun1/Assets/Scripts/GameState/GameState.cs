using System.Collections.Generic;
using UnityEngine;
public static class GameState
//es el pizarrón del juego: guarda lo que hay que recordar cuando cambiás de escena
//(dónde estaba el jugador, qué guardias derrotó, cuánta vida tiene, etc.), porque las escenas se arman de cero y se olvidan de todo.
{
    public static Vector2 playerPosition;
    //Es el lugar donde tiene que aparecer el jugador al volver al mapa. Antes del combate se anota acá dónde estaba parado.

    public static bool hasSavedPosition = false;
    //Es un cartelito de sí o no: “¿hay una posición (del jugador) anotada para usar?”.

    public static Vector2 savePointPosition;
    //Es el lugar del punto de guardado, a donde volvés si perdés. Arranca en (0, 0).

    public static bool hasSavePoint = false;
    //Es un cartelito de sí o no: “¿el jugador ya pasó por un punto de guardado?”.
    //Empieza en no. CombatResult lo mira en Lose() para decidir si te manda a un punto de guardado.

    public static List<int> defeatedGuards = new List<int>();
    //Es la libretita de los guardias derrotados. Cada renglón es el número de un guardia.

    public static List<int> encounterGuards = new List<int>();
    //Es la libretita de los guardias del combate actual: los que te agarraron.EnemyPatrol los anota
    //antes de ir al combate, y CombatResult los pasa a derrotados si ganás.

    public static List<int> savedDefeatedGuards = new List<int>();
    //Es una foto de la libretita de derrotados en el momento en que guardaste. Si perdés,
    //CombatResult la usa para que los guardias derrotados después de guardar revivan. 

    public static float encounterImmuneUntil = 0f;
    //Es la hora hasta la que el enemigo duerme después de que huís.

    public static int playerLife = -1;
    //Es la vida del jugador anotada en el pizarrón.

    public static int attackPattern = 0;                             // NUEVO: número de ficha de ataque del combate que viene (0 = la primera)
                                                                     //Es la notita que viaja del mapa al combate: el guardia que te agarra anota acá qué ataque usa.   // NUEVO

    public static Dictionary<int, Vector2> guardPositions = new Dictionary<int, Vector2>();
    //Es el cuaderno de posiciones de los guardias. Cada renglón dice: número de guardia → dónde estaba parado.

    public static Dictionary<int, int> guardWaypoints = new Dictionary<int, int>();
    //Es el cuaderno de a dónde iba cada guardia en su recorrido. Cada renglón dice: número de guardia → número del punto de ruta.

    public static List<int> collectedPickups = new List<int>();
    //Es la libretita de las cosas ya levantadas del mapa (por ejemplo, las pociones).
    //Cada renglón es el pickupId de algo que ya agarraste, para que no reaparezca. ItemPickup la usa.

    //nuevo:
    public static string currentRoom = "";
    //Es el nombre de la sala donde está la cámara ahora. "" quiere decir: ninguna anotada.
    public static string savedRoom = "";
    //Es la foto de currentRoom en el momento en que pasaste por un punto de guardado.

    public static bool saveIntroSeen = false;
}
