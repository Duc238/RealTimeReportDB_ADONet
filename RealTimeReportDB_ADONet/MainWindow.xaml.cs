using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealTimeReportDB_ADONet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timer;
        private string _connectionString = "Data Source=DESKTOP-460EDHU\\VOTANDUC;Initial Catalog=RealTimeReportDB;Integrated Security=True;";
        public MainWindow()
        {
            InitializeComponent();
            LoadData(); // Initial load
            StartTimer(); // Start real-time updates
        }
        private void StartTimer()
        {
            _timer = new Timer(5000); // 5 seconds
            _timer.Elapsed += (s, e) => Dispatcher.Invoke(LoadData);
            _timer.Start();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT TOP 100 * FROM SalesReport ORDER BY UpdatedAt DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ReportGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

    }
}
