using Eliseev.FolderToJson.Models;
using Newtonsoft.Json;

namespace Eliseev.FolderToJson
{
    internal static class Converter
    {
        public static void ToJson(string folderPath, string outputJsonPath, string password)
        {
            List<FileData> filesData = ConvertFilesToJson(folderPath);
            SaveToJsonFile(filesData, outputJsonPath, password);

            Console.WriteLine($"Files data converted and saved to {outputJsonPath}");
        }

        public static void FromJson(string inputJsonPath, string outputFolderPath, string password)
        {
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            List<FileData> filesData = ReadJsonFile(inputJsonPath, password);
            ConvertJsonToFiles(filesData, outputFolderPath);

            Console.WriteLine($"Files data read from {inputJsonPath} and decoded to {outputFolderPath}");
        }


        #region FromJson
        static List<FileData> ReadJsonFile(string inputPath, string password)
        {
            string jsonContent = File.ReadAllText(inputPath);
            jsonContent = CryptographyUtility.DecryptString(jsonContent, password);

            return JsonConvert.DeserializeObject<List<FileData>>(jsonContent);
        }

        static void ConvertJsonToFiles(List<FileData> filesData, string outputFolderPath)
        {
            foreach (var fileData in filesData)
            {
                try
                {
                    string filePath = Path.Combine(outputFolderPath, fileData.FileName);
                    byte[] fileBytes = Convert.FromBase64String(fileData.Base64Content);

                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    File.WriteAllBytes(filePath, fileBytes);

                    Console.WriteLine($"File {fileData.FileName} decoded and saved to {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error decoding file {fileData.FileName}: {ex.Message}");
                }
            }
        }
        #endregion

        #region ToJson
        static List<FileData> ConvertFilesToJson(string folderPath)
        {
            List<FileData> filesData = new List<FileData>();

            ProcessFilesInFolder(folderPath, folderPath, filesData);

            return filesData;
        }

        static void ProcessFilesInFolder(string basefolderPath, string folderPath, List<FileData> filesData)
        {
            var files = Directory.GetFiles(folderPath);
            foreach (var filePath in files)
            {
                ProcessFile(basefolderPath, filePath, filesData);
            }

            var subdirectories = Directory.GetDirectories(folderPath);
            foreach (var subdirectory in subdirectories)
            {
                ProcessFilesInFolder(basefolderPath, subdirectory, filesData);
            }
        }

        static void ProcessFile(string basefolderPath, string filePath, List<FileData> filesData)
        {
            try
            {
                string fileName = Path.GetRelativePath(basefolderPath, filePath);
                string base64String = FileToBase64(filePath);

                filesData.Add(new FileData
                {
                    FileName = fileName,
                    Base64Content = base64String
                });

                Console.WriteLine($"File {fileName} converted to base64");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
            }
        }

        static void SaveToJsonFile(List<FileData> filesData, string outputPath, string password)
        {
            string jsonContent = JsonConvert.SerializeObject(filesData, Formatting.Indented);

            jsonContent = CryptographyUtility.EncryptString(jsonContent, password);
            File.WriteAllText(outputPath, jsonContent);
        }

        static string FileToBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }
        #endregion

    }
}
