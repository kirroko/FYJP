using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance = null;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    public static void SaveLevel(Level level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + level.name + ".data";

        FileStream fs = new FileStream(path, FileMode.Create);

        formatter.Serialize(fs, level.data);
        fs.Close();
    }

    public static LevelData LoadLevel(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            LevelData temp = formatter.Deserialize(fs) as LevelData;

            fs.Close();
            return temp;
        }
        else
        {
            Debug.LogError(path + " does not exists");
            return null;
        }
    }
}
