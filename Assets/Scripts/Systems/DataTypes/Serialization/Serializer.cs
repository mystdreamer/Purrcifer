using UnityEngine;
using System.IO;
using System.Xml.Serialization;

namespace Purrcifer.Data.Xml
{
    /// <summary>
    /// XML serializer for generic types. 
    /// </summary>
    public static class XML_Serialization
    {
        /// <summary>
        /// The string specifying the folder in which to create alignment data. 
        /// </summary>
        public static string folderName = "/Data/";

        public const string gameSaveFileName = "GS.xml";
        public const string SavedEventXML = "E.xml";
        public const string DefaultEventXML = "D_E.xml";

        /// <summary>
        /// The folder path for serialized data.  
        /// </summary>
        public static string PersistPath => Application.persistentDataPath + folderName;

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

        public static T AttemptDeserialization<T>(string path, string fileName, out bool successful)
        {
            AssureDirectoryExists(path);

            if(DataExists(path + fileName))
            {
                successful = true;
                return Deserialize<T>(path + fileName);
            }
            successful = false;

            return default(T);
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
            Stream stream = new FileStream(path, FileMode.Open);
            var output = s.Deserialize(stream);
            stream.Close();
            return (T)output;
        }

        public static void CheckFileState(string dirPath, string fileName, out bool directoryExists, out bool fileExists)
        {
            //Check if the directory existed or had to be created. 
            AssureDirectoryExists(dirPath, out bool created);
            directoryExists = true;
            if (created) //The directory didn't exist and was made.
                fileExists = false;
            else //Check if the file exists in the directory. 
                fileExists = DataExists(dirPath);
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

        public static void AssureDirectoryExists(string path, out bool created)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                created = true;
            }
            created = false;
        }

        public static void AssureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}