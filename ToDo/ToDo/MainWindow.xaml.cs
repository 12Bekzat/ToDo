using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Note Note;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            text.Text = String.Empty;
        }

        private void Select(object sender, SelectionChangedEventArgs e)
        {
            var note = new Note();
            if (notes.SelectedItem != null)
            {
                note.Text = (notes.SelectedItem as Note).Text;
                note.IsDo = (notes.SelectedItem as Note).IsDo;
                note.Id = (notes.SelectedItem as Note).Id;
            }
            Note = note;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Task.Run(() => DeleteManager());
            notes.Items.Remove(Note);
        }

        private Task DeleteManager()
        {
            using (var context = new Context())
            {
                if (Note != null)
                {
                    context.Remove(Note);
                    context.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Task.Run(() => AddManager());
            notes.Items.Add(Note);
        }

        private Task AddManager()
        {
            using (var context = new Context())
            {
                if (string.IsNullOrWhiteSpace(text.Text))
                {
                    var note = new Note
                    {
                        Text = text.Text,
                        IsDo = false
                    };
                    context.Add(Note);
                    context.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Task.Run(() => SaveManager());
        }
        private Task SaveManager()
        {
            using (var context = new Context())
            {
                foreach (Note item in notes.Items)
                {
                    context.Update(item);
                    context.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Task.Run(() => RefreshManager());
        }
        private Task RefreshManager()
        {
            notes.Items.Clear();
            using(var context = new Context())
            {
                var items = context.Notes.ToList();
                notes.ItemsSource = items;
            }
            return Task.CompletedTask;
        }
    }
}
