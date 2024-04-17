public class ItemStack
{
    public Item rootItem { get; protected set; }
    public int quantity { get; protected set; }

    public ItemStack(Item of, int amount)
    {
        this.rootItem = of;
        this.quantity = amount;
    }

    public ItemStack(Item of) : this(of, 1) {}

    public ItemStack(ItemStack copy) : this(copy.rootItem, 1) {}

    public ItemStack(ItemStack copy, int amount) : this(copy.rootItem, amount) {}
}
