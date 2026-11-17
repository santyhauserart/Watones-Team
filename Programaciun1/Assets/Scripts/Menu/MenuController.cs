using UnityEngine;
public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    void Start()
    {
        menuCanvas.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

            if (menuCanvas.activeSelf)
                print("menu abierto");
            else
                print("menu cerrado");
        }
    } 
}