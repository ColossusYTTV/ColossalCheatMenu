﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System;
using System.Security.Cryptography.X509Certificates;
using Cryptographic;
using Colossal;
using Colossal.Patches;
using UnityEngine.Networking;
using UnityEngine;

public class api
{
    private static System.Random randomSleep = new System.Random();
    private static DateTime lastRequestTime = DateTime.MinValue; // Tracks the last request time
    private static readonly TimeSpan minRequestInterval = TimeSpan.FromSeconds(1); // Minimum 1 second between requests


    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetCurrentProcess();

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern ushort GlobalAddAtom(string lpString);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern ushort GlobalFindAtom(string lpString);

    public string name, ownerid, version, path, seed;
    /// <summary>
    /// Set up your application credentials in order to use keyauth
    /// </summary>
    /// <param name="name">Application Name</param>
    /// <param name="ownerid">Your OwnerID, found in your account settings.</param>
    /// <param name="version">Application Version, if version doesnt match it will open the download link you set up in your application settings and close the app, if empty the app will close</param>
    public api(string name, string ownerid, string version, string path = null)
    {
        if (ownerid.Length != 10)
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }

        this.name = name;
        this.ownerid = ownerid;
        this.version = version;
        this.path = path;
    }

    #region structures
    [DataContract]
    private class response_structure
    {
        [DataMember]
        public bool success { get; set; }

        [DataMember]
        public bool newSession { get; set; }

        [DataMember]
        public string sessionid { get; set; }

        [DataMember]
        public string contents { get; set; }

        [DataMember]
        public string response { get; set; }

        [DataMember]
        public string message { get; set; }

        [DataMember]
        public string ownerid { get; set; }

        [DataMember]
        public string download { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public user_data_structure info { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public app_data_structure appinfo { get; set; }

        [DataMember]
        public List<msg> messages { get; set; }

        [DataMember]
        public List<users> users { get; set; }

        [DataMember(Name = "2fa", IsRequired = false, EmitDefaultValue = false)]
        public TwoFactorData twoFactor { get; set; }
    }

    public class msg
    {
        public string message { get; set; }
        public string author { get; set; }
        public string timestamp { get; set; }
    }

    public class users
    {
        public string credential { get; set; }
    }

    [DataContract]
    private class user_data_structure
    {
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string hwid { get; set; }
        [DataMember]
        public string createdate { get; set; }
        [DataMember]
        public string lastlogin { get; set; }
        [DataMember]
        public List<Data> subscriptions { get; set; }
    }

    [DataContract]
    private class app_data_structure
    {
        [DataMember]
        public string numUsers { get; set; }
        [DataMember]
        public string numOnlineUsers { get; set; }
        [DataMember]
        public string numKeys { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string customerPanelLink { get; set; }
        [DataMember]
        public string downloadLink { get; set; }
    }
    #endregion
    private static string sessionid, enckey;
    bool initialized;
    /// <summary>
    /// Initializes the connection with keyauth in order to use any of the functions
    /// </summary>
    public void IMGONNACUM()
    {
        System.Random random = new System.Random();
        int length = random.Next(5, 51);
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            char randomChar = (char)random.Next(32, 127);
            sb.Append(randomChar);
        }

        seed = sb.ToString();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "init",
            ["ver"] = version,
            ["hash"] = fuckmegood(Process.GetCurrentProcess().MainModule.FileName),
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        if (!string.IsNullOrEmpty(path))
        {
            values_to_upload.Add("token", File.ReadAllText(path));
            values_to_upload.Add("thash", TokenHash(path));
        }

        var response = MEOW(values_to_upload);

        if (response == "KeyAuth_Invalid")
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }

        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
            if (json.success)
            {
                sessionid = json.sessionid;
                initialized = true;
            }
            else if (json.message == "invalidver")
            {
                app_data.downloadLink = json.download;
            }
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }

    public static string TokenHash(string tokenPath)
    {
        using (var sha256 = SHA256.Create())
        {
            using (var s = File.OpenRead(tokenPath))
            {
                byte[] bytes = sha256.ComputeHash(s);
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
    }
    /// <summary>
    /// Checks if Keyauth is been Initalized
    /// </summary>
    public void CheckInit()
    {
        if (!initialized)
        {
            UnityEngine.Debug.Log("Not init");
        }
    }

    /// <summary>
    /// Converts Unix time to Days,Months,Hours
    ///</summary>
    /// <param name="subscription">Subscription Number</param>
    /// <param name="Type">You can choose between Days,Hours,Months </param>
    public string expirydaysleft(string Type, int subscription)
    {
        CheckInit();

        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
        dtDateTime = dtDateTime.AddSeconds(long.Parse(user_data.subscriptions[subscription].expiry)).ToLocalTime();
        TimeSpan difference = dtDateTime - DateTime.Now;
        switch (Type.ToLower())
        {
            case "months":
                return Convert.ToString(difference.Days / 30);
            case "days":
                return Convert.ToString(difference.Days);
            case "hours":
                return Convert.ToString(difference.Hours);
        }
        return null;
    }

    /// <summary>
    /// Registers the user using a license and gives the user a subscription that matches their license level
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="pass">Password</param>
    /// <param name="key">License key</param>
    public void register(string username, string pass, string key, string email = "")
    {
        CheckInit();

        string hwid = OnGameInit.hwid;

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "register",
            ["username"] = username,
            ["pass"] = pass,
            ["key"] = key,
            ["email"] = email,
            ["hwid"] = hwid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            GlobalAddAtom(seed);
            GlobalAddAtom(ownerid);

            load_response_struct(json);
            if (json.success)
                load_user_data(json.info);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }
    /// <summary>
    /// Allow users to enter their account information and recieve an email to reset their password.
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="email">Email address</param>
    public void forgot(string username, string email)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "forgot",
            ["username"] = username,
            ["email"] = email,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);
    }
    /// <summary>
    /// Authenticates the user using their username and password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="pass">Password</param>
    public void login(string username, string pass, string code = null)
    {
        CheckInit();

        string hwid = OnGameInit.hwid;

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "login",
            ["username"] = username,
            ["pass"] = pass,
            ["hwid"] = hwid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid,
            ["code"] = code ?? string.Empty
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            GlobalAddAtom(seed);
            GlobalAddAtom(ownerid);

            load_response_struct(json);
            if (json.success)
                load_user_data(json.info);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }

    public void logout()
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "logout",
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }

    public void web_login()
    {
        CheckInit();

        string hwid = OnGameInit.hwid;

        string datastore, datastore2, outputten;

    start:

        HttpListener listener = new HttpListener();

        outputten = "handshake";
        outputten = "http://localhost:1337/" + outputten + "/";

        listener.Prefixes.Add(outputten);

        listener.Start();

        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;
        HttpListenerResponse responsepp = context.Response;

        responsepp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
        responsepp.AddHeader("Access-Control-Allow-Origin", "*");
        responsepp.AddHeader("Via", "hugzho's big brain");
        responsepp.AddHeader("Location", "your kernel ;)");
        responsepp.AddHeader("Retry-After", "never lmao");
        responsepp.Headers.Add("Server", "\r\n\r\n");

        if (request.HttpMethod == "OPTIONS")
        {
            responsepp.StatusCode = (int)HttpStatusCode.OK;
            Thread.Sleep(1);
            listener.Stop();
            goto start;
        }

        listener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
        listener.UnsafeConnectionNtlmAuthentication = true;
        listener.IgnoreWriteExceptions = true;

        string data = request.RawUrl;

        datastore2 = data.Replace("/handshake?user=", "");
        datastore2 = datastore2.Replace("&token=", " ");

        datastore = datastore2;

        string user = datastore.Split()[0];
        string token = datastore.Split(' ')[1];

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "login",
            ["username"] = user,
            ["token"] = token,
            ["hwid"] = hwid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        bool success = true;
        if (json.ownerid == ownerid)
        {
            GlobalAddAtom(seed);
            GlobalAddAtom(ownerid);

            load_response_struct(json);

            if (json.success)
            {
                load_user_data(json.info);

                responsepp.StatusCode = 420;
                responsepp.StatusDescription = "SHEESH";
            }
            else
            {
                Console.WriteLine(json.message);
                responsepp.StatusCode = (int)HttpStatusCode.OK;
                responsepp.StatusDescription = json.message;
                success = false;
            }
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }

        byte[] buffer = Encoding.UTF8.GetBytes("Complete");

        responsepp.ContentLength64 = buffer.Length;
        Stream output = responsepp.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        Thread.Sleep(1);
        listener.Stop();

        if (!success)
            Process.GetCurrentProcess().Kill();
        BepInPatcher.CallThrowException(OnGameInit.anti2);
    }

    /// <summary>
    /// Use Buttons from KeyAuth Customer Panel
    /// </summary>
    /// <param name="button">Button Name</param>
    public void button(string button)
    {
        CheckInit();

        HttpListener listener = new HttpListener();

        string output;

        output = button;
        output = "http://localhost:1337/" + output + "/";

        listener.Prefixes.Add(output);

        listener.Start();

        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;
        HttpListenerResponse responsepp = context.Response;

        responsepp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
        responsepp.AddHeader("Access-Control-Allow-Origin", "*");
        responsepp.AddHeader("Via", "hugzho's big brain");
        responsepp.AddHeader("Location", "your kernel ;)");
        responsepp.AddHeader("Retry-After", "never lmao");
        responsepp.Headers.Add("Server", "\r\n\r\n");

        responsepp.StatusCode = 420;
        responsepp.StatusDescription = "SHEESH";

        listener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
        listener.UnsafeConnectionNtlmAuthentication = true;
        listener.IgnoreWriteExceptions = true;

        listener.Stop();
    }

    /// <summary>
    /// Gives the user a subscription that has the same level as the key
    /// </summary>
    /// <param name="username">Username of the user thats going to get upgraded</param>
    /// <param name="key">License with the same level as the subscription you want to give the user</param>
    public void upgrade(string username, string key)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "upgrade",
            ["username"] = username,
            ["key"] = key,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            json.success = false;
            load_response_struct(json);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }

    /// <summary>
    /// Authenticate without using usernames and passwords
    /// </summary>
    /// <param name="key">Licence used to login with</param>
    public void license(string key, string code = null)
    {
        CheckInit();

        string hwid = OnGameInit.hwid;

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "license",
            ["key"] = key,
            ["hwid"] = hwid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid,
            ["code"] = code ?? string.Empty
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);

        if (json.ownerid == ownerid)
        {
            GlobalAddAtom(seed);
            GlobalAddAtom(ownerid);

            load_response_struct(json);
            if (json.success)
                load_user_data(json.info);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }
    /// <summary>
    /// Checks if the current session is validated or not
    /// </summary>
    public void check()
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "check",
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }
    /// <summary>
    /// Disable two factor authentication (2fa)
    /// </summary>
    public void disable2fa(string code)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "2fadisable",
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid,
            ["code"] = code
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);

        Console.WriteLine(json.message);
    }
    /// <summary>
    /// Enable two factor authentication (2fa)
    /// </summary>
    public void enable2fa(string code = null)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "2faenable",
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid,
            ["code"] = code
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);

        if (json.success)
        {
            if (code == null)
            {
                Console.WriteLine($"Your 2FA Secret is: {json.twoFactor.SecretCode}");
                Console.Write("Enter the 6 digit authentication code from your authentication app: ");
                string code6Digit = Console.ReadLine();
                this.enable2fa(code6Digit);
            }
            else
            {
                Console.WriteLine("2FA has been successfully enabled!");
                Thread.Sleep(3000);
            }
        }
        else
        {
            Console.WriteLine($"Error: {json.message}");
            Thread.Sleep(3000);
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }
    /// <summary>
    /// Change the data of an existing user variable, *User must be logged in*
    /// </summary>
    /// <param name="var">User variable name</param>
    /// <param name="data">The content of the variable</param>
    public void setvar(string var, string data)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "setvar",
            ["var"] = var,
            ["data"] = data,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }
    /// <summary>
    /// Gets the an existing user variable
    /// </summary>
    /// <param name="var">User Variable Name</param>
    /// <returns>The content of the user variable</returns>
    public string getvar(string var)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "getvar",
            ["var"] = var,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
            if (json.success)
                return json.response;
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
        return null;
    }
    /// <summary>
    /// Bans the current logged in user
    /// </summary>
    public void ban(string reason = null)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "ban",
            ["reason"] = reason,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }
    /// <summary>
    /// Gets an existing global variable
    /// </summary>
    /// <param name="varid">Variable ID</param>
    /// <returns>The content of the variable</returns>
    public string var(string varid)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "var",
            ["varid"] = varid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
            if (json.success)
                return json.message;
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
        return null;
    }
    /// <summary>
    /// Fetch usernames of online users
    /// </summary>
    /// <returns>ArrayList of usernames</returns>
    public List<users> fetchOnline()
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "fetchOnline",
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);

        if (json.success)
            return json.users;
        return null;
    }
    /// <summary>
    /// Fetch app statistic counts
    /// </summary>
    public void fetchStats()
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "fetchStats",
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);

        if (json.success)
            load_app_data(json.appinfo);
    }
    /// <summary>
    /// Gets the last 50 sent messages of that channel
    /// </summary>
    /// <param name="channelname">The channel name</param>
    /// <returns>the last 50 sent messages of that channel</returns>
    public List<msg> chatget(string channelname)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "chatget",
            ["channel"] = channelname,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);
        if (json.success)
        {
            return json.messages;
        }
        return null;
    }
    /// <summary>
    /// Sends a message to the given channel name
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="channelname">Channel Name</param>
    /// <returns>If the message was sent successfully, it returns true if not false</returns>
    public bool chatsend(string msg, string channelname)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "chatsend",
            ["message"] = msg,
            ["channel"] = channelname,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);
        if (json.success)
            return true;
        return false;
    }
    /// <summary>
    /// Checks if the current ip address/hwid is blacklisted
    /// </summary>
    /// <returns>If found blacklisted returns true if not false</returns>
    public bool checkblack()
    {
        CheckInit();
        string hwid = OnGameInit.hwid;

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "checkblacklist",
            ["hwid"] = hwid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
            if (json.success)
                return true;
            else
                return false;
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
        return true;
    }
    /// <summary>
    /// Sends a request to a webhook that you've added in the dashboard in a safe way without it being showed for example a http debugger
    /// </summary>
    /// <param name="webid">Webhook ID</param>
    /// <param name="param">Parameters</param>
    /// <param name="body">Body of the request, empty by default</param>
    /// <param name="conttype">Content type, empty by default</param>
    /// <returns>the webhook's response</returns>
    public string webhook(string webid, string param, string body = "", string conttype = "")
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "webhook",
            ["webid"] = webid,
            ["params"] = param,
            ["body"] = body,
            ["conttype"] = conttype,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        if (json.ownerid == ownerid)
        {
            load_response_struct(json);
            if (json.success)
                return json.response;
        }
        else
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
        return null;
    }
    /// <summary>
    /// KeyAuth acts as proxy and downlods the file in a secure way
    /// </summary>
    /// <param name="fileid">File ID</param>
    /// <returns>The bytes of the download file</returns>
    public byte[] download(string fileid)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "file",
            ["fileid"] = fileid,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        var response = MEOW(values_to_upload);


        var json = response_decoder.string_to_generic<response_structure>(response);
        load_response_struct(json);
        if (json.success)
            return encryption.str_to_byte_arr(json.contents);
        return null;
    }
    /// <summary>
    /// Logs the IP address,PC Name with a message, if a discord webhook is set up in the app settings, the log will get sent there and the dashboard if not set up it will only be in the dashboard
    /// </summary>
    /// <param name="message">Message</param>
    public void log(string message)
    {
        CheckInit();

        var values_to_upload = new NameValueCollection
        {
            ["type"] = "log",
            ["pcuser"] = Environment.UserName,
            ["message"] = message,
            ["sessionid"] = sessionid,
            ["name"] = name,
            ["ownerid"] = ownerid
        };

        MEOW(values_to_upload);

    }

    public static string fuckmegood(string filename)
    {
        string real = "Your a good boy arent you";

        string result;
        using (MD5 md = MD5.Create())
        {
            using (FileStream fileStream = File.OpenRead(filename))
            {
                byte[] value = md.ComputeHash(fileStream);
                result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
            }
        }
        return result;
    }

    private static string MEOW(NameValueCollection post_data)
    {
        // Rate limiting: Ensure at least 1 second between requests
        DateTime now = DateTime.UtcNow;
        TimeSpan timeSinceLastRequest = now - lastRequestTime;
        if (timeSinceLastRequest < minRequestInterval)
        {
            int sleepMs = (int)(minRequestInterval - timeSinceLastRequest).TotalMilliseconds;
            Thread.Sleep(sleepMs);
        }

        string responseText = "";
        UnityWebRequest request = new UnityWebRequest("https://keyauth.com/api/1.3/", "POST");

        WWWForm form = new WWWForm();
        foreach (string key in post_data.AllKeys)
        {
            form.AddField(key, post_data[key]);
        }
        request.uploadHandler = new UploadHandlerRaw(form.data);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        request.SendWebRequest();

        while (!request.isDone)
        {
            System.Threading.Thread.Sleep(30);
        }

        if (request.isNetworkError || request.isHttpError)
        {
            UnityEngine.Debug.LogError($"Request error: {request.error}");
            return "";
        }

        string raw_response = request.downloadHandler.text;

        WebHeaderCollection headers = new WebHeaderCollection();
        foreach (KeyValuePair<string, string> header in request.GetResponseHeaders())
        {
            headers.Add(header.Key, header.Value);
        }

        sigCheck(raw_response, headers, post_data.Get(0));

        // Update the last request time after the request completes
        lastRequestTime = DateTime.UtcNow;

        return raw_response;
    }

    private static bool assertSSL(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if ((!certificate.Issuer.Contains("Google Trust Services") && !certificate.Issuer.Contains("Let's Encrypt")) || sslPolicyErrors != SslPolicyErrors.None)
        {
            return false;
        }
        return true;
    }

    private static void sigCheck(string resp, WebHeaderCollection headers, string type)
    {
        if (type == "log" || type == "file" || type == "2faenable" || type == "2fadisable")
        {
            return;
        }

        try
        {
            string signature = headers["x-signature-ed25519"];
            string timestamp = headers["x-signature-timestamp"];

            if (!long.TryParse(timestamp, out long unixTimestamp))
            {
                UnityEngine.Debug.Log("error parsing");
            }

            DateTime timestampTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan timeDifference = currentTime - timestampTime;

            var byteSig = encryption.str_to_byte_arr(signature);
            var byteKey = encryption.str_to_byte_arr("5586b4bc69c7a4b487e4563a4cd96afd39140f919bd31cea7d1c6a1e8439422b");
            string body = timestamp + resp;
            var byteBody = Encoding.Default.GetBytes(body);

            bool signatureValid = Ed25519.CheckValid(byteSig, byteBody, byteKey);
            if (!signatureValid)
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
        }
        catch
        {
            Process.GetCurrentProcess().Kill();
            BepInPatcher.CallThrowException(OnGameInit.anti2);
        }
    }

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool LookupAccountName(string lpSystemName, string lpAccountName, byte[] Sid, ref uint cbSid, StringBuilder ReferencedDomainName, ref uint cchReferencedDomainName, out int peUse);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool ConvertSidToStringSid(byte[] Sid, out IntPtr StringSid);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LocalFree(IntPtr hMem);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetUserName(StringBuilder lpBuffer, ref uint pcbBuffer);
    public static string SENPAII()
    {
        try
        {
            StringBuilder username = new StringBuilder(256);
            uint usernameSize = (uint)username.Capacity;
            if (!GetUserName(username, ref usernameSize))
            {
                CustomConsole.Error("Failed to get username.");
            }

            byte[] sid = new byte[256];
            uint sidSize = (uint)sid.Length;
            StringBuilder domainName = new StringBuilder(256);
            uint domainSize = (uint)domainName.Capacity;
            int sidType;

            if (!LookupAccountName(null, username.ToString(), sid, ref sidSize, domainName, ref domainSize, out sidType))
            {
                CustomConsole.Error($"Failed to lookup SID. Error: {Marshal.GetLastWin32Error()}");
            }

            StringBuilder sidString = new StringBuilder();
            if (!ConvertSidToStringSid(sid, out IntPtr sidPtr))
            {
                CustomConsole.Error($"Failed to convert SID to string. Error: {Marshal.GetLastWin32Error()}");
            }

            sidString.Append(Marshal.PtrToStringAuto(sidPtr));
            LocalFree(sidPtr);

            return sidString.ToString();
        }
        catch (Exception ex)
        {
            CustomConsole.Error(ex.Message);
            return null;
        }
    }

    #region app_data
    public app_data_class app_data = new app_data_class();

    public class app_data_class
    {
        public string numUsers { get; set; }
        public string numOnlineUsers { get; set; }
        public string numKeys { get; set; }
        public string version { get; set; }
        public string customerPanelLink { get; set; }
        public string downloadLink { get; set; }
    }

    private void load_app_data(app_data_structure data)
    {
        app_data.numUsers = data.numUsers;
        app_data.numOnlineUsers = data.numOnlineUsers;
        app_data.numKeys = data.numKeys;
        app_data.version = data.version;
        app_data.customerPanelLink = data.customerPanelLink;
    }
    #endregion

    #region user_data
    public user_data_class user_data = new user_data_class();

    public class user_data_class
    {
        public string username { get; set; }
        public string ip { get; set; }
        public string hwid { get; set; }
        public string createdate { get; set; }
        public string lastlogin { get; set; }
        public List<Data> subscriptions { get; set; }
    }
    public class Data
    {
        public string subscription { get; set; }
        public string expiry { get; set; }
        public string timeleft { get; set; }
        public string key { get; set; }
    }

    private void load_user_data(user_data_structure data)
    {
        user_data.username = data.username;
        user_data.ip = data.ip;
        user_data.hwid = data.hwid;
        user_data.createdate = data.createdate;
        user_data.lastlogin = data.lastlogin;
        user_data.subscriptions = data.subscriptions;
    }
    #endregion

    [DataContract]
    private class TwoFactorData
    {
        [DataMember(Name = "secret_code")]
        public string SecretCode { get; set; }

        [DataMember(Name = "QRCode")]
        public string QRCode { get; set; }
    }

    #region response_struct
    public response_class response = new response_class();

    public class response_class
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    private void load_response_struct(response_structure data)
    {
        response.success = data.success;
        response.message = data.message;
    }
    #endregion

    private json_wrapper response_decoder = new json_wrapper(new response_structure());
}

