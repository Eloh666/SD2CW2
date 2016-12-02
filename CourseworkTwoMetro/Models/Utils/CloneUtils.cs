using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CourseworkTwoMetro.Models.Utils
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Runs a deep serialization on an object.
    /// Used for the implementation of the IClone interface in the models
    /// Note: I take no credit for this function.
    /// </summary>
    class CloneUtils
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
