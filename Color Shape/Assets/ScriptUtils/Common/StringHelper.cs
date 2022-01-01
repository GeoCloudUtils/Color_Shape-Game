using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.Common
{
    public class StringHelper : MonoBehaviour
    {
        /// <summary>
        /// Returns a list of lines, containing the given text wraped in to lines of given length.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="nrChar"></param>
        /// <returns></returns>
        public static List<string> WordWrap(string input, int nrChar, string lineFeed = "\n")
        {
            //bool lastLineAdded = false;
            int cLines = 0;
            List<string> lines = new List<string>();
            input = input.Replace(lineFeed, " " + lineFeed + " ");
            string[] words = input.Split(' ');
            string line = "";


            for (int i = 0; i < words.Length; i++)
            {
                if (line.Length + words[i].Length + 1 <= nrChar && words[i] != lineFeed)
                {
                    if (i == 0)
                        line = words[i];
                    else
                        line += (!string.IsNullOrEmpty(line) ? " " : "") + words[i].Trim();

                }
                else
                {
                    lines.Add(line);
                    line = words[i].Trim();
                    cLines++;
                }
            }

            lines.Add(line);

            return lines;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Gets the stirng represenation of a screen resolution. For example: 1920x1080
        /// </summary>
        /// <param name="rez"></param>
        /// <returns></returns>
        public static string resolutionToString(Resolution rez)
        {
            return rez.width + "x" + rez.height;
        }
        /// <summary>
        /// Checks if given char is an int.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isInt(char str)
        {
            return isInt(str.ToString());
        }
        /// <summary>
        /// Checks if given string is an int.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isInt(string str)
        {
            int n;
            bool _isNumeric = int.TryParse(str, out n);
            return _isNumeric;
        }
        /// <summary>
        /// Converts a string to a screen resolution.
        /// </summary>
        /// <param name="str">Format should be ZxY. For example 1027x768</param>
        /// <returns></returns>
        public static Resolution stringToResolution(string rez)
        {
            bool convertable = true;
            int width = -1;
            int heigth = -1;
            if (!rez.Contains("x") || rez.IndexOf("x") != rez.LastIndexOf("x"))
            {
                convertable = false;
            }
            else
            {
                string[] rezSplit = rez.Split('x');
                if (!int.TryParse(rezSplit[0], out width))
                    convertable = false;
                if (!int.TryParse(rezSplit[1], out heigth))
                    convertable = false;
            }
            if (width == -1 || heigth == -1 || !convertable)
                throw (new System.Exception("Input string was in wrong format, accaptable format is for ex. 1024x768"));
            Resolution result = new Resolution();
            result.width = width;
            result.height = heigth;
            return result;
        }
    }
}