using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection.Emit;
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

namespace Barzellette
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OleDbConnection dbConnection;
        private Random r;
        public MainWindow()
        {
            InitializeComponent();
            string[] args=Environment.GetCommandLineArgs(); ;
            if (args.Length != 2) {
                MessageBox.Show("Passare la path al file accdb. Il programma si chiude.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            dbConnection = new OleDbConnection { ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={args[1]}" };
            try
            {
                dbConnection.Open();
            } catch (System.Data.OleDb.OleDbException ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();

            }
            r=new Random();
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
        dbConnection.Close();
        }

        private void btn_Clicked(object sender, RoutedEventArgs e)
        {
            ulong id = 0;
            wbbarzelletta.NavigateToString("&nbsp;");
            try
            {
                 id = ulong.Parse(txtid.Text);
            } catch (System.FormatException)
            {
                wbbarzelletta.NavigateToString("Il campo id non &egrave; intero.");
                return;
            }
            DataSet myDataSet = new DataSet();
            var myAdapptor = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand($"SELECT Testo FROM Barzellette WHERE ID={id}", dbConnection);
            OleDbDataReader myReader;
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
                s = s.Replace("à", "&agrave;"); ;
                s = s.Replace("è", "&egrave:"); ;
                s = s.Replace("é", "&eacute;"); ;
                s = s.Replace("ì", "&igrave;"); ;
                s = s.Replace("ò", "&ograve;"); ;
                s = s.Replace("ù", "&ugrave;"); ;
                wbbarzelletta.NavigateToString(s);
            } else
            {
                wbbarzelletta.NavigateToString("L'id selezionato non &egrave; stato trovato");
            }
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
                wbbarzelletta.NavigateToString("Il campo id non &egrave; intero.");
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
                wbbarzelletta.NavigateToString("Il campo id non &egrave; intero.");
                return;
            }
            txtid.Text = $"{++id}";

        }

        private void btnRandom_Clicked(object sender, RoutedEventArgs e)
        {
            DataSet myDataSet = new DataSet();
            var myAdapptor = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand($"Select top 1 ID as MaxId FROM Barzellette order by ID desc ", dbConnection);
            OleDbDataReader myReader;
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
                txtid.Text = $"{r.Next(id)}";
            }
            else
            {
                wbbarzelletta.NavigateToString("L'id selezionato non &egrave; stato trovato");
            }
        }
    }
}
