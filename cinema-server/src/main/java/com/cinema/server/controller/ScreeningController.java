package com.cinema.server.controller;

import com.cinema.server.entity.Screening;
import com.cinema.server.service.ScreeningService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/screenings")
@CrossOrigin(origins = "*")
public class ScreeningController {

    @Autowired
    private ScreeningService screeningService;

    // GET: Toate programarile
    @GetMapping
    public List<Screening> getAllScreenings() {
        return screeningService.getAllScreenings();
    }

    // GET: Programari filtrate dupa film (Custom Query)
    @GetMapping("/movie/{movieId}")
    public List<Screening> getScreeningsByMovie(@PathVariable Long movieId) {
        return screeningService.getScreeningsByMovieId(movieId);
    }

    // POST: Adauga programare (Admin)
    @PostMapping
    public Screening addScreening(@RequestBody Screening screening) {
        return screeningService.saveScreening(screening);
    }
}