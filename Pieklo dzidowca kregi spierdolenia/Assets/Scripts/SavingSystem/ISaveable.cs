namespace jbzd.SavingSystem
{
    public interface ISaveable
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }
}