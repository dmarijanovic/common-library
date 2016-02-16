using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DamirM.CommonLibrary
{
    public class Escape
    {

        public static bool IsEscaped(string text, int index, char escapeChar)
        {
            int excapeCount = -1;
            for (int i = index - 1; i >= 0; i--)
            {
                if (text[i] == escapeChar)
                {
                    excapeCount++;
                }
                else
                {
                    break;
                }
            }

            if (excapeCount % 2 == 0)
            {
                return true;
            }
            else
            {
                // dodatna projera ako treba napraviti escape samoga escape znaka
                if (text[index] == escapeChar)
                {
                    if (text.Length > index + 1)
                    {
                        if (text[index + 1] == escapeChar)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Excape all character in text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="list"></param>
        /// <param name="escapeChar"></param>
        /// <returns></returns>
        public static string EscapeAll(string text, char[] list, char escapeChar)
        {
            string result = text;
            int indexOf;
            int startSearch;
            bool indikator;
            // for each all char that need to be escaped
            foreach (char charToEscape in list)
            {
                indexOf = 0;
                startSearch = 0;
                // ako se znak ponavlja escape-aj i njega
                do
                {
                    indexOf = result.IndexOf(charToEscape,startSearch);
                    // ako je naden znak za excape
                    if (indexOf != -1)
                    {
                        // ako vec nije escape-an, escape-aj ga
                        if (IsEscaped(result, indexOf, escapeChar) == false)
                        {
                            indikator = true;
                            // dodatna provjera ako je escape znak
                            if (charToEscape == escapeChar)
                            {
                                if (indexOf + 1 < result.Length)
                                {
                                    foreach (char charToEscape2 in list)
                                    {
                                        if (result[indexOf + 1] == charToEscape2)
                                        {
                                            indikator = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (indikator == true)
                            {
                                result = string.Concat(result.Substring(0, indexOf), escapeChar, charToEscape, indexOf + 1 < result.Length ? result.Substring(indexOf + 1) : "");
                            }
                        }
                        // uvecaj za 1 jer smo ubacili escape
                        startSearch = indexOf + 1;
                    }
                    else
                    {
                        break;
                    }

                } while (true);

            }

            return result;
        }


    }
}
