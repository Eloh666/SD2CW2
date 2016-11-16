﻿using CourseworkOneMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    public abstract class Person
    {
        public string Name { get; set; }
        protected Person(){}
        protected Person(string name)
        {
            Name = name;
        }

        public string ValidateName()
        {
            return ValidationUtilities.ValidateNonEmpty("Name", this.Name);
        }
    }
}