using System.Collections;
using UnityEngine;
using TMPro;
public class Dialogue : MonoBehaviour
#region Qué hace el script
//Este script es una conversación con un personaje: cuando te acercás y apretás Z, aparece una cajita de texto
//que escribe las frases letra por letra, como una máquina de escribir.
//Si apretás Z mientras escribe, muestra la frase completa de golpe; si apretás Z con la frase completa, pasa a la siguiente.
//Mientras dura la charla, el juego queda en pausa.
#endregion
{
    [SerializeField] private GameObject dialogueMark;
    #region El cartelito de hablar
    //Es el dibujito que avisa "podés hablar conmigo" (por ejemplo, un signo de exclamación). Se arrastra desde el Inspector.
    #endregion
    [SerializeField] private GameObject dialoguePanel;
    #region La cajita de la charla
    //Es el panel donde aparece el texto de la conversación (el fondo y el texto). Se arrastra desde el Inspector.
    #endregion
    [SerializeField] private TMP_Text dialogueText;
    #region El texto que se escribe
    //Es el texto donde se van escribiendo las frases. Se arrastra desde el Inspector.
    #endregion
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    #region Las frases de la charla
    //Es la lista de frases que dice el personaje, en orden. Se escriben en el Inspector.
    //TextArea(4, 6) hace que cada frase tenga una cajita de texto grande: entre 4 y 6 renglones de alto.
    #endregion
    private bool isPlayerInRange;
    #region ¿Está cerca el jugador?
    //Es un cartelito de sí o no: "¿el jugador está lo bastante cerca para hablar?".
    #endregion
    private bool didDialogueStart;
    #region ¿Ya empezó la charla?
    //Es un cartelito de sí o no: "¿la conversación ya empezó?".
    #endregion
    private int lineIndex;
    #region Número de la frase
    //Es el número de la frase que se está diciendo ahora. Es como el dedo que va señalando la lista.
    #endregion
    private float typingTime = 0.05f;
    #region Tiempo entre letras
    //Es cuántos segundos pasan entre una letra y la siguiente cuando escribe. Más chico, más rápido escribe.
    #endregion
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Z))
        #region ¿Cerca y apretó Z?
        //Pregunta: "¿el jugador está cerca, y además apretó Z en este instante?".
        #endregion
        {
            if (!didDialogueStart)
            #region ¿Todavía no empezó?
            //Pregunta: "¿la charla todavía NO empezó?".
            #endregion
            {
                StartDialogue();
            }
            else if(dialogueText.text == dialogueLines[lineIndex])
            #region ¿Terminó de escribir?
            //Pregunta: "¿lo que está escrito en pantalla es igual a la frase completa?". O sea, "¿ya terminó de escribir esta frase?".
            #endregion
            {
                NextDialogueLine();
            }
            else
            #region Si no, está escribiendo
            //else significa "si no...": la charla ya empezó y la frase todavía se está escribiendo.
            #endregion
            {
                StopAllCoroutines();
                #region Frenar la escritura
                //Frena la máquina de escribir, o sea, la rutina que va poniendo las letras.
                #endregion
                dialogueText.text = dialogueLines[lineIndex];
                #region Mostrar la frase entera
                //Pone la frase completa de golpe, sin esperar las letras que faltaban.
                #endregion
            }
        }
    }
    private void StartDialogue()
    {
        didDialogueStart = true;
        #region Marcar que empezó
        //Cambia el cartelito a sí: la charla ya empezó.
        #endregion
        dialoguePanel.SetActive(true);
        #region Mostrar la cajita
        //Muestra el panel de la conversación.
        #endregion
        dialogueMark.SetActive(false);
        #region Esconder el cartelito
        //Esconde el cartelito de "podés hablar", porque ya estás hablando.
        #endregion
        lineIndex = 0;
        #region Volver a la primera frase
        //Pone el dedo en la primera frase de la lista (la número 0), para empezar desde el principio.
        #endregion
        Time.timeScale = 0f;
        #region Pausar el juego
        //Frena el tiempo del juego mientras dura la charla. Los guardias, las balas y todo lo demás quedan quietos.
        //Time.timeScale: qué tan rápido pasa el tiempo. 1 es normal y 0 es pausa total.
        //Ojo: el Update de este script sigue funcionando, y por eso la tecla Z anda igual.
        #endregion
        StartCoroutine(ShowLine());
        #region Empezar a escribir
        //Arranca la rutina que escribe la frase letra por letra.
        #endregion
    }
    private IEnumerator ShowLine()
    #region La máquina de escribir
    //Es la rutina que escribe la frase actual, letra por letra.
    #endregion
    {
        dialogueText.text = string.Empty;
        #region Borrar el texto
        //Deja el texto vacío, para empezar a escribir de cero. Es como borrar el pizarrón antes de escribir.
        #endregion
        foreach (char ch in dialogueLines[lineIndex])
        #region Letra por letra
        //Es un repetidor: agarra las letras de la frase de a una, y lo de adentro se hace con cada una.
        #endregion
        {
            dialogueText.text += ch;
            #region Agregar la letra
            //Agrega la letra al final de lo que ya está escrito.
            //+=: significa "sumale". Con texto, sumar es pegar. Ejemplo: "Ho" + "l" queda "Hol".
            #endregion
            yield return new WaitForSecondsRealtime(typingTime);
            #region Esperar entre letras
            //Pausa la rutina un ratito y después sigue con la letra siguiente.
            #endregion
        }
    }
    private void NextDialogueLine()
    {
        lineIndex++;
        #region Mover el dedo
        //Pasa a la frase siguiente.
        #endregion
        if (lineIndex < dialogueLines.Length)
        #region ¿Quedan frases?
        //Pregunta: "¿todavía hay una frase con ese número?".
        #endregion
        {
            StartCoroutine(ShowLine());
            #region Escribir la siguiente
            //Arranca la máquina de escribir con la frase nueva.
            #endregion
        }
        else
        #region Si no, terminó la charla
        //else significa "si no...": ya no quedan frases, así que la conversación se termina.
        #endregion
        {
            didDialogueStart = false;
            #region Marcar que terminó
            //Cambia el cartelito a no: la charla ya no está en curso. false: significa "no".
            //Sirve para que, si volvés a apretar Z, la conversación empiece de nuevo.
            #endregion
            dialoguePanel.SetActive(false);
            #region Esconder la cajita
            //Apaga el panel de la conversación, y deja de verse.
            #endregion
            dialogueMark.SetActive(true);
            #region Mostrar el cartelito
            //Vuelve a mostrar el cartelito de "podés hablar", porque el jugador sigue cerca.
            #endregion
            Time.timeScale = 1f;
            #region Despausar el juego
            //Devuelve el tiempo a la velocidad normal, y el juego sigue.
            #endregion
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            #region Marcar que está cerca
            //Cambia el cartelito a sí: el jugador está cerca y puede hablar.
            #endregion
            dialogueMark.SetActive(true);
            #region Mostrar el cartelito
            //Muestra el cartelito de "podés hablar".
            #endregion
            print("se puede iniciar dialogo");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            #region Marcar que se alejó
            //Cambia el cartelito a no: el jugador ya no está cerca, y Z ya no inicia la charla.
            #endregion
            dialogueMark.SetActive(false);
            #region Esconder el cartelito
            //Esconde el cartelito de "podés hablar", porque ya no estás cerca.
            #endregion
            print("NO se puede iniciar dialogo");
        }
    }
}
