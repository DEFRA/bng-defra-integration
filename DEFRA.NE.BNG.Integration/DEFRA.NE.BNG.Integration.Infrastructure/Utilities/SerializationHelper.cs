using DEFRA.NE.BNG.Integration.Model.Request;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities
{
    public static class SerializationHelper
    {
        /// <summary>
        /// Function to deserialize JSON string using DataContractJsonSerializer
        /// </summary>
        /// <typeparam name="T">T Generic Type</typeparam>
        /// <param name="jsonString">string jsonString</param>
        /// <returns>Generic RemoteContextType object</returns>
        public static T DeserializeRemoteContextTypeString<T>(string jsonString)
        {
            T remoteContextObj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(remoteContextObj.GetType());
                remoteContextObj = (T)serializer.ReadObject(ms);
                ms.Close();
            }

            return remoteContextObj;
        }

        public static string GetExtensionFromFileName(string fileName)
        {
            var name = fileName.Split('.');
            return $".{name[name.Length - 1]}";
        }

        /// <summary>
        /// Returns File Type with appended count
        /// </summary>
        /// <param name="files"></param>
        public static void GroupbyAndIndexFileType(List<FileDetails> files)
        {
            var groupbyFileType = from file in files
                                  group file by file.FileType into fileTypeGroup
                                  select fileTypeGroup;


            foreach (var file in groupbyFileType)
            {
                int counter = file.Count();
                if (counter > 1)
                {
                    foreach (var groupFile in file)
                    {
                        groupFile.FileType = $"{groupFile.FileType}-{counter}";
                        counter--;
                    }
                }
            }
        }
    }
}
