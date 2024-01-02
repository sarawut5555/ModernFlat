using Google.Protobuf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;

namespace ModernFlat.Forms
{
    public partial class FormProduct : Form
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string user;
        private string password;
        private string port;
        private string connectionString;
        private string sslM;
        public FormProduct()
        {
            InitializeComponent();

            server = "localhost";
            database = "moderndb";
            user = "root";
            password = "";
            port = "3306";
            sslM = "none";
            connectionString = String.Format("server={0}; port={1}; userid={2}; password={3}; database={4}; SslMode={5}",
                                     server, port, user, password, database, sslM);

            connection = new MySqlConnection(connectionString);

        }
        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(System.Windows.Forms.Button))
                {
                    System.Windows.Forms.Button btn = (System.Windows.Forms.Button)btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
            label4.ForeColor = ThemeColor.SecondaryColor;
            label5.ForeColor = ThemeColor.PrimaryColor;
        }
        private void LoadtoListview()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM products";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            listView1.Items.Clear();

                            while (dataReader.Read())
                            {
                                ListViewItem item = new ListViewItem(dataReader["P_ID"].ToString());
                                item.SubItems.Add(dataReader["P_Name"].ToString());
                                item.SubItems.Add(dataReader["P_Descript"].ToString());
                                item.SubItems.Add(dataReader["Price"].ToString());
                                item.SubItems.Add(dataReader["SS"].ToString());
                                item.SubItems.Add(dataReader["QiS"].ToString());
                                item.SubItems.Add(dataReader["UoM"].ToString());
                                item.SubItems.Add(dataReader["PC"].ToString());
                                item.SubItems.Add(dataReader["Brand"].ToString());
                                item.SubItems.Add(dataReader["Manufacturer"].ToString());
                                item.SubItems.Add(dataReader["DateAdded"].ToString());
                                item.SubItems.Add(dataReader["AStatus"].ToString());
                                item.SubItems.Add(dataReader["Image"].ToString());
                                listView1.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }

            }
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            LoadTheme();
            listView1.Columns.Add("Id", 80);
            listView1.Columns.Add("Name", 80);
            listView1.Columns.Add("Descript", 80);
            listView1.Columns.Add("Price", 80);
            listView1.Columns.Add("StockStart", 80);
            listView1.Columns.Add("Quantity in Stock", 80);
            listView1.Columns.Add("Unit of Measure", 80);
            listView1.Columns.Add("Product Category", 80);
            listView1.Columns.Add("Brand", 80);
            listView1.Columns.Add("Manfacturer", 80);
            listView1.Columns.Add("Date Added", 80);
            listView1.Columns.Add("Availability Status", 80);
            LoadtoListview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Id = textBox1.Text;
            string Name = textBox2.Text;
            string Description = textBox3.Text;
            string Price = textBox4.Text;
            string Stockstart = textBox5.Text;
            string Quantity = textBox6.Text;
            string Unit = textBox7.Text;
            string Category = comboBox1.Text;
            string Brand = comboBox2.Text;
            string Manufacturer = textBox8.Text;
            string Date = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string Status = radioButton1.Checked ? "Available" : "Not for Sale";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();

                    connection.Open();

                    string query = "INSERT INTO products (P_ID, P_Name, P_Descript, Price, SS, QiS, UoM, PC, Brand, Manufacturer, DateAdded, AStatus, Image) " +
                        "VALUES (@id, @Name, @Description, @Price, @Stockstart, @Quantity, @Unit, @Category, @Brand, @Manufacturer, @Date, @Status, @img)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", Id);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.Parameters.AddWithValue("@Stockstart", Stockstart);
                        cmd.Parameters.AddWithValue("@Quantity", Quantity);
                        cmd.Parameters.AddWithValue("@Unit", Unit);
                        cmd.Parameters.AddWithValue("@Category", Category);
                        cmd.Parameters.AddWithValue("@Brand", Brand);

                        cmd.Parameters.Add("@img", MySqlDbType.Blob);
                        cmd.Parameters["@img"].Value = img;


                        cmd.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        // Assuming AStatus is a boolean field based on radio button selection
                        cmd.Parameters.AddWithValue("@Status", Status);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data added to the database.");
                        LoadtoListview();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                textBox3.Text = String.Empty;
                textBox4.Text = String.Empty;
                textBox5.Text = String.Empty;
                textBox6.Text = String.Empty;
                textBox7.Text = String.Empty;
                comboBox1.Text = String.Empty;
                comboBox2.Text = String.Empty;
                textBox8.Text = String.Empty;
                dateTimePicker1.Text = String.Empty;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                pictureBox1.Image = null;

            }


        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
                textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox3.Text = listView1.SelectedItems[0].SubItems[2].Text;
                textBox4.Text = listView1.SelectedItems[0].SubItems[3].Text;
                textBox5.Text = listView1.SelectedItems[0].SubItems[4].Text;
                textBox6.Text = listView1.SelectedItems[0].SubItems[5].Text;
                textBox7.Text = listView1.SelectedItems[0].SubItems[6].Text;
                comboBox1.Text = listView1.SelectedItems[0].SubItems[7].Text;
                comboBox2.Text = listView1.SelectedItems[0].SubItems[8].Text;
                textBox8.Text = listView1.SelectedItems[0].SubItems[9].Text;

                // Extract and display the date from the DateAdded column (assuming it's the 10th column)
                string dateString = listView1.SelectedItems[0].SubItems[10].Text;
                DateTime dateAdded;
                if (DateTime.TryParse(dateString, out dateAdded))
                {
                    dateTimePicker1.Value = dateAdded;
                }
                else
                {
                    MessageBox.Show("Invalid date format");
                }

                // Extract and display the status from the AStatus column (assuming it's the 11th column)
                string statusString = listView1.SelectedItems[0].SubItems[11].Text;
                radioButton1.Checked = (statusString == "Available");
                radioButton2.Checked = (statusString == "Not for Sale");

                // Extract and display the image path (assuming it's the 12th column)
                string imagePath = listView1.SelectedItems[0].SubItems[12].Text;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    try
                    {
                        // Assuming you have a MySqlConnection named 'connection'
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // Assuming your table name is 'products' and the image column is 'Image'
                            string query = "SELECT Image FROM products WHERE Image = @img";
                            using (MySqlCommand cmd = new MySqlCommand(query, connection))
                            {
                                cmd.Parameters.AddWithValue("@img", imagePath);

                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        byte[] imgData = (byte[])reader["Image"];
                                        using (MemoryStream ms = new MemoryStream(imgData))
                                        {
                                            pictureBox1.Image = Image.FromStream(ms);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception, for example, show an error message
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Handle the case where the image path is empty or null
                    pictureBox1.Image = null;
                }


            }
            LoadtoListview();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png; *.jpg)|*.png;*.jpg|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Id = textBox1.Text;
            string Name = textBox2.Text;
            string Description = textBox3.Text;
            string Price = textBox4.Text;
            string Stockstart = textBox5.Text;
            string Quantity = textBox6.Text;
            string Unit = textBox7.Text;
            string Category = comboBox1.Text;
            string Brand = comboBox2.Text;
            string Manufacturer = textBox8.Text;
            string Date = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string Status = radioButton1.Checked ? "Available" : "Not for Sale";
            string Img = pictureBox1.ImageLocation;


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();

                    connection.Open();

                    string query = "UPDATE products SET P_Name = @Name, P_Descript = @Description, Price = @Price, SS = @StockStart, QiS = @Quantity, UoM = @Unit, PC = @Category, Brand = @Brand, Manufacturer = @Manufacturer, DateAdded = @Date, AStatus = @Status, Image = @img WHERE P_ID = @Id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.Parameters.AddWithValue("@Stockstart", Stockstart);
                        cmd.Parameters.AddWithValue("@Quantity", Quantity);
                        cmd.Parameters.AddWithValue("@Unit", Unit);
                        cmd.Parameters.AddWithValue("@Category", Category);
                        cmd.Parameters.AddWithValue("@Brand", Brand);
                        cmd.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.Add("@img", MySqlDbType.Blob);
                        cmd.Parameters["@img"].Value = img;
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("ข้อมูลถูกแก้ไขแล้ว");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด " + ex.Message);
                }
            }

            LoadtoListview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM products";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        StringBuilder Bulabel = new StringBuilder();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด" + ex.Message);
                }
            }
            LoadtoListview();
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;
            textBox4.Text = String.Empty;
            textBox5.Text = String.Empty;
            textBox6.Text = String.Empty;
            textBox7.Text = String.Empty;
            comboBox1.Text = String.Empty;
            comboBox2.Text = String.Empty;
            textBox8.Text = String.Empty;
            dateTimePicker1.Text = String.Empty;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            pictureBox1.Image = null;

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int day = dateTimePicker1.Value.Day;
            int month = dateTimePicker1.Value.Month;
            int year = dateTimePicker1.Value.Year;

            DateTime selectedDate = new DateTime(year, month, day);

            Console.WriteLine(selectedDate);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text; 

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM products WHERE P_ID = @Id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("Id", id);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("ข้อมูลถูกลบแล้ว");
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        StringBuilder Bulabel = new StringBuilder();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด " + ex.Message);
                }
            }
            LoadtoListview();
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;
            textBox4.Text = String.Empty;
            textBox5.Text = String.Empty;
            textBox6.Text = String.Empty;
            textBox7.Text = String.Empty;
            comboBox1.Text = String.Empty;
            comboBox2.Text = String.Empty;
            textBox8.Text = String.Empty;
            dateTimePicker1.Text = String.Empty;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            pictureBox1.Image = null;

        }
    }
}
