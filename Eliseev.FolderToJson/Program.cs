namespace Eliseev.FolderToJson
{
    internal class Program
    {
        static void Main()
        {
            string jsonPath = @"G:\2\FilesData.json";
            string inputFolderPath = @"G:\2";
            string outputFolderPath = @"G:\1";

            var password = "123g42gfvs2q@!@#E@!D";
            Converter.ToJson(inputFolderPath, jsonPath, password);
            Converter.FromJson(jsonPath, outputFolderPath, password);
        }
    }
}
