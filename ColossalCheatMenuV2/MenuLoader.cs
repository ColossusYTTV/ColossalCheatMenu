﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Colossal.Auth;
using Colossal.Menu;
using UnityEngine;

namespace Colossal
{
    public class MenuOption
    {
        public string DisplayName;
        public string _type;
        public bool AssociatedBool;
        public string AssociatedString;
        public float AssociatedFloat;
        public int AssociatedInt;
        public string[] StringArray;
        public int stringsliderind;
        public string AssociatedBind;
        public string extra;
    }

    public static class MenuLoader
    {
        private static string subMenuType = "9fi2GKmQYphDjFstEgHvs9fi2GKmQYphDjFstEgHvu9fi2GKmQYphDjFstEgHvb9fi2GKmQYphDjFstEgHvm9fi2GKmQYphDjFstEgHve9fi2GKmQYphDjFstEgHvn9fi2GKmQYphDjFstEgHvu9fi2GKmQYphDjFstEgHvi9fi2GKmQYphDjFstEgHvV9fi2GKmQYphDjFstEgHvn9fi2GKmQYphDjFstEgHvt9fi2GKmQYphDjFstEgHvP9fi2GKmQYphDjFstEgHvY9fi2GKmQYphDjFstEgHvm9fi2GKmQYphDjFstEgHv09fi2GKmQYphDjFstEgHv49fi2GKmQYphDjFstEgHvT9fi2GKmQYphDjFstEgHvq9fi2GKmQYphDjFstEgHvC9fi2GKmQYphDjFstEgHvc9fi2GKmQYphDjFstEgHv59fi2GKmQYphDjFstEgHv69fi2GKmQYphDjFstEgHvG9fi2GKmQYphDjFstEgHvF9fi2GKmQYphDjFstEgHv99fi2GKmQYphDjFstEgHv49fi2GKmQYphDjFstEgHvc9fi2GKmQYphDjFstEgHvz9fi2GKmQYphDjFstEgHv".Replace("9fi2GKmQYphDjFstEgHv", "");
        private static string sliderType = "bZUPMm9JgABFs8UxzpamSbZUPMm9JgABFs8UxzpamTbZUPMm9JgABFs8UxzpamRbZUPMm9JgABFs8UxzpamIbZUPMm9JgABFs8UxzpamNbZUPMm9JgABFs8UxzpamGbZUPMm9JgABFs8UxzpamsbZUPMm9JgABFs8UxzpamlbZUPMm9JgABFs8UxzpamibZUPMm9JgABFs8UxzpamdbZUPMm9JgABFs8UxzpamebZUPMm9JgABFs8UxzpamrbZUPMm9JgABFs8UxzpamfbZUPMm9JgABFs8UxzpamobZUPMm9JgABFs8UxzpamebZUPMm9JgABFs8UxzpamzbZUPMm9JgABFs8UxzpamGbZUPMm9JgABFs8UxzpamDbZUPMm9JgABFs8UxzpamAbZUPMm9JgABFs8Uxzpam5bZUPMm9JgABFs8Uxzpam9bZUPMm9JgABFs8UxzpamjbZUPMm9JgABFs8UxzpambbZUPMm9JgABFs8UxzpamCbZUPMm9JgABFs8UxzpammbZUPMm9JgABFs8UxzpamrbZUPMm9JgABFs8Uxzpam7bZUPMm9JgABFs8UxzpamZbZUPMm9JgABFs8Uxzpam6bZUPMm9JgABFs8Uxzpam6bZUPMm9JgABFs8Uxzpam2bZUPMm9JgABFs8UxzpamfbZUPMm9JgABFs8UxzpamQbZUPMm9JgABFs8Uxzpam".Replace("bZUPMm9JgABFs8Uxzpam", "");
        private static string toggleType = "Z7Y2gZejP0bY8xsuqRnytZ7Y2gZejP0bY8xsuqRnyoZ7Y2gZejP0bY8xsuqRnygZ7Y2gZejP0bY8xsuqRnygZ7Y2gZejP0bY8xsuqRnylZ7Y2gZejP0bY8xsuqRnyeZ7Y2gZejP0bY8xsuqRnyAZ7Y2gZejP0bY8xsuqRnyKZ7Y2gZejP0bY8xsuqRnybZ7Y2gZejP0bY8xsuqRnyYZ7Y2gZejP0bY8xsuqRnycZ7Y2gZejP0bY8xsuqRnyCZ7Y2gZejP0bY8xsuqRnypZ7Y2gZejP0bY8xsuqRnyhZ7Y2gZejP0bY8xsuqRnyaZ7Y2gZejP0bY8xsuqRnyYZ7Y2gZejP0bY8xsuqRnyZZ7Y2gZejP0bY8xsuqRnyCZ7Y2gZejP0bY8xsuqRnyiZ7Y2gZejP0bY8xsuqRnyiZ7Y2gZejP0bY8xsuqRny9Z7Y2gZejP0bY8xsuqRny2Z7Y2gZejP0bY8xsuqRnyEZ7Y2gZejP0bY8xsuqRnynZ7Y2gZejP0bY8xsuqRnyFZ7Y2gZejP0bY8xsuqRnyJZ7Y2gZejP0bY8xsuqRnyHZ7Y2gZejP0bY8xsuqRny".Replace("Z7Y2gZejP0bY8xsuqRny", "");
        private static string buttonType = "9ZFYQUgxH4PmTcRiPnrNb9ZFYQUgxH4PmTcRiPnrNu9ZFYQUgxH4PmTcRiPnrNt9ZFYQUgxH4PmTcRiPnrNt9ZFYQUgxH4PmTcRiPnrNo9ZFYQUgxH4PmTcRiPnrNn9ZFYQUgxH4PmTcRiPnrNf9ZFYQUgxH4PmTcRiPnrNk9ZFYQUgxH4PmTcRiPnrNP9ZFYQUgxH4PmTcRiPnrNA9ZFYQUgxH4PmTcRiPnrNi9ZFYQUgxH4PmTcRiPnrNu9ZFYQUgxH4PmTcRiPnrNM9ZFYQUgxH4PmTcRiPnrNn9ZFYQUgxH4PmTcRiPnrN19ZFYQUgxH4PmTcRiPnrNn9ZFYQUgxH4PmTcRiPnrND9ZFYQUgxH4PmTcRiPnrNT9ZFYQUgxH4PmTcRiPnrNx9ZFYQUgxH4PmTcRiPnrN19ZFYQUgxH4PmTcRiPnrNX9ZFYQUgxH4PmTcRiPnrNV9ZFYQUgxH4PmTcRiPnrNK9ZFYQUgxH4PmTcRiPnrNL9ZFYQUgxH4PmTcRiPnrNk9ZFYQUgxH4PmTcRiPnrNg9ZFYQUgxH4PmTcRiPnrND9ZFYQUgxH4PmTcRiPnrN".Replace("9ZFYQUgxH4PmTcRiPnrN", "");