public static class encryption
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetCurrentProcess();

    public static string HashHMAC(string enckey, string resp)
    {
        byte[] key = Encoding.UTF8.GetBytes(enckey);
        byte[] message = Encoding.UTF8.GetBytes(resp);
        var hash = new HMACSHA256(key);
        return byte_arr_to_str(hash.ComputeHash(message));
    }

    public static string byte_arr_to_str(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    public static byte[] str_to_byte_arr(string hex)
    {
        try
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        catch
        {
            UnityEngine.Debug.Log("str_to_byte_arr");
            return null;
        }
    }

    public static string iv_key() =>
        Guid.NewGuid().ToString().Substring(0, 16);
}

public class json_wrapper
{
    public static bool is_serializable(Type to_check) =>
        to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);

    public json_wrapper(object obj_to_work_with)
    {
        current_object = obj_to_work_with;

        var object_type = current_object.GetType();

        serializer = new DataContractJsonSerializer(object_type);

        if (!is_serializable(object_type))
            throw new Exception($"the object {current_object} isn't a serializable");
    }

    public object string_to_object(string json)
    {
        var buffer = Encoding.Default.GetBytes(json);

        using (var mem_stream = new MemoryStream(buffer))
            return serializer.ReadObject(mem_stream);
    }

    public T string_to_generic<T>(string json) =>
        (T)string_to_object(json);

    private DataContractJsonSerializer serializer;

    private object current_object;
}