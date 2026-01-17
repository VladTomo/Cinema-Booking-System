package com.cinema.server.controller;

import com.cinema.server.entity.Reservation;
import com.cinema.server.entity.Screening;
import com.cinema.server.repository.ReservationRepository;
import com.cinema.server.repository.ScreeningRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/reservations") // <--- Adresa de baza
public class ReservationController {

    @Autowired private ReservationRepository reservationRepo;
    @Autowired private ScreeningRepository screeningRepo;

    // 1. GET TOATE (Pentru Admin)
    @GetMapping
    public List<Reservation> getAll() { return reservationRepo.findAll(); }

    // 2. GET DUPA SCREENING (Pentru Harta Vue - AICI E PROBLEMA TA DE 404)
    // Daca lipseste linia de mai jos, primesti 404.
    @GetMapping("/screening/{id}")
    public List<Reservation> getByScreening(@PathVariable Long id) {
        return reservationRepo.findByScreeningId(id);
    }

    // 3. POST (Creare rezervare)
    @PostMapping
    public Reservation create(@RequestBody Reservation reservation) {
        // Logica de siguranta pentru ID
        if (reservation.getScreening() != null && reservation.getScreening().getId() != null) {
            Screening s = screeningRepo.findById(reservation.getScreening().getId()).orElse(null);
            reservation.setScreening(s);
        }
        reservation.setStatus("CONFIRMED");
        return reservationRepo.save(reservation);
    }

    // 4. DELETE (Stergere)
    @DeleteMapping("/{id}")
    public void delete(@PathVariable Long id) { reservationRepo.deleteById(id); }
}