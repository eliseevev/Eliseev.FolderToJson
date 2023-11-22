using System.Security.Cryptography;
using System.Text;

namespace Eliseev.FolderToJson
{
    public static class CryptographyUtility
    {
        public static string EncryptString(string plainText, string password)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, aesAlg.IV, 10000);
                aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
                }
            }
        }

        public static string DecryptString(string cipherText, string password)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                aesAlg.IV = cipherBytes.Take(aesAlg.BlockSize / 8).ToArray();
                cipherBytes = cipherBytes.Skip(aesAlg.BlockSize / 8).ToArray();

                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, aesAlg.IV, 10000);
                aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
