using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image fillImage;

    private void Update()
    {
        fillImage.fillAmount = Mathf.Clamp01((float)player.life / player.maxLife);
    }
}
