public interface Living
{   
    public float Health();
    public float MaxHealth();
    public void TakeDamage(float amount);
    public void Heal(float amount);
    public float GetStrength();
    public delegate void HealthChange(float health);
    public event HealthChange onHealthChange;
}