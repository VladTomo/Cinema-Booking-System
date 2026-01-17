using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaAdmin
{
    public partial class Form1 : Form
    {
        // --- ELEMENTE UI ---
        private TabControl tabControl;
        
        // Tab 1: Filme
        private DataGridView gridMovies;
        private TextBox txtTitle, txtGenre, txtDuration, txtDescription;
        private Button btnAddMovie, btnDeleteMovie, btnReloadMovies;

        // Tab 2: Rezervari
        private DataGridView gridReservations;
        private Button btnReloadReservations, btnDeleteReservation;

        // Tab 3: Programare (Screenings)
        private DataGridView gridScreenings;
        private Button btnReloadScreenings;
        private ComboBox cmbMovies;
        private DateTimePicker dtpTime; 
        private TextBox txtRoom, txtPrice;
        private Button btnAddScreening;

        // Server
        private const string BASE_URL = "http://localhost:8080/api";
        private readonly HttpClient client = new HttpClient();

        public Form1()
        {
            this.Text = "Cinema Admin - Full Control";
            this.Size = new Size(1000, 800);
            InitializeCustomUI();
        }

        private void InitializeCustomUI()
        {
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);

            // STILURI
            var inputFont = new Font("Segoe UI", 10);
            var textColor = Color.Black;
            var bgColor = Color.White;

            // ================= TAB 1: GESTIUNE FILME =================
            TabPage tabMovies = new TabPage("Gestiune Filme");
            tabControl.TabPages.Add(tabMovies);

            tabMovies.Controls.Add(new Label() { Text = "Adaugă Film Nou:", Location = new Point(20, 10), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) });
            
            // RANDUL 1: Titlu, Gen, Durata
            tabMovies.Controls.Add(new Label() { Text = "Titlu:", Location = new Point(20, 43), AutoSize = true });
            txtTitle = new TextBox() { Location = new Point(70, 40), Width = 200, Font = inputFont, ForeColor = textColor, BackColor = bgColor };
            tabMovies.Controls.Add(txtTitle);

            tabMovies.Controls.Add(new Label() { Text = "Gen:", Location = new Point(290, 43), AutoSize = true });
            txtGenre = new TextBox() { Location = new Point(330, 40), Width = 150, Font = inputFont, ForeColor = textColor, BackColor = bgColor };
            tabMovies.Controls.Add(txtGenre);

            tabMovies.Controls.Add(new Label() { Text = "Min:", Location = new Point(500, 43), AutoSize = true });
            txtDuration = new TextBox() { Location = new Point(540, 40), Width = 60, Font = inputFont, ForeColor = textColor, BackColor = bgColor };
            tabMovies.Controls.Add(txtDuration);

            // RANDUL 2: Descriere
            tabMovies.Controls.Add(new Label() { Text = "Descriere:", Location = new Point(20, 80), AutoSize = true });
            txtDescription = new TextBox() { 
                Location = new Point(100, 80), 
                Width = 500, 
                Height = 60, 
                Multiline = true, 
                ScrollBars = ScrollBars.Vertical, 
                Font = inputFont, ForeColor = textColor, BackColor = bgColor 
            };
            tabMovies.Controls.Add(txtDescription);

            // Butoane
            btnAddMovie = new Button() { Text = "Salvează Film", Location = new Point(620, 80), Width = 120, Height = 60, BackColor = Color.LightGreen };
            btnAddMovie.Click += async (s, e) => await AddMovie();
            tabMovies.Controls.Add(btnAddMovie);

            btnReloadMovies = new Button() { Text = "🔄 Reîncarcă Lista", Location = new Point(20, 160), Width = 150 };
            btnReloadMovies.Click += async (s, e) => await LoadMovies();
            tabMovies.Controls.Add(btnReloadMovies);

            btnDeleteMovie = new Button() { Text = "❌ Șterge Film", Location = new Point(180, 160), Width = 150, BackColor = Color.LightCoral };
            btnDeleteMovie.Click += async (s, e) => await DeleteMovie();
            tabMovies.Controls.Add(btnDeleteMovie);

            gridMovies = new DataGridView() { Location = new Point(20, 200), Size = new Size(940, 500), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            tabMovies.Controls.Add(gridMovies);


            // ================= TAB 2: GESTIUNE REZERVARI =================
            TabPage tabReservations = new TabPage("Rezervări");
            tabControl.TabPages.Add(tabReservations);

            btnReloadReservations = new Button() { Text = "🔄 Toate Rezervările", Location = new Point(20, 20), Width = 150 };
            btnReloadReservations.Click += async (s, e) => await LoadReservations();
            tabReservations.Controls.Add(btnReloadReservations);

            btnDeleteReservation = new Button() { Text = "❌ Șterge Rezervare", Location = new Point(180, 20), Width = 150, BackColor = Color.LightCoral };
            btnDeleteReservation.Click += async (s, e) => await DeleteReservation();
            tabReservations.Controls.Add(btnDeleteReservation);

            gridReservations = new DataGridView() { Location = new Point(20, 70), Size = new Size(940, 600), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            tabReservations.Controls.Add(gridReservations);


            // ================= TAB 3: PROGRAMARE FILME =================
            TabPage tabScreenings = new TabPage("Programare Filme");
            tabControl.TabPages.Add(tabScreenings);

            GroupBox grpAdd = new GroupBox() { Text = "Programează Film Nou", Location = new Point(20, 10), Size = new Size(940, 100) };
            tabScreenings.Controls.Add(grpAdd);

            grpAdd.Controls.Add(new Label() { Text = "Film:", Location = new Point(20, 30), AutoSize = true });
            cmbMovies = new ComboBox() { Location = new Point(60, 28), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Font = inputFont, ForeColor = textColor, BackColor = bgColor };
            grpAdd.Controls.Add(cmbMovies);

            grpAdd.Controls.Add(new Label() { Text = "Data:", Location = new Point(280, 30), AutoSize = true });
            
            // FIX: ShowUpDown = true ca sa poti selecta ora usor
            dtpTime = new DateTimePicker() { 
                Location = new Point(320, 28), 
                Width = 160, 
                Format = DateTimePickerFormat.Custom, 
                CustomFormat = "dd/MM/yyyy HH:mm", 
                Font = inputFont,
                ShowUpDown = true 
            };
            grpAdd.Controls.Add(dtpTime);

            grpAdd.Controls.Add(new Label() { Text = "Sala:", Location = new Point(490, 30), AutoSize = true });
            txtRoom = new TextBox() { Location = new Point(530, 28), Width = 50, Font = inputFont, ForeColor = textColor, BackColor = bgColor };
            grpAdd.Controls.Add(txtRoom);

            grpAdd.Controls.Add(new Label() { Text = "Preț:", Location = new Point(600, 30), AutoSize = true });
            txtPrice = new TextBox() { Location = new Point(640, 28), Width = 50, Font = inputFont, ForeColor = textColor, BackColor = bgColor };
            grpAdd.Controls.Add(txtPrice);

            btnAddScreening = new Button() { Text = "Salvează Programarea", Location = new Point(720, 26), Width = 180, BackColor = Color.LightSkyBlue };
            btnAddScreening.Click += async (s, e) => await AddScreening();
            grpAdd.Controls.Add(btnAddScreening);

            btnReloadScreenings = new Button() { Text = "🔄 Reîncarcă Programul", Location = new Point(20, 120), Width = 180 };
            btnReloadScreenings.Click += async (s, e) => await LoadScreenings();
            tabScreenings.Controls.Add(btnReloadScreenings);

            gridScreenings = new DataGridView() { Location = new Point(20, 160), Size = new Size(940, 500), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            tabScreenings.Controls.Add(gridScreenings);
        }

        // ---------------- LOGICA FILME ----------------
        private async Task LoadMovies()
        {
            try {
                string json = await client.GetStringAsync($"{BASE_URL}/movies");
                var movies = JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                gridMovies.DataSource = movies;

                // Populare Dropdown Programari
                cmbMovies.DataSource = movies;
                cmbMovies.DisplayMember = "Title";
                cmbMovies.ValueMember = "Id";
            } catch (Exception ex) { MessageBox.Show("Eroare server: " + ex.Message); }
        }

        private async Task AddMovie()
        {
            if(string.IsNullOrWhiteSpace(txtTitle.Text)) return;

            var newMovie = new {
                title = txtTitle.Text,
                genre = txtGenre.Text,
                description = txtDescription.Text, // Luam descrierea din casuta mare
                durationMin = int.TryParse(txtDuration.Text, out int d) ? d : 90
            };

            var content = new StringContent(JsonSerializer.Serialize(newMovie), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{BASE_URL}/movies", content);

            if (response.IsSuccessStatusCode) {
                MessageBox.Show("Film adaugat!");
                txtTitle.Clear(); txtGenre.Clear(); txtDuration.Clear(); txtDescription.Clear();
                await LoadMovies();
            } else {
                MessageBox.Show("Eroare server.");
            }
        }

        private async Task DeleteMovie()
        {
            if (gridMovies.SelectedRows.Count == 0) return;
            long id = (long)gridMovies.SelectedRows[0].Cells["Id"].Value;
            if (MessageBox.Show("Sigur stergi filmul? Se vor sterge si programarile lui!", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                await client.DeleteAsync($"{BASE_URL}/movies/{id}");
                await LoadMovies();
            }
        }

        // ---------------- LOGICA REZERVARI ----------------
        private async Task LoadReservations()
        {
            try {
                string json = await client.GetStringAsync($"{BASE_URL}/reservations");
                var list = JsonSerializer.Deserialize<List<Reservation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                gridReservations.DataSource = list;
            } catch (Exception ex) { MessageBox.Show("Eroare server: " + ex.Message); }
        }

        private async Task DeleteReservation()
        {
            if (gridReservations.SelectedRows.Count == 0) return;
            long id = (long)gridReservations.SelectedRows[0].Cells["Id"].Value;
            await client.DeleteAsync($"{BASE_URL}/reservations/{id}");
            await LoadReservations();
        }

        // ---------------- LOGICA PROGRAMARE (Screenings) ----------------
        private async Task LoadScreenings()
        {
            try {
                string json = await client.GetStringAsync($"{BASE_URL}/screenings");
                var rawList = JsonSerializer.Deserialize<List<Screening>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                // Transformare pentru a vedea Numele Filmului in tabel, nu obiectul
                var displayList = rawList.Select(s => new ScreeningViewModel {
                    Id = s.Id,
                    MovieTitle = s.Movie != null ? s.Movie.Title : "Necunoscut",
                    DateAndTime = DateTime.Parse(s.StartTime).ToString("dd/MM/yyyy HH:mm"),
                    Room = s.RoomNumber,
                    Price = s.TicketPrice
                }).ToList();
                gridScreenings.DataSource = displayList;
            } catch (Exception ex) { MessageBox.Show("Eroare incarcare program: " + ex.Message); }
        }

        private async Task AddScreening()
        {
            if (cmbMovies.SelectedItem == null) { MessageBox.Show("Alege un film!"); return; }

            long movieId = (long)cmbMovies.SelectedValue;
            if (!int.TryParse(txtRoom.Text, out int room)) room = 1;
            if (!double.TryParse(txtPrice.Text, out double price)) price = 25.0;

            var screeningData = new {
                movie = new { id = movieId },
                startTime = dtpTime.Value.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                roomNumber = room,
                ticketPrice = price
            };

            var content = new StringContent(JsonSerializer.Serialize(screeningData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{BASE_URL}/screenings", content);

            if (response.IsSuccessStatusCode) {
                MessageBox.Show("Programare adaugata!");
                await LoadScreenings();
            } else {
                MessageBox.Show("Eroare: " + response.StatusCode);
            }
        }
    }

    // --- MODELE DE DATE ---
    public class Movie {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int DurationMin { get; set; }
    }
    public class Reservation {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string SeatNumber { get; set; }
        public string Status { get; set; }
    }
    public class Screening {
        public long Id { get; set; }
        public Movie Movie { get; set; }
        public string StartTime { get; set; }
        public int RoomNumber { get; set; }
        public double TicketPrice { get; set; }
    }
    // Clasa ajutatoare pentru afisare in tabel
    public class ScreeningViewModel {
        public long Id { get; set; }
        public string MovieTitle { get; set; }
        public string DateAndTime { get; set; }
        public int Room { get; set; }
        public double Price { get; set; }
    }
}