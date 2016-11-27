﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CourseworkTwoMetro.Models.Utils
{
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
