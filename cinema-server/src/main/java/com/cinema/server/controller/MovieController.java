package com.cinema.server.controller;

import com.cinema.server.entity.Movie;
import com.cinema.server.service.MovieService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/movies")
@CrossOrigin(origins = "*") // Lasa pe oricine sa apeleze (Vue, Windows)
public class MovieController {

    @Autowired
    private MovieService movieService;

    // GET: Toate filmele
    @GetMapping
    public List<Movie> getAllMovies() {
        return movieService.getAllMovies();
    }

    // POST: Adauga film (Admin)
    @PostMapping
    public Movie addMovie(@RequestBody Movie movie) {
        return movieService.saveMovie(movie);
    }

    // DELETE: Sterge film (Admin)
    @DeleteMapping("/{id}")
    public void deleteMovie(@PathVariable Long id) {
        movieService.deleteMovie(id);
    }

    // GET: AI Autocomplete (Custom Query)
    @GetMapping("/autocomplete")
    public List<Movie> autocomplete(@RequestParam String query) {
        return movieService.getAutocompleteSuggestions(query);
    }
}