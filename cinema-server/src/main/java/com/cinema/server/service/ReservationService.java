package com.cinema.server.service;

import com.cinema.server.entity.Reservation;
import com.cinema.server.repository.ReservationRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class ReservationService {

    @Autowired
    private ReservationRepository reservationRepository;

    public Reservation createReservation(Reservation reservation) {
        // Aici putem pune logica: verifica daca locul e liber etc.
        // Momentan o lasam simpla.
        return reservationRepository.save(reservation);
    }

    public List<Reservation> getReservationsForScreening(Long screeningId) {
        return reservationRepository.findByScreeningId(screeningId);
    }

    public void deleteReservation(Long id) {
        reservationRepository.deleteById(id);
    }

    public List<Reservation> getAllReservations() {
        return reservationRepository.findAll();
    }
}