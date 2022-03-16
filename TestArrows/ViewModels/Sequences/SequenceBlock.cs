using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestArrows.ViewModels.Sequences
{
    public class SequenceBlock
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


        private double _topOffset;
        public double TopOffset
        {
            get { return _topOffset; }
            set { _topOffset = value; }
        }

        private double _bottomOffset;
        public double BottomOffset
        {
            get { return _bottomOffset; }
            set { _bottomOffset = value; }
        }

        private double _leftOffset;
        public double LeftOffset
        {
            get { return _leftOffset; }
            set { _leftOffset = value; }
        }

        private double _rightOffset;
        public double RightOffset
        {
            get { return _rightOffset; }
            set { _rightOffset = value; }
        }

        private TextBlock _seqBlock;
        public TextBlock SeqBlock
        {
            get { return _seqBlock; }
            set { _seqBlock = value; }
        }


    }
}
