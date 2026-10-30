using UnityEngine;
#region ¿Que hace este script?
// guarda el valor individual de cada instancia de llave, asi cada una abre puertas distintas
#endregion
public class KeyDoor : MonoBehaviour
{
    #region Variables
    // KeyIndex -> el numero que diferencia a las llaves
    // puertaAsociada -> se asigna en el inspector el collider de puerta que va a ser destruido
    #endregion
    public int keyIndex = 1;
    public GameObject puertaAsociada;
}
