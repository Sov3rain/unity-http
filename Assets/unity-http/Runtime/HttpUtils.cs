using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityHttp
{
    /// <summary>
    /// A utility class for http.
    /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status"/>
    /// </summary>
    public static class HttpUtils
    {
        /// <summary>
        /// Format and append parameters to an uri
        /// </summary>
        /// <param name="uri">The uri to append the properties to.</param>
        /// <param name="parameters">A dictionary of parameters to append to the uri.</param>
        /// <returns>The uri with the appended parameters.</returns>
        public static string ConstructUriWithParameters(string uri, Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return uri;
            }

            var stringBuilder = new StringBuilder(uri);

            for (var i = 0; i < parameters.Count; i++)
            {
                var element = parameters.ElementAt(i);
                stringBuilder.Append(i == 0 ? "?" : "&");
                stringBuilder.Append(element.Key);
                stringBuilder.Append("=");
                stringBuilder.Append(element.Value);
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Writes the response data to a file, using either synchronous or asynchronous writing based on data size.
        /// </summary>
        /// <param name="data">The data to write</param>
        /// <param name="filePath">The full path where the file should be written</param>
        /// <remarks>
        /// For files smaller than 150KB, synchronous writing is used.
        /// For larger files, asynchronous writing is performed on a background thread to avoid blocking.
        /// </remarks>
        /// <exception cref="Exception">Exceptions during file writing are caught and logged</exception>
        public static void WriteFile(byte[] data, string filePath)
        {
            const int threshold = 150 * 1024; // 150KB

            if (data.Length < threshold)
            {
                try
                {
                    File.WriteAllBytes(filePath, data);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            else
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await File.WriteAllBytesAsync(filePath, data);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                });
            }
        }
    }
}