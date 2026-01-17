package com.cinema.server.repository;

import com.cinema.server.entity.Reservation;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface ReservationRepository extends JpaRepository<Reservation, Long> {

    // Custom Query: Gaseste toate rezervarile pentru o anumita programare
    List<Reservation> findByScreeningId(Long screeningId);
}