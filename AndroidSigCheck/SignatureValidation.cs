using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;

namespace tule.AndroidSigCheck
{
    public static class SignatureValidation
    {
        static readonly byte[] iv = new byte[] { 0x0f, 0x37, 0x7a, 0x45, 0xed, 0xdc, 0xdd, 0xe3, 0x33, 0xca, 0x83, 0x9e, 0xf1, 0xb4, 0xe7, 0x46 };

        static byte[] signature;
        public static byte[] GetSignature()
        {
            if (signature == null)
            {
#if UNITY_EDITOR
                signature = HexStringToBytes("3A:A9:A5:C7:FC:34:C7:C3:0A:A4:A8:A0:8C:7C:0F:61:6E:06:83:20:43:AD:D4:07:E9:64:71:51:67:2A:B8:79");
#elif UNITY_ANDROID
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

                AndroidJavaClass javaSigValidation = new AndroidJavaClass("com.example.androidsigcheck.SignatureValidation");
                AndroidJavaObject javaSigByteArray = javaSigValidation.CallStatic<AndroidJavaObject>("getSignature", context);
                if (javaSigByteArray.GetRawObject().ToInt32() != 0)
                {
                    sbyte[] signed = AndroidJNI.FromSByteArray(javaSigByteArray.GetRawObject());
                    signature = (byte[])(System.Array)signed;
                }
#endif
            }
            return signature;
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            byte[] enc = null;
            using (Aes aes = Aes.Create())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                        enc = ms.ToArray();
                    }
                }
            }
            return enc;
        }

        public static byte[] Decrypt(byte[] encryptedData, byte[] key)
        {
            byte[] dec = null;
            using(Aes aes = Aes.Create())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                        {
                            cs.Write(encryptedData, 0, encryptedData.Length);
                            cs.FlushFinalBlock();
                            dec = ms.ToArray();
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
            return dec;
        }

#if UNITY_EDITOR
        private static  bool CheckValidCharacter(char c)
        {
            return c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F' || c >= '0' && c <= '9';
        }

        private static byte[] HexStringToBytes(string hex)
        {
            StringBuilder sb = new StringBuilder();
            int j = 0, count = 0;
            byte[] key = new byte[32];
            for (int i = 0; i < hex.Length; i++)
            {
                if (CheckValidCharacter(hex[i]))
                {
                    sb.Append(hex[i]);
                    count++;
                    if (count == 2)
                    {
                        key[j] = (Convert.ToByte(sb.ToString(), 16));
                        j++;
                        count = 0;
                        sb.Clear();
                    }
                }
            }
            return key;
        }
#endif
    }
}