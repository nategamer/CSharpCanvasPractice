using Petzold.Media2D;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TestArrows.ViewModels.Sequences;
using Visualization.ViewModel;

namespace TestArrows.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands
        private ICommand arrowCommand;
        public ICommand ArrowCommand
        {
            get
            {
                if (arrowCommand == null)
                {
                    arrowCommand = new RelayCommand(x => CreateLines());
                }
                return arrowCommand;
            }
            set { arrowCommand = value; }
        }

        private ICommand getCanvasCommand;
        public ICommand GetCanvasCommand
        {
            get
            {
                if (getCanvasCommand == null)
                {
                    getCanvasCommand = new RelayCommand(GetCanvas);
                }
                return getCanvasCommand;
            }
            set { getCanvasCommand = value; }
        }

        private ICommand _generateSeqs;
        public ICommand GenerateSeqs
        {
            get 
            {
                if(_generateSeqs == null)
                {
                    _generateSeqs = new RelayCommand(x => CreateInitialSequences());
                }
                return _generateSeqs; 
            }
            set { _generateSeqs = value; }
        }
        #endregion // Commands

        #region Properties

        private Canvas _arrowCanvas;
        public Canvas ArrowCanvas
        {
            get { return _arrowCanvas; }
            set 
            { 
                _arrowCanvas = value;
            }
        }

        private ObservableCollection<ArrowLine> _lineCollection;
        public ObservableCollection<ArrowLine> LineCollection
        {
            get { return _lineCollection; }
            set 
            { 
                _lineCollection = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<SequenceBlock> _sequenceBlocks;
        public ObservableCollection<SequenceBlock> SequenceBlocks
        {
            get { return _sequenceBlocks; }
            set { _sequenceBlocks = value; }
        }

        private ObservableCollection<TextBlock> _visualBlocks;
        public ObservableCollection<TextBlock> VisualBlocks
        {
            get { return _visualBlocks; }
            set { _visualBlocks = value; }
        }

        private ObservableCollection<ObservableCollection<Sequence>> _seqs;
        public ObservableCollection<ObservableCollection<Sequence>> Seqs
        {
            get { return _seqs; }
            set { _seqs = value; }
        }

        private List<double> _leftOffsets;
        public List<double> LeftOffsets
        {
            get { return _leftOffsets; }
            set { _leftOffsets = value; }
        }


        #endregion // Properties

        #region Functions
        public void CreateInitialSequences()
        {
            ObservableCollection<ObservableCollection<Sequence>> tempCollectionMain = new ObservableCollection<ObservableCollection<Sequence>>();
            ObservableCollection<Sequence> tempCollection = new ObservableCollection<Sequence>();
            // Make 20 Sequences 
            // Two Caste 5, Four Caste 5, Six Caste 3, 16 Caste 2.
            int iterator = 1;
            for (int i = 0; i < 2; i++)
            {
                tempCollection.Add(new Sequence
                {
                    Name = "Seq " + iterator.ToString(),
                    Id = (i + 50).ToString(),
                    Caste = "5"
                });
                iterator++;
            }
            tempCollectionMain.Add(tempCollection);
            tempCollection = new ObservableCollection<Sequence>();
            for (int i = 0; i < 5; i++)
            {
                tempCollection.Add(new Sequence
                {
                    Name = "Seq " + iterator.ToString(),
                    Id = (i + 40).ToString(),
                    Caste = "4"
                });
                iterator++;
            }
            tempCollectionMain.Add(tempCollection);
            tempCollection = new ObservableCollection<Sequence>();
            for (int i = 0; i < 6; i++)
            {
                tempCollection.Add(new Sequence
                {
                    Name = "Seq " + iterator.ToString(),
                    Id = (i + 30).ToString(),
                    Caste = "3"
                });
                iterator++;
            }
            tempCollectionMain.Add(tempCollection);
            tempCollection = new ObservableCollection<Sequence>();
            for (int i = 0; i < 16; i++)
            {
                tempCollection.Add(new Sequence
                {
                    Name = "Seq " + iterator.ToString(),
                    Id = (i + 10).ToString(),
                    Caste = "2"
                });
                iterator++;
            }
            tempCollectionMain.Add(tempCollection);
            Seqs = tempCollectionMain;
            CalculateOffsets();
        }

        public void CreateSequenceBlock(double topoffset, double leftoffset, Sequence seq)
        {
            ObservableCollection<SequenceBlock> tempCollect = new ObservableCollection<SequenceBlock>();
            if (SequenceBlocks == null)
            {
                tempCollect.Add(new SequenceBlock
                {
                    Name = seq.Name,
                    Caste = seq.Caste,
                    TopOffset = topoffset,
                    LeftOffset = leftoffset,
                });
                SequenceBlocks = tempCollect;
                CreateVisualBlocks(seq);
            }
            else
            {
                SequenceBlocks.Add(new SequenceBlock
                {
                    Name = seq.Name,
                    Caste = seq.Caste,
                    TopOffset = topoffset,
                    LeftOffset = leftoffset,
                });
                CreateVisualBlocks(seq);
            }
        }

        public void CalculateOffsets()
        {
            double leftOffset;
            double topoffset;
            int seqWidth = 110;
            int seqHeight = 85;
            int iterator = 0;
            int maxSeqsPerRow = 10;
            foreach (ObservableCollection<Sequence> obs in Seqs)
            {
                int numberOfSeqs = obs.Count;
                double availableSpace = ArrowCanvas.ActualWidth / numberOfSeqs;
                if(numberOfSeqs > 10)
                {
                    availableSpace = ArrowCanvas.ActualWidth / maxSeqsPerRow;
                    for (int i = 0; i < maxSeqsPerRow; i++)
                    {
                        leftOffset = (i * availableSpace) + ((availableSpace - seqWidth) / 2);
                        topoffset = iterator * seqHeight;
                        CreateSequenceBlock(topoffset, leftOffset, obs[i]);
                    }
                    iterator++;
                    availableSpace = ArrowCanvas.ActualWidth / (numberOfSeqs - maxSeqsPerRow);
                    for(int i = 0; i < (numberOfSeqs - maxSeqsPerRow); i++)
                    {
                        leftOffset = (i * availableSpace) + ((availableSpace - seqWidth) / 2);
                        topoffset = iterator * seqHeight;
                        CreateSequenceBlock(topoffset, leftOffset, obs[i+10]);
                    }
                }
                else
                {
                    for (int i = 0; i < numberOfSeqs; i++)
                    {
                        leftOffset = (i * availableSpace) + ((availableSpace - seqWidth) / 2);
                        topoffset = iterator * seqHeight;
                        CreateSequenceBlock(topoffset, leftOffset, obs[i]);
                    }
                    iterator++;
                }
            }
            
        }

        public void CreateVisualBlocks(Sequence seq)
        {
            var bc = new BrushConverter();
            ObservableCollection<TextBlock> tempCollection = new ObservableCollection<TextBlock>();
            if(VisualBlocks == null)
            {
                VisualBlocks = tempCollection;
            }
            TextBlock txb = new TextBlock();
            txb.Height = 60;
            txb.Width = 100;
            txb.Text = seq.Name  + "\n" + seq.Id;
            txb.Background = (Brush)bc.ConvertFrom("#E3FDFF");
            double left = 5, top = 0, right = 5, bottom = 0;
            txb.Margin = new Thickness(left, top, right, bottom);
            txb.TextAlignment = TextAlignment.Center;
            VisualBlocks.Add(txb);
        }

        public void CreateLines()
        {
            ObservableCollection<ArrowLine> tempCollection = new ObservableCollection<ArrowLine>();
            if(LineCollection == null)
            {
                LineCollection = tempCollection;
            }
            else
            {
                tempCollection = LineCollection;
            }
            ArrowLine arrow = new ArrowLine()
            {
                X1 = 30,
                Y1 = 4,
                X2 = 20,
                Y2 = 80,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Black),
                ArrowEnds = ArrowEnds.End,
                IsArrowClosed = true,
                ArrowAngle = 45,
                ArrowLength = 10,
                Fill = new SolidColorBrush(Colors.Black)
            };
                tempCollection.Add(arrow);
            LineCollection = tempCollection;
        }

        private void GetCanvas(object obj)
        {

            var canvas = obj as Canvas;
            ArrowCanvas = canvas;
            //foreach (ArrowLine line in LineCollection)
            //{
            //    if (ArrowCanvas.Children.Contains(line))
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        ArrowCanvas.Children.Add(line);
            //    }
            //}
            if (VisualBlocks != null && SequenceBlocks != null)
            {
                for (int i = 0; i < VisualBlocks.Count; i++)
                {
                    Canvas.SetTop(VisualBlocks[i], SequenceBlocks[i].TopOffset);
                    Canvas.SetLeft(VisualBlocks[i], SequenceBlocks[i].LeftOffset);

                }
                foreach (TextBlock txb in VisualBlocks)
                {
                    ArrowCanvas.Children.Add(txb);
                }
            }
            
        }
        #endregion // Functions
    }

}
