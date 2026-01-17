<template>
  <div class="container py-4">
    <header class="pb-3 mb-4 border-bottom d-flex justify-content-between align-items-center">
      <div>
        <h1 class="display-5 fw-bold text-primary">Tomovies</h1>
        <p class="fs-5 text-muted">Preturi cinstite pentru filme cinstite.</p>
      </div>
      <button v-if="selectedMovie" class="btn btn-outline-secondary" @click="goBack">← Înapoi la Filme</button>
    </header>

    <div v-if="!selectedMovie">
      <div class="row mb-5">
        <div class="col-md-6 mx-auto position-relative">
          <input
              type="text"
              class="form-control form-control-lg shadow-sm"
              placeholder="🔍 Caută film"
              v-model="searchQuery"
              @input="onSearchInput"
          />
          <ul v-if="suggestions.length > 0" class="list-group position-absolute w-100 shadow mt-1" style="z-index: 1000;">
            <li v-for="movie in suggestions" :key="movie.id" class="list-group-item list-group-item-action cursor-pointer" @click="selectSuggestion(movie)">
              <strong>{{ movie.title }}</strong> <small class="text-muted">({{ movie.genre }})</small>
            </li>
          </ul>
        </div>
      </div>

      <div class="row g-4">
        <div class="col-md-4" v-for="movie in movies" :key="movie.id">
          <div class="card h-100 shadow-sm border-0 movie-card">
            <div class="card-body d-flex flex-column">
              <h5 class="card-title fw-bold">{{ movie.title }}</h5>

              <div>
                <span class="badge bg-secondary mb-2 d-inline-flex align-items-center justify-content-center px-3"
                      style="font-size: 0.9rem; min-width: 80px; height: 30px;">
                  {{ movie.genre }}
                </span>
              </div>

              <p class="card-text text-muted flex-grow-1">{{ movie.description }}</p>
              <div class="d-flex justify-content-between align-items-center mt-3">
                <small class="text-muted">⏱ {{ movie.durationMin }} min</small>
                <button class="btn btn-primary" @click="viewScreenings(movie)">Vezi Program</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else>
      <div class="row mb-4">
        <div class="col-12">
          <div class="card border-0 shadow-sm bg-light">
            <div class="card-body">
              <h2 class="text-primary">{{ selectedMovie.title }}</h2>
              <p class="lead">{{ selectedMovie.description }}</p>
            </div>
          </div>
        </div>
      </div>

      <h4 class="mb-3" v-if="!bookingScreening">📅 Alege o programare:</h4>
      <div class="row g-3" v-if="!bookingScreening">
        <div class="col-12" v-if="screenings.length === 0">
          <div class="alert alert-warning">Nu există programări disponibile momentan pentru acest film.</div>
        </div>
        <div class="col-md-4" v-for="sc in screenings" :key="sc.id">
          <div class="card text-center h-100 border-primary cursor-pointer screening-card" @click="openVisualBooking(sc)">
            <div class="card-body">
              <h4 class="text-primary">{{ formatTime(sc.startTime) }}</h4>
              <p class="mb-0">{{ formatDate(sc.startTime) }}</p>
              <hr>
              <div class="d-flex justify-content-between">
                <span>Sala {{ sc.roomNumber }}</span>
                <span class="fw-bold">{{ sc.ticketPrice }} RON</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-if="bookingScreening" class="row animate__animated animate__fadeIn">
        <div class="col-lg-8">
          <div class="card shadow border-0">
            <div class="card-body text-center bg-dark rounded">

              <div class="screen mb-5">ECRAN CINEMA</div>

              <div class="d-flex justify-content-center gap-4 mb-4 text-white">
                <div class="d-flex align-items-center"><div class="seat-legend available"></div> Liber</div>
                <div class="d-flex align-items-center"><div class="seat-legend selected"></div> Selectat</div>
                <div class="d-flex align-items-center"><div class="seat-legend occupied"></div> Ocupat</div>
              </div>

              <div class="cinema-container">
                <div v-for="row in rows" :key="row" class="d-flex justify-content-center mb-2">
                  <div
                      v-for="seatNum in seatsPerRow"
                      :key="seatNum"
                      class="seat"
                      :class="getSeatStatus(row, seatNum)"
                      @click="toggleSeat(row, seatNum)"
                  >
                    <small>{{ row }}{{ seatNum }}</small>
                  </div>
                </div>
              </div>

            </div>
          </div>
        </div>

        <div class="col-lg-4">
          <div class="card shadow-sm border-0 sticky-top" style="top: 20px;">
            <div class="card-header bg-white fw-bold">🎫 Rezumat Rezervare</div>
            <div class="card-body">
              <p><strong>Film:</strong> {{ selectedMovie.title }}</p>
              <p><strong>Data:</strong> {{ formatDate(bookingScreening.startTime) }}</p>
              <p><strong>Ora:</strong> {{ formatTime(bookingScreening.startTime) }}</p>
              <hr>

              <h6>Locuri selectate:</h6>
              <div v-if="selectedSeats.length === 0" class="text-muted fst-italic">Niciun loc selectat</div>
              <div class="d-flex flex-wrap gap-2 mb-3">
                <span v-for="s in selectedSeats" :key="s" class="badge bg-success">{{ s }}</span>
              </div>

              <div class="d-flex justify-content-between align-items-center mb-3">
                <span class="fs-5">Total:</span>
                <span class="fs-4 fw-bold text-primary">{{ totalPrice }} RON</span>
              </div>

              <div class="mb-3">
                <label class="form-label">Nume Client:</label>
                <input type="text" class="form-control" v-model="customerName" placeholder="Introdu numele tău..." :class="{'is-invalid': showErrors && !customerName}">
              </div>

              <button class="btn btn-success w-100 btn-lg" :disabled="selectedSeats.length === 0" @click="confirmBooking">
                CONFIRMĂ ({{ selectedSeats.length }} bilete)
              </button>
            </div>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      // Date Server
      movies: [],
      suggestions: [],
      screenings: [],

      // Navigare si Selectie
      searchQuery: '',
      selectedMovie: null,
      bookingScreening: null,

      // Configurare Sala
      rows: ['A', 'B', 'C', 'D', 'E', 'F'], // 6 randuri
      seatsPerRow: 8,                       // 8 locuri pe rand
      occupiedSeats: [],                    // Locuri ocupate (de la server)
      selectedSeats: [],                    // Locuri selectate (de utilizator)

      // Formular
      customerName: '',
      showErrors: false
    };
  },
  computed: {
    totalPrice() {
      if (!this.bookingScreening) return 0;
      return this.selectedSeats.length * this.bookingScreening.ticketPrice;
    }
  },
  mounted() {
    this.fetchMovies();
  },
  methods: {
    // --- API CALLS INITIALE ---
    async fetchMovies() {
      try {
        const res = await axios.get('http://localhost:8080/api/movies');
        this.movies = res.data;
      } catch (e) { console.error("Eroare incarcare filme:", e); }
    },

    // --- SEARCH / AUTOCOMPLETE ---
    async onSearchInput() {
      if (this.searchQuery.length < 2) { this.suggestions = []; return; }
      try {
        const res = await axios.get(`http://localhost:8080/api/movies/autocomplete?query=${this.searchQuery}`);
        this.suggestions = res.data;
      } catch (e) { console.error(e); }
    },
    selectSuggestion(movie) {
      this.searchQuery = movie.title;
      this.suggestions = [];
      this.viewScreenings(movie);
    },

    // --- NAVIGARE ---
    async viewScreenings(movie) {
      this.selectedMovie = movie;
      this.bookingScreening = null;
      try {
        const res = await axios.get(`http://localhost:8080/api/screenings/movie/${movie.id}`);
        this.screenings = res.data;
      } catch (e) { console.error(e); }
    },
    goBack() {
      this.selectedMovie = null;
      this.bookingScreening = null;
      this.searchQuery = '';
    },

    // --- LOGICA HARTA SALA ---
    async openVisualBooking(screening) {
      this.bookingScreening = screening;
      this.selectedSeats = [];
      this.customerName = '';
      this.occupiedSeats = [];

      try {
        console.log(`Cerere catre server pentru Screening ID: ${screening.id}`);
        const res = await axios.get(`http://localhost:8080/api/reservations/screening/${screening.id}`);

        console.log("JSON Brut de la server:", res.data);

        this.occupiedSeats = res.data.map(r => {
          // 1. Încercăm toate variantele posibile de nume de câmp
          let seat = r.seatNumber || r.seat_number || r.SeatNumber || r.seat;

          // 2. Dacă am găsit ceva, curățăm textul
          if (seat) {
            return seat.toString().trim().toUpperCase();
          } else {
            return "XX"; // Nu am găsit locul în obiect
          }
        });

        console.log("Lista finală de locuri roșii:", this.occupiedSeats);
      } catch (e) {
        console.error("Eroare la preluarea rezervărilor:", e);
      }
    },

    getSeatStatus(row, num) {
      const seatId = `${row}${num}`; // Genereaza ID unic, ex: "A1"

      if (this.occupiedSeats.includes(seatId)) return 'occupied';
      if (this.selectedSeats.includes(seatId)) return 'selected';
      return 'available';
    },

    toggleSeat(row, num) {
      const seatId = `${row}${num}`;

      // Blocare click daca e ocupat
      if (this.occupiedSeats.includes(seatId)) return;

      if (this.selectedSeats.includes(seatId)) {
        this.selectedSeats = this.selectedSeats.filter(s => s !== seatId); // Deselectare
      } else {
        this.selectedSeats.push(seatId); // Selectare
      }
    },

    // --- TRIMITE REZERVAREA ---
    async confirmBooking() {
      if (!this.customerName) {
        this.showErrors = true;
        alert("Te rog introdu numele clientului!");
        return;
      }

      try {
        // Facem cate un request pentru fiecare loc selectat
        const promises = this.selectedSeats.map(seat => {
          return axios.post('http://localhost:8080/api/reservations', {
            screening: { id: this.bookingScreening.id }, // ID-ul programarii
            customerName: this.customerName,
            seatNumber: seat
          });
        });

        await Promise.all(promises);

        alert(`Rezervare reușită pentru ${this.selectedSeats.length} locuri!`);

        // Refresh la locuri (ca sa apara rosii imediat)
        await this.openVisualBooking(this.bookingScreening);

      } catch (e) {
        alert("Eroare la rezervare. Posibil ca un loc sa fi fost ocupat intre timp.");
        console.error(e);
      }
    },

    // --- FORMATARI DATA/ORA ---
    formatDate(dateStr) {
      return new Date(dateStr).toLocaleDateString('ro-RO', { weekday: 'short', day: 'numeric', month: 'long' });
    },
    formatTime(dateStr) {
      return new Date(dateStr).toLocaleTimeString('ro-RO', { hour: '2-digit', minute:'2-digit' });
    }
  }
};
</script>

