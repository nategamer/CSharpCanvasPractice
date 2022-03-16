﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestArrows.ViewModels.Sequences
{
    public class Sequence
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _caste;
        public string Caste
        {
            get { return _caste; }
            set { _caste = value; }
        }
    }
}
