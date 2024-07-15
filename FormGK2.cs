using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTGK
{
    public partial class FormGK2 : Form
    {
  
        public FormGK2()
        {
            InitializeComponent();
            rb_all.Checked = true;
            cb_CTDT.Enabled = false;
            LoadAllStudents();


        }

        private void rb_CTDT_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_CTDT.Checked)
            {
                cb_CTDT.Enabled = true;
                LoadComboBox();
            }
            else
            {
                cb_CTDT.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentsByCTDT(cb_CTDT.SelectedValue.ToString());
        }
        private void LoadComboBox()
        {
            string connectionString = @"Data Source=DESKTOP-3FLNI2M;Initial Catalog=KTGK;Integrated Security=True";
            string query = "SELECT sMaCTDT FROM tblCTDT";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    cb_CTDT.DataSource = dataTable;
                    cb_CTDT.DisplayMember = "sMaCTDT";
                    cb_CTDT.ValueMember = "sMaCTDT";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LoadAllStudents()
        {
            string connectionString = @"Data Source=DESKTOP-3FLNI2M;Initial Catalog=KTGK;Integrated Security=True";
            string query = @"
               SELECT 
    tblSINHVIEN.sMaSV, 
    sHoTen, 
    dNgaySinh, 
    tblMONHOC.sTenHocPhan AS MonHoc, 
    (0.3 * fDiemQT + 0.7 * fDiemCK) AS DiemMonHoc
FROM tblSINHVIEN
JOIN tblDIEM ON tblSINHVIEN.sMaSV = tblDIEM.sMaSV
JOIN tblMONHOC ON tblDIEM.sMaHocPhan = tblMONHOC.sMaHocPhan";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadStudentsByCTDT(string maHocPhan)
        {
            string connectionString = @"Data Source=DESKTOP-3FLNI2M;Initial Catalog=KTGK;Integrated Security=True";
            string query = @"
                SELECT 
                    tblSINHVIEN.sMaSV, 
                    tblSINHVIEN.sHoTen, 
                    tblSINHVIEN.dNgaySinh, 
                    tblMONHOC.sTenHocPhan AS MonHoc, 
                    (0.3 * tblDIEM.fDiemQT + 0.7 * tblDIEM.fDiemCK) AS DiemMonHoc
                FROM tblSINHVIEN
                JOIN tblDIEM ON tblSINHVIEN.sMaSV = tblDIEM.sMaSV
                JOIN tblMONHOC ON tblDIEM.sMaHocPhan = tblMONHOC.sMaHocPhan
                JOIN tblCTDT ON tblMONHOC.sMaCTDT = tblCTDT.sMaCTDT
                WHERE tblCTDT.sMaCTDT = @MaHocPhan";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaHocPhan", maHocPhan);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
