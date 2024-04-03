public interface Living
{
    public int maxHealth { get; }
    public int health { get; }

    public void Damage(int amount);
    public void Heal(int amount);
}