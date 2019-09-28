using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace population_data
{
    class VM : INotifyPropertyChanged
    {
        private const int FIRST_YEAR = 1950;
        private const int LAST_YEAR = 1990;
        private Dictionary<int, int> years = new Dictionary<int, int>();
        List<KeyValuePair<int, int>> dataList = new List<KeyValuePair<int, int>>();
        private Dictionary<int, int> diffs = new Dictionary<int, int>();

        private Visibility readBtnVisiblility = Visibility.Visible;
        public Visibility ReadBtnVisiblility { get { return readBtnVisiblility; } set { readBtnVisiblility = value; onChange(); } }

        private Visibility analyseVisiblility = Visibility.Hidden;
        public Visibility AnalyseVisiblility { get { return analyseVisiblility; } set { analyseVisiblility = value; onChange(); } }

        private Visibility otherVisiblility = Visibility.Hidden;
        public Visibility OtherVisiblility { get { return otherVisiblility; } set { otherVisiblility = value; onChange(); } }

        private int avg;
        public int Avg { get { return avg; } set { avg = value; onChange(); } }

        private double great;
        public double Great { get { return great; } set { great = value; onChange(); } }

        private double least;
        public double Least { get { return least; } set { least = value; onChange(); } }

        public BindingList<string> PopulationList { get; set; } = new BindingList<string>();

        private List<KeyValuePair<int, int>> chart;
        public List<KeyValuePair<int, int>> Chart { get { return chart; } set { chart = value; onChange(); } }

        public VM()
        {
            readFile();
            calcDiff();
        }

        private void readFile()
        {
            string[] lines = File.ReadAllLines("USPopulation.txt");
            if (lines.Length != LAST_YEAR - FIRST_YEAR + 1)
            {
                MessageBox.Show("Wrong number of lines in input file");
                Application.Current.Shutdown();
            }
            int year = FIRST_YEAR;
            foreach (string line in lines)
            {
                try
                {
                    years.Add(year, int.Parse(line));
                    dataList.Add(new KeyValuePair<int, int>(year, int.Parse(line)));
                    PopulationList.Add("The population in " + year + " is " + line);
                }
                catch (Exception)
                {
                    MessageBox.Show("Incorrect data in input file");
                    Application.Current.Shutdown();
                }
                year++;
            }
        }

        public void Read()
        {
            ReadBtnVisiblility = Visibility.Hidden;
            AnalyseVisiblility = Visibility.Visible;
        }

        public void Analyse()
        {
            AnalyseVisiblility = Visibility.Hidden;
            OtherVisiblility = Visibility.Visible;
            Avg = getAvgDiff();
            Great = getMaxDiffYear().Key;
            Least = getMinDiffYear().Key;
            Chart = dataList;
        }

        private void calcDiff()
        {
            for (int y = FIRST_YEAR + 1; y <= LAST_YEAR; y++)
            {
                diffs.Add(y, years[y] - years[y - 1]);
            }
        }

        private KeyValuePair<int, int> getMinDiffYear()
        {
            return diffs.OrderBy(pair => pair.Value).First();
        }

        private KeyValuePair<int, int> getMaxDiffYear()
        {
            return diffs.OrderBy(pair => pair.Value).Last();
        }

        private int getAvgDiff()
        {
            return (int)diffs.Values.Average();
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void onChange([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
