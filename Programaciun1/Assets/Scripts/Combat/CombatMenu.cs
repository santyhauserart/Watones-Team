using UnityEngine;
public class CombatMenu : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private int currentSlot = 0;
    [SerializeField] private int columns = 2;
    [SerializeField] private int minDamage = 5;
    [SerializeField] private int maxDamage = 20;
    private CombatButton currentButton;
    [SerializeField] private EnemyCombat enemyCombat;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private CombatResult combatResult;
    
    private void OnEnable()
    {
        transform.position = slots[currentSlot].position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CombatButton>(out CombatButton combatButton))
        {
            currentButton = combatButton;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CombatButton>(out CombatButton combatButton) && combatButton == currentButton)
        {
            currentButton = null;
        }
    }
    private void Update()
    {
            int column = currentSlot % columns;
            if (Input.GetKeyDown(KeyCode.RightArrow) && currentSlot < slots.Length - 1 && column != 1)
            {
                currentSlot += 1;
                transform.position = slots[currentSlot].position;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && currentSlot + columns < slots.Length)
            {
                currentSlot += columns;
                transform.position = slots[currentSlot].position;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentSlot > 0 && column != 0)
            {
                currentSlot -= 1;
                transform.position = slots[currentSlot].position;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && currentSlot - columns >= 0)
            {
                currentSlot -= columns;
                transform.position = slots[currentSlot].position;
            }
            if (currentButton != null && Input.GetKeyDown(KeyCode.Z))
            {
                if (currentButton.button == "Attack")
                {
                    int damage = Random.Range(minDamage, maxDamage + 1);
                    enemyCombat.life -= damage;
                    if (enemyCombat.life <= 0)
                    {
                        combatResult.Win();
                    }
                    else
                    {
                        turnManager.StartEnemyTurn();
                    }
                }
            }
    }

}
