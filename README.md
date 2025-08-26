
<img width="300" height="300" alt="CCMV2Logo" src="https://github.com/user-attachments/assets/327e6d7e-2a8d-4afb-8df8-dbf90ca6b1ce" />

# Colossal Cheat Menu (https://colossal.lol)
This is the full source code to Colossal Cheat Menu V2 (Colossal.lol), I am releasing this publically because seagate decided to release the crack (doesnt work btw) behind a pay wall, yes you read that right lmfao. Please continue the Colossal legacy, if you decide to release your own fork of the menu please make sure you keep my name somewhere! As for the whole wyvern thing, they are succeeding is fearmongering and extorting members of the cheating community for their own financial gain; ex: "We will crack your menu and dox you if you dont put it on wyvern" (which takes a % of the devs money). DO NOT LET THIS CONTINUE.

# How to make this project (almost) uncrackable (for anyone in gtag com atleast)
The reason Colosal Cheat Menu got cracked was because of 2 major vulnerabilities in how I was handling both the hash checking aswell as the basic implimentation of the keyauth variable system. If you plan on using this source to sell your own menu you will need to fix these.

### Hash Checks
```
            string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
            string gameFolder = System.IO.Path.GetDirectoryName(gameExePath);

            string[] files = Directory.GetFiles(gameFolder, "ColossalCheatMenuV2.dll", SearchOption.AllDirectories);  // Vulnerability exists here; checks for the menu DLL in the Gorilla Tag folder, this can be exploited by having a valid release dll in the root of the folder allowing you to use a modified version while still passing the hash check.

            if (files.Length > 0)
            {
                string filePath = files[0];

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Get the total length of the file
                    long fileLength = fileStream.Length;

                    // Ensure the file is long enough to contain a watermark
                    if (fileLength < 52) // Minimum watermark length (38 + 1 + 13)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Read the last 70 bytes to capture the full watermark (max expected length: 38 + 1 + 19 = 58)
                    byte[] watermarkBuffer = new byte[70]; // Increased to ensure COLOSSAL is included
                    long startPosition = fileLength - watermarkBuffer.Length;
                    fileStream.Seek(startPosition, SeekOrigin.Begin);
                    int bytesRead = fileStream.Read(watermarkBuffer, 0, watermarkBuffer.Length);
                    string watermark = System.Text.Encoding.ASCII.GetString(watermarkBuffer, 0, bytesRead).TrimEnd('\0');

                    // Find the last occurrence of the separator (:)
                    int separatorIndex = watermark.LastIndexOf(':');
                    if (separatorIndex == -1 || separatorIndex >= watermark.Length - 1)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Calculate the user ID length (from separator to end)
                    int userIdLength = watermark.Length - (separatorIndex + 1);
                    if (userIdLength < 13 || userIdLength > 19) // Allow up to 19 digits
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Find the start of the key by looking for "COLOSSAL" in the buffer
                    int keyStartIndex = watermark.IndexOf("COLOSSAL");
                    if (keyStartIndex == -1)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Total watermark length
                    int watermarkLength = (separatorIndex - keyStartIndex) + 1 + userIdLength;

                    // Calculate where the watermark starts in the file
                    long watermarkStart = fileLength - watermarkLength;

                    // Ensure we have content to hash
                    if (watermarkStart <= 0)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Reset stream position to the beginning
                    fileStream.Seek(0, SeekOrigin.Begin);

                    // Read only the content before the watermark
                    byte[] contentBuffer = new byte[watermarkStart];
                    fileStream.Read(contentBuffer, 0, (int)watermarkStart);

                    using (var memoryStream = new MemoryStream(contentBuffer))
                    {
                        using (var sha256 = SHA256.Create())
                        {
                            // Compute the hash on the content excluding the watermark
                            byte[] hashBytes = sha256.ComputeHash(memoryStream);

                            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                            if (hashString != OnGameInit.hash.ToLower() && hashString != OnGameInit.betahash.ToLower())
                            {
                                string Script = $@"
                            $path = '{filePath}'
                            $bytes = [System.IO.File]::ReadAllBytes($path)
                            for ($i = 0; $i -lt 512; $i++) {{
                                $bytes[$i] = 0
                            }}
                            [System.IO.File]::WriteAllBytes($path, $bytes)
                        ".Replace("Dp05yegDzFT75Aq2MUw0", "");
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "powershell",
                                    Arguments = $"-command \"{Script}\"",
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                });

                                BepInPatcher.SendToDiscord(true);

                                Process.Start("https://colossal.lol/rick.mp4");

                                Process.GetCurrentProcess().Kill();
                                BepInPatcher.CallThrowException(OnGameInit.anti2);
                            }
                        }
                    }
                }
            }
            else
            {
                BepInPatcher.SendToDiscord(true);
                Process.Start("https://colossal.lol/badapple.mp4");
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
```

