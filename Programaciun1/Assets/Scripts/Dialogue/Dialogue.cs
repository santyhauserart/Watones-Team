#region ¿Que hacen las librerias?
// 
#endregion
using System.Collections;
using UnityEngine;
using TMPro;
#region Que hace este script?
// Gestiona el dialogo
#endregion
public class Dialogue : MonoBehaviour
{
    #region ¿Que proposito cumplen las variables?
    // dialogueMark -> Me deja colocar el indicador de interaccion, a traves del inspector, a mi script para modificarlo a traves del codigo
    // dialoguePanel -> Me deja colocar el panel de dialogo, a traves del inspector, a mi script para modificarlo a traves del codigo
    // dialogueText -> Me permite colocar en el inspector el texto para utilizarlo en mi script
    // dialogueLines -> Creo un array de lineas de texto y le doy un espaciado de 4-6 lineas visibles en el inspector
    // isPlayerInRange -> Booleano para verificar si el jugador entre en un rango
    // didDialogueStart -> verifica si el dialogo inicio
    // lineIndex -> indica la linea de dialogo actual
    // typingTime -> El tiempo que tarda en pasar de un caracter a otro
    #endregion
    #region Variables
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private float typingTime = 0.05f;
    #endregion
    #region ¿Que hace Update?
    // Si el jugador entra en rango y presiona z
    // Si el dialogo no inicio -> inicia el dialogo
    // Si la linea de dialogo esta completa -> pasa a la siguiente linea
    // Si el dialogo aun no esta completo -> corta la animacion de tipeo y completa el dialogo de golpe
    #endregion
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Z))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if(dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
    }
    #region ¿Que hace StarDialogue?
    // Funcion General: inicia el dialogo
    // Avisa que el dialogo inicio
    // Visible el cuadro de dialogo
    // Invisible el icono de interaccion
    // Reinicia el LineIndex
    // timeScale = 0 _ Pone en pausa toda la física y el tiempo del juego (a nivel global)
    // timeScale no afecta a Time.unscaledDeltaTime que se usa para menu
    // Llama a ShowLine
    // StartCoroutine crea una funcion que puede pausarse y continuar por si misma, sin pausar el juego
    #endregion
    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }
    #region ¿Que hace ShowLine?
    // Funcion general: se encarga de mostrar el texto caracter x caracter
    // IEnumerator convierte un metodo normal en coroutine
    // Se le asigna un string vacio al texto
    // Por cada caracter en la linea de dialogo actual
    // Se va sumando los caracteres 1 por 1 que corresponden a dicha linea de dialogo
    // yield return pausa el tiempo entre que aparece cada caracter
    #endregion
    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach(char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }
    #region ¿Que hace NextDialogueLine?
    // Suma 1 al lineIndex
    // Si lineIndex es menor a la totalidad de las lineas de dialogo -> llama a ShowLine
    // Si lineIndex es mayor ->
    // se desactiva el panel del dialogo
    // se vuelve a activar el icono de interaccion
    // y el timeScale vuelve a 1 (el personaje se puede mover)
    #endregion
    private void NextDialogueLine()
    {
        lineIndex++;
        if(lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(true);
            Time.timeScale = 1f;
        }
    }
    #region ¿Que hace OnTriggerEnter2D?
    // Si el jugador tiene tag player y colisiona con el trigger de dialogo ->
    // El rango de interaccion se activa
    // La marca de interaccion aparece
    // Print avisa que funciona el dialogo
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
            print("se puede iniciar dialogo");
        }
    }
    #region ¿Que hace OnTriggerExit2D?
    // // Si el jugador tiene tag player sale de la colision con el trigger de dialogo ->
    // El rango de interaccion se desactiva
    // La marca de interaccion desaparece
    // Print avisa que NO funciona el dialogo
    #endregion
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
            print("NO se puede iniciar dialogo");
        }
    }
}
