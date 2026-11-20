using UnityEngine;
public class CombatMenu : MonoBehaviour
//es la manito que señala los botones del combate: se mueve con las flechas y, cuando apretás Z, hace lo que dice el botón (atacar, huir o usar una poción).
{
    [SerializeField] private Transform[] slots;
    //Son las casillas donde la manito se puede parar, como las casillas de un juego de mesa.

    [SerializeField] private int currentSlot = 0;
    //Es en qué casilla está parada la manito ahora. int es un número entero. Empieza en 0, la primera.

    [SerializeField] private int columns = 2;
    //Es cuántas casillas hay por fila, como una cajita de huevos con 2 huevos por fila.

    [SerializeField] private int minDamage = 5;
    //Es el daño más chiquito que puede hacer el ataque.

    [SerializeField] private int maxDamage = 20;
    //Es el daño más grande que puede hacer el ataque.

    private CombatButton currentButton;
    //Es el botón que la manito está tocando ahora. Empieza vacío (sin botón).
    //Es private y sin [SerializeField], así que no aparece en el Inspector: el script se lo anota solo.

    [SerializeField] private EnemyCombat enemyCombat;
    //Es el enemigo al que le pegás. Lo arrastrás desde el Inspector. Sirve para sacarle vida.

    [SerializeField] private TurnManager turnManager;
    //Es el árbitro de los turnos. Sirve para decirle “ya hice mi acción, que le toque al enemigo”.

    [SerializeField] private CombatResult combatResult;
    //Es el que decide cómo termina el combate (ganar, perder o huir).

    [SerializeField] private int healAmount = 3;
    //Es cuánta vida cura una poción. Ahora vale 3.

    private void OnEnable()
    //Crea la función OnEnable, una función especial de Unity que se ejecuta cada vez que el script se enciende
    //(o sea, cada vez que empieza el turno del jugador).
    {
        transform.position = slots[currentSlot].position;
        //Pone la manito en su casilla.
        if (currentButton != null)                                      // NUEVO: si la manito ya estaba sobre un botón...
        {                                                               // NUEVO
            currentButton.SetHighlight(true);                           // NUEVO: ...volvé a iluminarlo al empezar tu turno
        }                                                               // NUEVO
    }
    private void OnDisable()                                            // NUEVO (método entero)
    {                                                                   // NUEVO
        if (currentButton != null)                                      // NUEVO
        {                                                               // NUEVO
            currentButton.SetHighlight(false);                          // NUEVO: en el turno del enemigo, nada queda iluminado
        }                                                               // NUEVO
    }                                                                   // NUEVO
    private void OnTriggerEnter2D(Collider2D collision)
    //Crea la función OnTriggerEnter2D, que Unity llama cuando algo entra en la zona de la manito.
    {
        if (collision.TryGetComponent<CombatButton>(out CombatButton combatButton))   // IGUAL
        {                                                               // IGUAL
            currentButton = combatButton;                               // IGUAL
            combatButton.SetHighlight(true);                            // NUEVO: ilumina el que tocaste
        }                                                               // IGUAL
        //if (collision.TryGetComponent<CombatButton>(out CombatButton combatButton))
        ////Pregunta: “¿lo que entró es un botón?”
        //{
        //    currentButton = combatButton;
        //    //Anota el botón que está tocando, así sabe cuál apretar con Z.
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    //Crea la función OnTriggerExit2D, que Unity llama cuando algo sale de la zona de la manito.
    {
        if (collision.TryGetComponent<CombatButton>(out CombatButton combatButton))   // CAMBIADO: saqué "&& combatButton == currentButton" de acá
        {                                                               // IGUAL
            combatButton.SetHighlight(false);                           // NUEVO: apaga siempre el que salió
            if (combatButton == currentButton)                          // NUEVO: la pregunta que sacamos de arriba, ahora acá adentro
            {                                                           // NUEVO
                currentButton = null;                                   // IGUAL (movida adentro del if nuevo)
            }                                                           // NUEVO
        }                                                               // IGUAL
        //if (collision.TryGetComponent<CombatButton>(out CombatButton combatButton) && combatButton == currentButton)
        ////Pregunta: “¿lo que salió es un botón, y además es el que yo tenía anotado?”
        ////Sirve para no olvidarse de un botón nuevo solo porque salió uno viejo.
        //{
        //    currentButton = null;
        //    //Se olvida del botón. null significa “nada”.
        //    combatButton.SetHighlight(true);
        //}
    }
    private void Update()
    {
        if (SceneFader.IsBusy) return;
        //int column = currentSlot % columns;
        //Calcula en qué columna está la manito.

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentSlot < slots.Length - 1 /*&& column != 1*/)
        //Pregunta: “¿apretaste derecha, y hay casilla a la derecha?”

        {
            currentSlot += 1;
            //Pasa a la casilla siguiente. += es “sumale”.

            transform.position = slots[currentSlot].position;
            //Mueve la manito a la casilla nueva. (Esta línea se repite en las cuatro flechas.)
        }
        //if (Input.GetKeyDown(KeyCode.DownArrow) && currentSlot + columns < slots.Length)
        ////Pregunta: “¿apretaste abajo, y hay casilla debajo?” Sumar columns es “saltar una fila entera”. Si ese número todavía
        ////es menor que la cantidad de casillas, la casilla existe.
        //{
        //    currentSlot += columns;
        //    //Baja una fila: salta tantas casillas como columnas haya.

        //    transform.position = slots[currentSlot].position;
        //    }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentSlot > 0 /*&& column != 0*/)
            {
                currentSlot -= 1;
                transform.position = slots[currentSlot].position;
            }
            //if (Input.GetKeyDown(KeyCode.UpArrow) && currentSlot - columns >= 0)
            //{
            //    currentSlot -= columns;
            //    transform.position = slots[currentSlot].position;
            //}
            if (currentButton != null && Input.GetKeyDown(KeyCode.Z))
        //Pregunta: “¿estás tocando un botón y apretaste Z?”
        {

            if (currentButton.button == "Attack")
            //Pregunta: “¿el botón que estás tocando dice Attack?” currentButton.button es
            //el texto que le escribiste al botón en el Inspector, y == compara.
            {
                int damage = Random.Range(minDamage, maxDamage + 1);
                //Tira un dado para ver cuánto daño hacés.

                enemyCombat.life -= damage;
                //Le saca vida al enemigo, tanta como salió en el dado.

                if (enemyCombat.life <= 0)
                //Pregunta: “¿al enemigo se le acabó la vida?” <= es “menor o igual que 0”.
                {
                    combatResult.Win();
                    //Si se le acabó, ganaste.
                }
                else
                //Significa “si no...”: el enemigo todavía tiene vida.
                {
                    turnManager.StartEnemyTurn();
                    //Le toca al enemigo: empieza a tirar balas
                }
            }
            if (currentButton.button == "Flee")
            //Pregunta: “¿el botón dice Flee?” (huir).
            {
                combatResult.Flee();
                //Huís del combate: perdés vida, volvés al mapa y el enemigo se queda dormidito un ratito. 
            }
            if (currentButton.button == "Item")
            //Pregunta: “¿el botón dice Item?”
            {
                Player player = turnManager.enemyTurn;
                //Agarra al jugador a través del árbitro. turnManager.enemyTurn es el Player de la escena de combate
                //(se llama así por cómo lo nombramos antes), y se lo guarda en la cajita player.

                if (player.life < player.maxLife && Inventory.Remove("Potion"))
                //Pregunta: “¿te falta vida, y además tenías una poción?”

                {
                    player.Heal(healAmount);
                    //Te cura tanta vida como diga healAmount.

                    turnManager.StartEnemyTurn();
                    //Le toca al enemigo. Usar una poción gasta tu turno.
                }
            }

        }
    }

}
