﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using static Mono.Security.X509.X520;
using UnityEngine;
using SharpDX.Direct3D11;

namespace Colossal.Auth
{
    public class Init
    {
        public static string anti1;
        public static string anti2;
        public static string hash;
        public static string betahash;

        public static string hwid;

        public const string RegistryPath = @"SOFTWARE\ColossalCheatMenuV2";

        public static api KeyAuthApp = new api(
            name: "hWr2AQ3gMjzvCkcR8ACz5zGB8ChWr2AQ3gMjzvCkcR8ACz5zGB8ohWr2AQ3gMjzvCkcR8ACz5zGB8lhWr2AQ3gMjzvCkcR8ACz5zGB8ohWr2AQ3gMjzvCkcR8ACz5zGB8shWr2AQ3gMjzvCkcR8ACz5zGB8shWr2AQ3gMjzvCkcR8ACz5zGB8ahWr2AQ3gMjzvCkcR8ACz5zGB8lhWr2AQ3gMjzvCkcR8ACz5zGB8ChWr2AQ3gMjzvCkcR8ACz5zGB8hhWr2AQ3gMjzvCkcR8ACz5zGB8ehWr2AQ3gMjzvCkcR8ACz5zGB8ahWr2AQ3gMjzvCkcR8ACz5zGB8thWr2AQ3gMjzvCkcR8ACz5zGB8MhWr2AQ3gMjzvCkcR8ACz5zGB8ehWr2AQ3gMjzvCkcR8ACz5zGB8nhWr2AQ3gMjzvCkcR8ACz5zGB8uhWr2AQ3gMjzvCkcR8ACz5zGB8".Replace("hWr2AQ3gMjzvCkcR8ACz5zGB8", ""),
            ownerid: "U5dJGHWNYvqmWTK74a0iKCA7BkU5dJGHWNYvqmWTK74a0iKCA7BoU5dJGHWNYvqmWTK74a0iKCA7BYU5dJGHWNYvqmWTK74a0iKCA7BRU5dJGHWNYvqmWTK74a0iKCA7BJU5dJGHWNYvqmWTK74a0iKCA7BPU5dJGHWNYvqmWTK74a0iKCA7BfU5dJGHWNYvqmWTK74a0iKCA7BbU5dJGHWNYvqmWTK74a0iKCA7BzU5dJGHWNYvqmWTK74a0iKCA7BfU5dJGHWNYvqmWTK74a0iKCA7B".Replace("U5dJGHWNYvqmWTK74a0iKCA7B", ""),
            version: "1.0"
        );

        public static bool logginIn = false;

