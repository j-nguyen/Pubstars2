using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubstarsDtos
{
    public class UserData
    {
        public string Name;
        public string Password;
        public float Rating;

        public UserData(string name, string pw, float r)
        {
            Name = name;
            Password = pw;
            Rating = r;
        }
    }
}
