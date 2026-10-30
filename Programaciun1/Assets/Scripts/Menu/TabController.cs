using UnityEngine;
using UnityEngine.UI;
#region ¿Que hace este script?
// guarda 2 arrays -> aca van las imagenes, textos y botones que pertenecen a la UI del menu
// da feedback de cual boton esta seleccionado y cual pagina esta abierta
// este script NO detecta que boton pulsaste, eso lo hace un componente del inspector "Event Trigger"
#endregion
public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    void Start()
    {
        ActivateTab(0);
    }
    public void ActivateTab(int tabNO)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }
        pages[tabNO].SetActive(true);
        tabImages[tabNO].color = Color.white;
    }
}
