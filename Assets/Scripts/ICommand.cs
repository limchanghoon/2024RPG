public interface ICommand 
{
    void Execute();
    float GetCooldownRatio();
    void ResetCooldown();

    QuickSlotType GetQuickSlotType();

    int GetID();
}
