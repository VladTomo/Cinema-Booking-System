package com.cinema.server.service;

import com.cinema.server.entity.Screening;
import com.cinema.server.repository.ScreeningRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class ScreeningService {

    @Autowired
    private ScreeningRepository screeningRepository;

    public List<Screening> getAllScreenings() {
        return screeningRepository.findAll();
    }

    public List<Screening> getScreeningsByMovieId(Long movieId) {
        return screeningRepository.findByMovieId(movieId);
    }

    // Adminul va folosi asta
    public Screening saveScreening(Screening screening) {
        return screeningRepository.save(screening);
    }
}