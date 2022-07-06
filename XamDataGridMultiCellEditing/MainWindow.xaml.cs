using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;

namespace XamDataGridMultiCellEditing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Cell> _cells = new List<Cell>();

        public MainWindow()
        {
            InitializeComponent();
            _xamDataGrid.DataSource = Person.GenerateSampleData();
        }

        private void XamDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            XamDataGrid grid = sender as XamDataGrid;
            if (e.Key == Key.F2)
            {
                foreach (var item in grid.SelectedItems.Cells)
                {
                    _cells.Add(item);
                }
            }
        }

        private void XamDataGrid_EditModeStarting(object sender, Infragistics.Windows.DataPresenter.Events.EditModeStartingEventArgs e)
        {
            foreach (var cell in _cells)
            {
                cell.IsSelected = true;
            }
        }

        private void XamDataGrid_EditModeEnding(object sender, Infragistics.Windows.DataPresenter.Events.EditModeEndingEventArgs e)
        {
            if (_cells.Count > 0)
            {
                foreach (var cell in _cells)
                {
                    cell.Record.DataItem.GetType().GetProperty(cell.Field.Name).SetValue(cell.Record.DataItem, e.Editor.Value, null);
                }
                _cells.Clear();
            }
        }
    }

    public class Person : INotifyPropertyChanged
    {
        private string _firstname;
        public string FirstName
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public static ObservableCollection<Person> GenerateSampleData()
        {
            ObservableCollection<Person> _people = new ObservableCollection<Person>();

            for (int i = 0; i < 25; i++)
            {
                _people.Add(new Person() { FirstName = String.Format("First {0}", i), LastName = String.Format("Last {0}", i), Age = i * 2 });
            }

            return _people;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
