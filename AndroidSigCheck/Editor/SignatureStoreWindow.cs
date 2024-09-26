using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text;

namespace tule.AndroidSigCheck.Editor
{
    public class SignatureStoreWindow : EditorWindow
    {
        string stringData;
        string stringKey;
        string stringEncrypt;

        [MenuItem("Window/Signature Encryptor")]
        static void Open()
        {
            SignatureStoreWindow window = GetWindow<SignatureStoreWindow>(true, "Signature Encryptor", true);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Input text to encrypt:");
            stringData = EditorGUILayout.TextField(stringData);
            EditorGUILayout.LabelField("Input 256-bit hex string:");
            stringKey = EditorGUILayout.TextField(stringKey);
            if (GUILayout.Button("Encrypt"))
            {
                byte[] key = HexStringToBytes(stringKey);
                byte[] data = Encoding.UTF8.GetBytes(stringData);
                byte[] encrypt = SignatureValidation.Encrypt(data, key);
                stringEncrypt = Convert.ToBase64String(encrypt);
                Debug.Log("Encryption result: " + stringEncrypt);
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Encrypted result:");
            stringEncrypt = EditorGUILayout.TextField(stringEncrypt);
            if (GUILayout.Button("Decrypt"))
            {
                byte[] key = HexStringToBytes(stringKey);
                byte[] encrypt = Convert.FromBase64String(stringEncrypt);
                byte[] decrypt = SignatureValidation.Decrypt(encrypt, key);
                if (decrypt != null)
                {
                    string stringDecrypt = Encoding.UTF8.GetString(decrypt);
                    Debug.Log("Decrypt result: " + stringDecrypt);
                }
                else
                {
                    Debug.LogError("Error while decrypting. Posible wrong key");
                }
            }
        }

        private bool CheckValidCharacter(char c)
        {
            return c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F' || c >= '0' && c <= '9';
        }

        private byte[] HexStringToBytes(string hex)
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
    }

}