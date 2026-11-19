using UnityEngine;
public class MenuController : MonoBehaviour
#region Qué hace el script
//Este script es el interruptor del menú del juego.
//Cuando apretás la tecla Tab, el menú aparece si estaba escondido y se esconde si estaba a la vista.
#endregion
{
    public GameObject menuCanvas;
    #region El cartel del menú
    //Es el objeto del menú (el Canvas con los botones y dibujos). Se arrastra desde el Inspector.
    //public: otros scripts lo ven y aparece en el Inspector.
    #endregion
    void Start()
    {
        menuCanvas.SetActive(false);
        #region Esconder el menú
        //Esconde el menú apenas empieza el juego, para que no aparezca solo.
        #endregion
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        #region ¿Apretaste Tab?
        //Pregunta: "¿se apretó la tecla Tab en este instante?".
        #endregion
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            #region Cambiar el menú
            //Si el menú estaba a la vista lo esconde, y si estaba escondido lo muestra. Es como un interruptor de luz.
            #endregion
            if (menuCanvas.activeSelf)
                #region ¿Quedó abierto?
                //Pregunta: "¿el menú quedó prendido después del cambio?".
                #endregion
                Debug.Log("menu abierto");
            else
                Debug.Log("menu cerrado");
        }
    } 
}