using UnityEngine;
using System.IO;
using System.Xml.Serialization;

namespace GameEditor.GameEvent
{
    /// <summary>
    /// XML serializer for generic types. 
    /// </summary>
    public static class GameEventSerialization
    {
        /// <summary>
        /// The string specifying the folder in which to create alignment data. 
        /// </summary>
        public static string folderName = "/Data/";
        public static string fileName = "D_E.xml";

        /// <summary>
        /// The folder path for serialized data.  
        /// </summary>
        public static string DataPath => Application.persistentDataPath + folderName;
        public static string FullPath => DataPath + fileName;

        /// <summary>
        /// Generic XML Serializer.  
        /// </summary>
        /// <typeparam name="T"> The type of file to be serialized. </typeparam>
        /// <param name="type"> The file reference. </param>
        /// <param name="fileName"> The file name to serialize the data to. </param>
        public static void Serialize<T>(T type, string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.Create);
            s.Serialize(stream, type);
            stream.Close();

#if DEBUG
            Debug.Log("File Path: " + path);
#endif
        }

        /// <summary>
        /// Generic XML deserializer. 
        /// </summary>
        /// <typeparam name="T"> The file type to deserialize. </typeparam>
        /// <param name="fileName"> The name of the file to deserialize. </param>
        /// <returns> A deserialized instance of type T. </returns>
        public static T Deserialize<T>(string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.OpenOrCreate);
            var output = s.Deserialize(stream);
            stream.Close();
            return (T)output;
        }

        /// <summary>
        /// Check if the data file exists.
        /// </summary>
        /// <param name="path"> The path to serialize to. </param>
        /// <returns> True if the file exist. </returns>
        public static bool DataExists(string path)
        {
            return File.Exists(path);
        }

        public static void CheckPathExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static bool CheckFileExists(string path, string filename)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(path + filename))
            {
                return true;
            }

            return false;
        }
    }
}