<style>
/* --- STILURI GENERALE --- */
.cursor-pointer { cursor: pointer; }
.movie-card:hover { transform: translateY(-5px); transition: 0.3s; }
.screening-card:hover { background-color: #f8f9fa; border-color: #0d6efd; }

/* --- CSS HARTA CINEMA --- */
.screen {
  background: white;
  height: 50px;
  width: 80%;
  margin: 0 auto;
  box-shadow: 0 0 20px rgba(255, 255, 255, 0.5);
  transform: perspective(500px) rotateX(-10deg);
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  color: #333;
  border-radius: 5px;
  letter-spacing: 5px;
}

.cinema-container {
  display: inline-block;
  padding: 20px;
}

.seat {
  width: 40px;
  height: 40px;
  margin: 5px;
  border-radius: 8px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  font-weight: bold;
  transition: all 0.2s;
}

.seat.available {
  background-color: #444; /* Gri Inchis */
  color: #fff;
}
.seat.available:hover {
  background-color: #666;
  transform: scale(1.1);
}

.seat.selected {
  background-color: #28a745; /* Verde */
  color: white;
  box-shadow: 0 0 10px #28a745;
}

/* FORTAM CULOAREA ROSIE PENTRU OCUPAT */
.seat.occupied {
  background-color: #dc3545 !important;
  color: white !important;
  opacity: 0.6;
  cursor: not-allowed;
  border: 1px solid #a00;
}

.seat-legend { width: 20px; height: 20px; border-radius: 4px; margin-right: 8px; }
.seat-legend.available { background-color: #444; }
.seat-legend.selected { background-color: #28a745; }
.seat-legend.occupied { background-color: #dc3545; }
</style>