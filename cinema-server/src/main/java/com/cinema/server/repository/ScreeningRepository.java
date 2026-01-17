package com.cinema.server.repository;

import com.cinema.server.entity.Screening;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface ScreeningRepository extends JpaRepository<Screening, Long> {

    // Custom Query: Gaseste toate programarile unui anumit film (dupa ID-ul filmului)
    List<Screening> findByMovieId(Long movieId);
}