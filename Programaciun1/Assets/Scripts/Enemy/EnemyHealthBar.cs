using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyCombat enemy;
    [SerializeField] private Image fillImage;

    private void Update()
    {
        fillImage.fillAmount = Mathf.Clamp01((float)enemy.life / enemy.maxLife);
    }
}
