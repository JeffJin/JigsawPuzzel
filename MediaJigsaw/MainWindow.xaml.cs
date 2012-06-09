using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaJigsaw.Models;

namespace MediaJigsaw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JigsawModel _viewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            base.Loaded += delegate(object s, RoutedEventArgs e)
            {
                this._viewModel = JigsawModel.CreateModel(this);
                base.DataContext = this._viewModel;
            };

        }

        // Properties
        public Canvas Canvas
        {
            get
            {
                return this.puzzlePanel;
            }
        }

    }
}
