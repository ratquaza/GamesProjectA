public interface Living
{   
    public int Health();
    public int MaxHealth();
    public void Damage(int amount);
    public void Heal(int amount);
    public int DamageDealt();
    public delegate void HealthChange(int health);
    public event HealthChange onHealthChange;
}