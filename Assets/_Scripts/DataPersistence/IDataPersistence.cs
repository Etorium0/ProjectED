using Etorium.DataPersistence.Data;

namespace Etorium.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);
    }
}