### Keyauth Variables
Keyauth variables are not encrypted in anyway, the reason wyvern was able to emulate the server and reconstruct the menu with its variables was because of this. Adding end to end encryption will fix this issue.

# How does this work? / Security Practices
Colossal Cheat Menu V2 has lots a big security things aswell as smaller things to be a inconvenience to any threat actors, do not remove any of these (everything is there for a reason). I will not be listing everything, but here are a couple things that may confuse you. As for how it works the menu options are located at "MenuOptions.json" (the unformatted version that you can place directly into the menu if you arent selling) and the formatting script for keyauth is located at "ConvertMenuOptions.ps1" (yes its ai lmfao). You edit the "MenuOptions.json" then run the powershell script, then you copy and paste the line from "SS MenuOptions.txt" into a keyauth variable (without the title, ex: ---INFO---).

### Primary DLL, Secondary DLL
The Colossal Cheat Menu V2 project is the main menu with the menu features, the ColossalV2 project is the secondary DLL meant to be placed on the server.
### Process Ending
In Colossal Cheat Menu I use 
```
Process.GetCurrentProcess().Kill();  // This is harder to hook than Enviroment
BepInPatcher.CallThrowException(OnGameInit.anti2);  // Another layer of protection, basically spams errors etc
```
### Manual String Obfuscation
You will see many things like this, this is just a annoying thing for decompilers/deobfuscation
```
"U9yQXsMDvrqK4hMLmQtZtU9yQXsMDvrqK4hMLmQtZrU9yQXsMDvrqK4hMLmQtZuU9yQXsMDvrqK4hMLmQtZeU9yQXsMDvrqK4hMLmQtZ".Replace("U9yQXsMDvrqK4hMLmQtZ", "")
```
### Anti Emulation/Debugger
This checks if a specific method is in GorillaLocomotion.GTPlayer, if not (you arent running in gorilla tag obviously lmao) and it kills itself.
```
                if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
                {
                    Process.GetCurrentProcess().Kill();
                    CallThrowException(OnGameInit.anti2);
                }
```

### Anti Server Emulation
This is one example of some anti server emulation in Colossal Cheat Menu V2
```
gameob.name = KeyAuthApp.var("cuPftMGnrzUqlooEY"); // In BepinexPatcher, names the holder gameobject to a specific thing from the server

// In PLugin, checks if its that specific name
if (BepInPatcher.gameob.name != "LXHaiU3JVPzrj8hPYCqXBLXHaiU3JVPzrj8hPYCqXeLXHaiU3JVPzrj8hPYCqXpLXHaiU3JVPzrj8hPYCqXILXHaiU3JVPzrj8hPYCqXnLXHaiU3JVPzrj8hPYCqXPLXHaiU3JVPzrj8hPYCqXaLXHaiU3JVPzrj8hPYCqXtLXHaiU3JVPzrj8hPYCqXcLXHaiU3JVPzrj8hPYCqXhLXHaiU3JVPzrj8hPYCqXCLXHaiU3JVPzrj8hPYCqXCLXHaiU3JVPzrj8hPYCqXMLXHaiU3JVPzrj8hPYCqXVLXHaiU3JVPzrj8hPYCqX2LXHaiU3JVPzrj8hPYCqX".Replace("LXHaiU3JVPzrj8hPYCqX", ""))
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
```
# Thank you
Thank you for all of the love and support over the 5 years iv been making content for Gorilla Tag, it means a lot and all of the money I have raised has helped me reach a lot of my goals aswell as help my friends/family. Sorry if the code quality is not very good, I have been using the same project for almost 3 years and have just been re-writing parts lmfao.

Credits to Lars/LHax, Mios, Starry, 64Will64/WM, Antic and Satire for helping me and motivating me.