        public static void Load()
        {
            if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
            {
                Process.GetCurrentProcess().Kill();
                ThrowException();
            }


            string You_Found_The_Second_DLL = "good boy";


            while (true)
            {
                using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                {
                    byte[] buffer = new byte[32];
                    PingReply reply = ping.Send("8.8.8.8", 1000, buffer);
                    if (reply.Status == IPStatus.Success)
                    {
                        KeyAuthApp.initreal();

                        anti1 = KeyAuthApp.var("YUTh8UttMDcwKCX3t6T0QYUTh8UttMDcwKCX3t6T0AYUTh8UttMDcwKCX3t6T0HYUTh8UttMDcwKCX3t6T0dYUTh8UttMDcwKCX3t6T0ZYUTh8UttMDcwKCX3t6T0CYUTh8UttMDcwKCX3t6T02YUTh8UttMDcwKCX3t6T0rYUTh8UttMDcwKCX3t6T0uYUTh8UttMDcwKCX3t6T04YUTh8UttMDcwKCX3t6T0sYUTh8UttMDcwKCX3t6T0oYUTh8UttMDcwKCX3t6T0RYUTh8UttMDcwKCX3t6T02YUTh8UttMDcwKCX3t6T0YYUTh8UttMDcwKCX3t6T06YUTh8UttMDcwKCX3t6T0QYUTh8UttMDcwKCX3t6T0aYUTh8UttMDcwKCX3t6T03YUTh8UttMDcwKCX3t6T02YUTh8UttMDcwKCX3t6T0HYUTh8UttMDcwKCX3t6T0BYUTh8UttMDcwKCX3t6T0".Replace("YUTh8UttMDcwKCX3t6T0", ""));
                        anti2 = KeyAuthApp.var("i3Cke3DVTTwwDUvwxCtdVi3Cke3DVTTwwDUvwxCtdPi3Cke3DVTTwwDUvwxCtdZi3Cke3DVTTwwDUvwxCtdqi3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdui3Cke3DVTTwwDUvwxCtdwi3Cke3DVTTwwDUvwxCtdui3Cke3DVTTwwDUvwxCtdni3Cke3DVTTwwDUvwxCtd6i3Cke3DVTTwwDUvwxCtd6i3Cke3DVTTwwDUvwxCtdui3Cke3DVTTwwDUvwxCtd5i3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdWi3Cke3DVTTwwDUvwxCtdCi3Cke3DVTTwwDUvwxCtddi3Cke3DVTTwwDUvwxCtdEi3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdwi3Cke3DVTTwwDUvwxCtdBi3Cke3DVTTwwDUvwxCtdEi3Cke3DVTTwwDUvwxCtdni3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdXi3Cke3DVTTwwDUvwxCtdVi3Cke3DVTTwwDUvwxCtd".Replace("i3Cke3DVTTwwDUvwxCtd", ""));
                        hash = KeyAuthApp.var("4U435vYJzdWc2NoU3HFrH4U435vYJzdWc2NoU3HFra4U435vYJzdWc2NoU3HFrs4U435vYJzdWc2NoU3HFrh4U435vYJzdWc2NoU3HFr".Replace("4U435vYJzdWc2NoU3HFr", ""));
                        betahash = KeyAuthApp.var("BetaHash");

                        hwid = api.SENPAII();

                        CheckIntegrity();

                        if (anti1 == "bGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErmbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErQbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr5bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErWbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tEr6bGN4FdQGYXwKWBJrEq5tEr9bGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tEr1bGN4FdQGYXwKWBJrEq5tErcbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tErLbGN4FdQGYXwKWBJrEq5tErubGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErtbGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErjbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErVbGN4FdQGYXwKWBJrEq5tErGbGN4FdQGYXwKWBJrEq5tErgbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErUbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErnbGN4FdQGYXwKWBJrEq5tErXbGN4FdQGYXwKWBJrEq5tErZbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErkbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErBbGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErNbGN4FdQGYXwKWBJrEq5tErMbGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tEr".Replace("bGN4FdQGYXwKWBJrEq5tEr", "")) // Server-side variable check
                        {
                            var credentials = LoadCredentials();
                            if (!credentials.username.IsNullOrWhiteSpace() && !credentials.password.IsNullOrWhiteSpace())
                            {
                                KeyAuthApp.login(credentials.username, credentials.password);
                                if (KeyAuthApp.response.success)
                                {
                                    logginIn = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Process.GetCurrentProcess().Kill();
                            ThrowException();
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }
        public static (string username, string password) LoadCredentials()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
            {
                if (key != null)
                {
                    string username = (string)key.GetValue("User");
                    string encryptedPassword = (string)key.GetValue("Pass");

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(encryptedPassword))
                    {
                        return (null, null); // Return false for autologin if credentials are not set
                    }

                    string password = DecryptString(encryptedPassword);

                    return (username, password);
                }
            }
            return (null, null); // If nothing is found, return false for autologin
        }
        public static string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                Process.GetCurrentProcess().Kill();
                ThrowException();
                return null;  // Early return if the input is invalid
            }

            byte[] fullCipher = Convert.FromBase64String(cipherText);
            byte[] iv = new byte[16];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);

            using (Aes aes = Aes.Create())
            {
                string gpuId = hwid;
                string password = KeyAuthApp.var("CredentialEncryptionKey");
                aes.Key = GenerateKey(password, gpuId);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        private static byte[] GenerateKey(string password, string gpuId)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combined = System.Text.Encoding.UTF8.GetBytes(password + gpuId);
                return sha256.ComputeHash(combined);
            }
        }


        public static void ThrowException()
        {
            Process.GetCurrentProcess().Kill();
            while (true)
            {
                throw new Exception("Unauthorized access attempt. Stop trying to crack weirdo");
            }
        }
        public static void CheckIntegrity()
        {
            //string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
            //string gameFolder = System.IO.Path.GetDirectoryName(gameExePath);

            //string[] files = Directory.GetFiles(gameFolder, "ColossalCheatMenuV2.dll", SearchOption.AllDirectories);

            //if (files.Length > 0)
            //{
            //    string filePath = files[0];

            //    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //    {
            //        // Get the total length of the file
            //        long fileLength = fileStream.Length;

            //        // Ensure the file is long enough to contain a watermark
            //        if (fileLength < 52) // Minimum watermark length (38 + 1 + 13)
            //        {
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            ThrowException();
            //            return;
            //        }

            //        // Read the last 70 bytes to capture the full watermark (max expected length: 38 + 1 + 19 = 58)
            //        byte[] watermarkBuffer = new byte[70]; // Increased to ensure COLOSSAL is included
            //        long startPosition = fileLength - watermarkBuffer.Length;
            //        fileStream.Seek(startPosition, SeekOrigin.Begin);
            //        int bytesRead = fileStream.Read(watermarkBuffer, 0, watermarkBuffer.Length);
            //        string watermark = System.Text.Encoding.ASCII.GetString(watermarkBuffer, 0, bytesRead).TrimEnd('\0');

            //        // Find the last occurrence of the separator (:)
            //        int separatorIndex = watermark.LastIndexOf(':');
            //        if (separatorIndex == -1 || separatorIndex >= watermark.Length - 1)
            //        {
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            ThrowException();
            //            return;
            //        }

            //        // Calculate the user ID length (from separator to end)
            //        int userIdLength = watermark.Length - (separatorIndex + 1);
            //        if (userIdLength < 13 || userIdLength > 19) // Allow up to 19 digits
            //        {
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            ThrowException();
            //            return;
            //        }

            //        // Find the start of the key by looking for "COLOSSAL" in the buffer
            //        int keyStartIndex = watermark.IndexOf("COLOSSAL");
            //        if (keyStartIndex == -1)
            //        {
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            ThrowException();
            //            return;
            //        }

            //        // Total watermark length
            //        int watermarkLength = (separatorIndex - keyStartIndex) + 1 + userIdLength;

            //        // Calculate where the watermark starts in the file
            //        long watermarkStart = fileLength - watermarkLength;

            //        // Ensure we have content to hash
            //        if (watermarkStart <= 0)
            //        {
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            ThrowException();
            //            return;
            //        }

            //        // Reset stream position to the beginning
            //        fileStream.Seek(0, SeekOrigin.Begin);

            //        // Read only the content before the watermark
            //        byte[] contentBuffer = new byte[watermarkStart];
            //        fileStream.Read(contentBuffer, 0, (int)watermarkStart);

            //        using (var memoryStream = new MemoryStream(contentBuffer))
            //        {
            //            using (var sha256 = SHA256.Create())
            //            {
            //                // Compute the hash on the content excluding the watermark
            //                byte[] hashBytes = sha256.ComputeHash(memoryStream);

            //                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            //                if (hashString != hash.ToLower() && hashString != betahash.ToLower())
            //                {
            //                    string Script = $@"
            //                $path = '{filePath}'
            //                $bytes = [System.IO.File]::ReadAllBytes($path)
            //                for ($i = 0; $i -lt 512; $i++) {{
            //                    $bytes[$i] = 0
            //                }}
            //                [System.IO.File]::WriteAllBytes($path, $bytes)
            //            ".Replace("Dp05yegDzFT75Aq2MUw0", "");
            //                    Process.Start(new ProcessStartInfo
            //                    {
            //                        FileName = "powershell",
            //                        Arguments = $"-command \"{Script}\"",
            //                        UseShellExecute = false,
            //                        CreateNoWindow = true
            //                    });

            //                    Process.Start("https://colossal.lol/rick.mp4");

            //                    Process.GetCurrentProcess().Kill();
            //                    ThrowException();
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    Process.Start("https://colossal.lol/badapple.mp4");
            //    Process.GetCurrentProcess().Kill();
            //    ThrowException();
            //}
        }
    }
}