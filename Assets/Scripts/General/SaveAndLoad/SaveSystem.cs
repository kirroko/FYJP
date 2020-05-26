using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/**
 * This class handles the saving and loading of data 
 * 
 * It also contains a method to delete all of level's data and player's color data
 */
public class SaveSystem
{
    public static string mainDirectory = Application.persistentDataPath + "/Data/";
    public static string levelDirectory = mainDirectory + "/Levels/";
    public static string playerInfoDirectory = mainDirectory + "/PlayerInfo/";

    public static void Init()
    {
        //Check if Main directory is created, if not create it
        DirectoryInfo directoryInfo = new DirectoryInfo(mainDirectory);
        if(!directoryInfo.Exists) Directory.CreateDirectory(mainDirectory);

        //Check if Level directory is created, if not create it
        directoryInfo = new DirectoryInfo(levelDirectory);
        if (!directoryInfo.Exists) Directory.CreateDirectory(levelDirectory);

        //Check if PlayerInfo directory is created, if not create it
        directoryInfo = new DirectoryInfo(playerInfoDirectory);
        if (!directoryInfo.Exists) Directory.CreateDirectory(playerInfoDirectory);
    }

    public static void SaveLevel(Level level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = levelDirectory + "Level " + level.levelNum + ".data";

        FileStream fs = new FileStream(path, FileMode.Create);

        formatter.Serialize(fs, level.data);
        fs.Close();
    }

    public static LevelData LoadLevel(string fileName)
    {
        string path = levelDirectory + fileName + ".data";

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
            Debug.LogWarning(path + " does not exists");
            return null;
        }
    }

    public static void SaveColor(BaseColor color)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = playerInfoDirectory + color.GetMain.ToString() +".data";

        FileStream fs = new FileStream(path, FileMode.Create);

        ColorData temp = new ColorData();
        temp.locked = color.IsLocked;

        formatter.Serialize(fs, temp);
        fs.Close();
    }
    
    public static ColorData LoadColor(string fileName)
    {
        string path = playerInfoDirectory + fileName + ".data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            ColorData temp = formatter.Deserialize(fs) as ColorData;

            fs.Close();
            return temp;
        }
        else
        {
            Debug.LogWarning(path + " does not exists");
            return null;
        }
    }

    public static void DeleteAllSaveData()
    {
        DeleteFilesInDirectory(Application.persistentDataPath);
    }

    private static void DeleteFilesInDirectory(string directoryPath)
    {
        string[] directories = Directory.GetDirectories(directoryPath);
        string[] files = Directory.GetFiles(directoryPath);

        foreach (string directory in directories)
        {
            DeleteFilesInDirectory(directory);
        }

        foreach(string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
    }
}
