using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Cryptography
{
    // Start is called before the first frame update
    static SHA256 _sha256 = SHA256.Create();


    public static string HashPasswordWithSalt(string str, string salt)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(str);
        byte[] saltedPasswordBytes = new byte[passwordBytes.Length + saltBytes.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
        Buffer.BlockCopy(saltBytes, 0, saltedPasswordBytes, passwordBytes.Length, saltBytes.Length);

        return Convert.ToBase64String(_sha256.ComputeHash(saltedPasswordBytes));

    }

}
