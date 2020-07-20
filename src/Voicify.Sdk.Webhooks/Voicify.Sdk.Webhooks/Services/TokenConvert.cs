using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    //Simple implementation from https://stackoverflow.com/questions/31339708/object-de-serializing-from-base64-in-c-sharp/31339927
    //Encryption implementation from https://stackoverflow.com/questions/202011/encrypt-and-decrypt-a-string-in-c

    public static class TokenConvert
    {
        private const string _salt = "4ed0287a-899c-48ca-96f1-0e3ecf552fe0";
        public static T DeserializeToken<T>(string token) where T : new()
        {
            byte[] obj = Convert.FromBase64String(token);
            string jsonObj = Encoding.UTF8.GetString(obj);
            T result = JsonConvert.DeserializeObject<T>(jsonObj);
            return result;
        }

        public static string SerializeToken(object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            var base64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            return base64Str;
        }

        public static T DeserializeEncryptedToken<T>(string token, string key, string saltOverride = _salt) where T : new()
        {
            string jsonObj = DecryptString(token, key, saltOverride);
            T result = JsonConvert.DeserializeObject<T>(jsonObj);
            return result;
        }

        public static string SerializeEncryptedToken(object obj, string key, string saltOverride = _salt)
        {
            var str = JsonConvert.SerializeObject(obj);
            return EncryptString(str, key, saltOverride);            
        }

        private static string EncryptString(string plainText, string sharedSecret, string salt)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            var outStr = "";                   
            RijndaelManaged aesAlg = null;   

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Encoding.ASCII.GetBytes(salt));

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        private static string DecryptString(string cipherText, string sharedSecret, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");


            RijndaelManaged aesAlg = null;
            var plaintext = "";
            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Encoding.ASCII.GetBytes(salt));

                // Create the streams used for decryption.                
                var bytes = Convert.FromBase64String(cipherText);
                using (var msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg is object)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }    
}
