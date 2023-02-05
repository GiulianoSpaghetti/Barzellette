using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Barzellette
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OleDbConnection dbConnection;
        private Random r;
        private DataSet myDataSet;
        private OleDbCommand command;
        private OleDbDataReader myReader;
        public MainWindow()
        {
            InitializeComponent();
            string[] args=Environment.GetCommandLineArgs();
            if (args.Length != 2) {
                MessageBox.Show(this.FindResource("PathNonValida") as string, this.FindResource("Error") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            dbConnection = new OleDbConnection { ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={args[1]}" };
            try
            {
                dbConnection.Open();
            } catch (System.Data.OleDb.OleDbException ex)
            {
                MessageBox.Show(ex.Message, this.FindResource("Error") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();

            }
            r=new Random();
            Closing += MainWindow_Closing;
            mnFile.Header=this.FindResource("File") as string;
            mnEsci.Header = this.FindResource("Exit") as string;
            mnPI.Header = this.FindResource("?") as string;
            mnInfo.Header = this.FindResource("Informations") as string;
            btnIndietro.Content = this.FindResource("Back") as string;
            btnVisualizza.Content = this.FindResource("Show") as string;
            btnAvanti.Content = this.FindResource("Forward") as string;
            btnRandom.Content = this.FindResource("Random") as string;
        }

        void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            dbConnection.Close();
        }

        private void btn_Clicked(object sender, RoutedEventArgs e)
        {
            ulong id = 0;
            try
            {
                 id = ulong.Parse(txtid.Text);
            } catch (System.FormatException)
            {
                wbbarzelletta.NavigateToString(this.FindResource("IDNotInteger") as string);
                return;
            }
            myDataSet = new DataSet();
            var myAdapptor = new OleDbDataAdapter();
            command = new OleDbCommand($"SELECT Testo FROM Barzellette WHERE ID={id}", dbConnection);
            try
            {
                myReader = command.ExecuteReader();
            } catch (System.Data.OleDb.OleDbException ex)
            {
                wbbarzelletta.NavigateToString(ex.Message);
                btnVisualizza.IsEnabled = false;
                btnAvanti.IsEnabled = false;
                btnIndietro.IsEnabled = false;
                btnRandom.IsEnabled = false;
                return;
            }
            if (myReader.HasRows)
            {
                myReader.Read();
                String s=myReader.GetString(0);
                s = s.Replace("à", "&agrave;"); 
                s = s.Replace("è", "&egrave:"); 
                s = s.Replace("é", "&eacute;"); 
                s = s.Replace("ì", "&igrave;"); 
                s = s.Replace("ò", "&ograve;"); 
                s = s.Replace("ù", "&ugrave;"); 
                wbbarzelletta.NavigateToString(s);
            } else
            {
                wbbarzelletta.NavigateToString(this.FindResource("IDNotFound") as string);
            }
            myReader.Close();
        }

        private void btnIndietro_Clicked(object sender, RoutedEventArgs e)
        {
            ulong id;
            wbbarzelletta.NavigateToString("&nbsp;");
            try
            {
                id = ulong.Parse(txtid.Text);
            }
            catch (System.FormatException)
            {
                wbbarzelletta.NavigateToString(this.FindResource("IDNotInteger") as string);
                return;
            }
            txtid.Text = $"{--id}";
        }

        private void btnAvanti_Clicked(object sender, RoutedEventArgs e)
        {
            ulong id;
            wbbarzelletta.NavigateToString("&nbsp;");
            try
            {
                id = ulong.Parse(txtid.Text);
            }
            catch (System.FormatException)
            {
                wbbarzelletta.NavigateToString(this.FindResource("IDNotInteger") as string);
                return;
            }
            txtid.Text = $"{++id}";

        }

        private void btnRandom_Clicked(object sender, RoutedEventArgs e)
        {
            myDataSet = new DataSet();
            var myAdapptor = new OleDbDataAdapter();
            command = new OleDbCommand($"Select top 1 ID as MaxId FROM Barzellette order by ID desc ", dbConnection);
            try
            {
                myReader = command.ExecuteReader();
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                wbbarzelletta.NavigateToString(ex.Message);
                btnVisualizza.IsEnabled = false;
                btnAvanti.IsEnabled = false;
                btnIndietro.IsEnabled = false;
                btnRandom.IsEnabled = false;
                return;
            }
            if (myReader.HasRows)
            {
                myReader.Read();
                int id = myReader.GetInt32(0);
                txtid.Text = $"{r.Next(1,id)}";
            }
            else
            {
                wbbarzelletta.NavigateToString(this.FindResource("IDNotFound") as string);
            }
            myReader.Close();
        }

        private void mnExit_click(object sender, RoutedEventArgs e)
        {
             Application.Current.Shutdown();
        }
        private void mnInfo_click(object sender, RoutedEventArgs e)
        {
            wbbarzelletta.NavigateToString("<b>Autore</b>: Giulio Sorrentino &copy; 2023<br /><b>Versione</b>: 0.1<br />Un semplice fortune personale basato su access<br /><b>Licenza</b>: GPL 3.0 o, secondo la tua opinione, qualsiasi versione successiva.");
        }
    }
}
