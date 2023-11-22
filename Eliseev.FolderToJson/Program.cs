using System.Xml;

namespace Eliseev.FolderToJson
{
    internal class Program
    {
        static void Main()
        {
            string jsonPath = @"G:\Projects\ПСБ\CryptoProContracts\CryptoProRa\FilesData.json"; // Замените на путь к файлу JSON
            string inputFolderPath = @"G:\Projects\ПСБ\CryptoProContracts\CryptoProRa\CryptoPro.Uc2"; // Замените на путь к папке для сохранения декодированных файлов
            string outputFolderPath = @"G:\123"; // Замените на путь к папке для сохранения декодированных файлов

            Converter.ToJson(inputFolderPath, jsonPath);
            Converter.FromJson(jsonPath, outputFolderPath);
        }
    }
}
