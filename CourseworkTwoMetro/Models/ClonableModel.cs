using System;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    [ Serializable ]
    public class ClonableModel : ICloneable
    {
        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }
    }
}