        public static MenuOption[] LoadMenu(params string[] keyAuthVarNames)
        {
            if (keyAuthVarNames == null || keyAuthVarNames.Length == 0)
            {
                return new MenuOption[0];
            }

            List<string> menuDataParts = new List<string>();
            foreach (string varName in keyAuthVarNames)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    continue;
                }

                string data = Init.KeyAuthApp.var(varName)?.Trim();
                if (string.IsNullOrEmpty(data))
                {
                    continue;
                }

                menuDataParts.Add(data);
            }

            if (menuDataParts.Count == 0)
            {
                return new MenuOption[0];
            }

            string menuData = string.Join(";", menuDataParts);
            List<MenuOption> options = new List<MenuOption>();
            string[] entries = menuData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string entry in entries)
            {
                string trimmedEntry = entry.Trim();
                if (string.IsNullOrEmpty(trimmedEntry))
                {
                    continue;
                }

                string[] fields = trimmedEntry.Split(new char[] { ',' }, StringSplitOptions.None);
                if (fields.Length < 2)
                {
                    continue;
                }

                for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                {
                    fields[fieldIndex] = fields[fieldIndex].Trim();
                }

                MenuOption option = new MenuOption();
                if (string.IsNullOrEmpty(fields[0]))
                {
                    continue;
                }
                option.DisplayName = fields[0];

                string typeStr = fields[1];
                if (string.IsNullOrEmpty(typeStr))
                {
                    continue;
                }

                if (typeStr == "submenuthingy")
                {
                    option._type = subMenuType;
                }
                else if (typeStr == "togglethingy")
                {
                    option._type = toggleType;
                }
                else if (typeStr == "sliderthingy")
                {
                    option._type = sliderType;
                }
                else if (typeStr == "buttonthingy")
                {
                    option._type = buttonType;
                }
                else
                {
                    continue;
                }

                if (option._type == subMenuType || option._type == buttonType)
                {
                    if (fields.Length > 2 && !string.IsNullOrEmpty(fields[2]))
                    {
                        option.AssociatedString = fields[2];
                    }
                    else
                    {
                        option.AssociatedString = "";
                    }
                    if (fields.Length > 3 && !string.IsNullOrEmpty(fields[3]))
                    {
                        option.extra = fields[3];
                    }
                }
                else if (option._type == toggleType)
                {
                    if (fields.Length > 2 && !string.IsNullOrEmpty(fields[2]))
                    {
                        string boolField = fields[2];
                        try
                        {
                            FieldInfo field = typeof(PluginConfig).GetField(boolField, BindingFlags.Public | BindingFlags.Static);
                            if (field != null && field.FieldType == typeof(bool))
                            {
                                object value = field.GetValue(null);
                                if (value != null)
                                {
                                    option.AssociatedBool = (bool)value;
                                }
                                else
                                {
                                    option.AssociatedBool = false;
                                }
                            }
                            else
                            {
                                option.AssociatedBool = false;
                            }
                        }
                        catch
                        {
                            option.AssociatedBool = false;
                        }
                    }
                    else
                    {
                        option.AssociatedBool = false;
                    }
                    if (fields.Length > 3 && !string.IsNullOrEmpty(fields[3]))
                    {
                        option.extra = fields[3];
                    }
                }
                else if (option._type == sliderType)
                {
                    int fieldPos = 2;
                    if (fieldPos < fields.Length && !string.IsNullOrEmpty(fields[fieldPos]) && !fields[fieldPos].StartsWith("["))
                    {
                        option.AssociatedString = fields[fieldPos];
                        fieldPos++;
                    }
                    if (fieldPos < fields.Length && !string.IsNullOrEmpty(fields[fieldPos]))
                    {
                        string arrayStr = fields[fieldPos].Trim();
                        if (arrayStr.StartsWith("[") && arrayStr.EndsWith("]"))
                        {
                            string innerStr = arrayStr.Substring(1, arrayStr.Length - 2);
                            option.StringArray = string.IsNullOrEmpty(innerStr) ? new string[0] : innerStr.Split(new char[] { '|' }, StringSplitOptions.None);
                            for (int arrayIndex = 0; arrayIndex < option.StringArray.Length; arrayIndex++)
                            {
                                option.StringArray[arrayIndex] = option.StringArray[arrayIndex].Trim();
                            }
                        }
                        else
                        {
                            option.StringArray = new string[0];
                        }
                    }
                    else
                    {
                        option.StringArray = new string[0];
                    }
                    fieldPos++;
                    if (fieldPos < fields.Length && float.TryParse(fields[fieldPos], out float assocFloat))
                    {
                        option.AssociatedFloat = assocFloat;
                    }
                    fieldPos++;
                    if (fieldPos < fields.Length && int.TryParse(fields[fieldPos], out int assocInt))
                    {
                        option.AssociatedInt = assocInt;
                    }
                    fieldPos++;
                    if (fieldPos < fields.Length && int.TryParse(fields[fieldPos], out int slider))
                    {
                        option.stringsliderind = slider;
                    }
                    fieldPos++;
                    if (fieldPos < fields.Length && !string.IsNullOrEmpty(fields[fieldPos]))
                    {
                        option.AssociatedBind = fields[fieldPos];
                    }
                    fieldPos++;
                    if (fieldPos < fields.Length && !string.IsNullOrEmpty(fields[fieldPos]))
                    {
                        option.extra = fields[fieldPos];
                    }
                }

                options.Add(option);
            }

            return options.ToArray();
        }
    }
}