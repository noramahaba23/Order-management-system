using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using static WinFormsApp2.Class1;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        private HubConnection _hubConnection;

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5102/classhub")
                .Build();

            _hubConnection.On<string>("ReceiveMessage", async (message) =>
            {
                // Invoke on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Message from SignalR: {message}");
                        LoadDataFromAPI();
                    }));
                }
                else
                {
                    MessageBox.Show($"Message from SignalR: {message}");
                    await LoadDataFromAPI();
                }
            });

            try
            {
                await _hubConnection.StartAsync();

                // Invoke on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (_hubConnection.State == HubConnectionState.Connected)
                        {
                            MessageBox.Show("SignalR connection established successfully.");
                        }
                        else
                        {
                            MessageBox.Show("SignalR connection failed to establish.");
                        }
                    }));
                }
                else
                {
                    if (_hubConnection.State == HubConnectionState.Connected)
                    {
                        MessageBox.Show("SignalR connection established successfully.");
                    }
                    else
                    {
                        MessageBox.Show("SignalR connection failed to establish.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"SignalR Connection Error: {ex.Message}");
                    }));
                }
                else
                {
                    MessageBox.Show($"SignalR Connection Error: {ex.Message}");
                }
            }
        }

        private async Task LoadDataFromAPI()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5102/all");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to a list of products
                    var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responseBody);

                    // Convert the list to a DataTable
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("id");
                    dataTable.Columns.Add("name");
                    dataTable.Columns.Add("price");
                    dataTable.Columns.Add("description");
                    dataTable.Columns.Add("quantity");

                    foreach (var product in products)
                    {
                        dataTable.Rows.Add(product.name, product.id, product.price, product.description, product.quantity);
                    }

                    // Invoke on the UI thread
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            dataGridViewProducts.DataSource = dataTable;
                        }));
                    }
                    else
                    {
                        dataGridViewProducts.DataSource = dataTable;
                    }
                }
                else
                {
                    // Invoke on the UI thread
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show($"Error: {response.StatusCode}");
                        }));
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Invoke on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Request error: {httpEx.Message}");
                    }));
                }
                else
                {
                    MessageBox.Show($"Request error: {httpEx.Message}");
                }
            }
            catch (Exception ex)
            {
                // Invoke on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Unexpected error: {ex.Message}\n\n{ex.StackTrace}");
                    }));
                }
                else
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}\n\n{ex.StackTrace}");
                }
            }
        }


        private async Task<List<Product>> GetProductList()
        {
            string apiUrl = "http://localhost:5102/all";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"API Response: {responseBody}");

                    var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responseBody);

                    return products;
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                    return new List<Product>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Request error: {httpEx.Message}");
                return new List<Product>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                return new List<Product>();
            }
        }






        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            // throw new NotImplementedException();


        }

        public class Product
        {
            public string name { get; set; }
            public int id { get; set; }
            public double price { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            // private void btnViewData_Click(object sender, EventArgs e)
            //{
            //  List<Product__> products = GetProductList();
            // dataGridViewProducts.DataSource = products;
            // }

        }


        private async void button3_Click(object sender, EventArgs e)
        {
            var products = await GetProductList(); // ������� ������ ���� ��������

            // ����� ����� �������� ��� DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("price");
            dataTable.Columns.Add("description");
            dataTable.Columns.Add("quantity");

            foreach (var product in products)
            {
                dataTable.Rows.Add(product.name, product.id, product.price, product.description, product.quantity);
            }

            dataGridViewProducts.DataSource = dataTable;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadDataFromAPI();



        }

        private async void button1_Click(object sender, EventArgs e)
        

            {
                string name = textBox1.Text;
                string price = textBox3.Text;
                string description = textBox4.Text;
                string quantity = textBox5.Text;

                // call the API 
                var data = new
                {
                    name,
                    price,
                    description,
                    quantity
                };

                string apiUrl = "http://localhost:5102/send";
                string jsonData = System.Text.Json.JsonSerializer.Serialize(data);
                StringContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        // Display the response in a message box
                        MessageBox.Show($"Response from API: {responseBody}");
                    }
                    else
                    {
                        // Handle unsuccessful status codes
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error: {response.StatusCode}\n{errorContent}");
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    // Handle network errors or invalid requests
                    MessageBox.Show($"Request error: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    // Handle any other unexpected errors
                    MessageBox.Show($"Unexpected error: {ex.Message}");
                }
            

        }
    }
}

    
