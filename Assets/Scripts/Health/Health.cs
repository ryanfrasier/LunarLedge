using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    private void Awake()
    {
        // Initialize current health in the Start or Awake method
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        // Subtract damage from current health and clamp the value to ensure its between 0 and startingHealth
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth <= 0)
        {
            // Debug.Log("Health is depleted. Implement death or damage reaction.");
        }
        else
        {
            // Debug.Log("Took damage, but still alive. Current health: " + currentHealth);
        }
    } // This closing brace was missing

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);
    }
}
