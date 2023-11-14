using System.IO;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class SaveSystem : AGameSystem
    {
        public void LoadDefaultSaveFile(SaveFile saveFile)
        {
            SaveFileData newSaveFile = DuplicateSaveFile(saveFile.content);
            LoadSaveFile(newSaveFile);
        }

        /**
         * Never use the m_defaultSaveFile as-is, but instead, always duplicate it (deep copy) to prevent changing the initial scriptable object data.
         * This is especially useful in editor. (TODO: make it #if UNITY_EDITOR, otherwise directly use the data without cloning it)
         */
        public SaveFileData DuplicateSaveFile(SaveFileData saveFile)
        {
            string saveData = JsonUtility.ToJson(saveFile, true);
            return JsonUtility.FromJson<SaveFileData>(saveData);
        }

        public static void EraseSaveData(string saveFileName)
        {
            string filepath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, saveFileName));

            File.Delete(filepath);
        }

        public static bool TryExtractingSaveData(string saveFileName, out SaveFileData output)
        {
            string filepath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, saveFileName));

            if (!File.Exists(filepath))
            {
                output = new SaveFileData { };
                return false;
            }

            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string dataToLoad = reader.ReadToEnd();
                        output = JsonUtility.FromJson<SaveFileData>(dataToLoad);
                        return true;
                    }
                }
            }
            catch
            {
                output = new SaveFileData { };
                return false;
            }
        }

        public void LoadFromFile(string saveFileName)
        {
            Debug.Log($"Loading from {saveFileName}...");

            SaveFileData saveFile;

            if (TryExtractingSaveData(saveFileName, out saveFile))
            {
                LoadSaveFile(saveFile);
                Debug.Log($"Loading succeeded!");
            }
            else
            {
                Debug.LogError($"Loading failed!");

            }
        }

        public void SaveToFile(string saveFileName)
        {
            string filepath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, saveFileName));

            Debug.Log($"Saving to {filepath}...");

            try
            {
                SaveFileData saveFile = CreateSaveFile();

                string dataToStore = JsonUtility.ToJson(saveFile, true);

                using (FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                        Debug.Log($"Saving succeeded!");

                    }
                }
            }
            catch
            {
                Debug.LogError($"Saving failed!");
            }
        }

        public void LoadSaveFile(SaveFileData saveFile)
        {
            GameManager.GameFlagSystem.LoadDataBlock(saveFile.gameFlags);
            GameManager.InventorySystem.LoadDataBlock(saveFile.inventory);
            GameManager.JournalSystem.LoadDataBlock(saveFile.journal);
            GameManager.PlayerSystem.LoadDataBlock(saveFile.player);
            GameManager.MapLoadingSystem.RequestTransition(saveFile.map);
        }

        private string GenerateSavefileHeader()
        {
            string header;

            Hero player = GameManager.PlayerSystem.PlayerInstance;

            header = string.Format("{0} {1}{2}",
                player.characterSheet.displayName,
                GameManager.Config.GetTermDefinition("level").shortName,
                player.level
                );

            return header;
        }

        private SaveFileData CreateSaveFile()
        {
            return new SaveFileData
            {
                header = GenerateSavefileHeader(),
                map = GameManager.MapLoadingSystem.GetCurrentMapName(),
                gameFlags = GameManager.GameFlagSystem.CreateDataBlock(),
                inventory = GameManager.InventorySystem.CreateDataBlock(),
                journal = GameManager.JournalSystem.CreateDataBlock(),
                player = GameManager.PlayerSystem.CreateDataBlock()
            };
        }
    }
}
