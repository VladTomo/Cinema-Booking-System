package com.cinema.server.service;

import com.cinema.server.entity.Movie;
import com.cinema.server.repository.MovieRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class MovieService {

    @Autowired
    private MovieRepository movieRepository;

    // Adauga un film
    public Movie saveMovie(Movie movie) {
        return movieRepository.save(movie);
    }

    // Ia toate filmele
    public List<Movie> getAllMovies() {
        return movieRepository.findAll();
    }

    // Sterge un film
    public void deleteMovie(Long id) {
        movieRepository.deleteById(id);
    }

    // --- AUTOCOMPLETE ---
    public List<Movie> getAutocompleteSuggestions(String query) {
        String lowerQuery = query.toLowerCase();
        List<Movie> allMovies = movieRepository.findAll();
        List<Movie> suggestions = new ArrayList<>();

        for (Movie movie : allMovies) {
            String title = movie.getTitle().toLowerCase();

            // Potrivire exacta sau partiala (standard)
            if (title.contains(lowerQuery)) {
                suggestions.add(movie);
            }
            // Potrivire Fuzzy (AI) - daca utilizatorul a scris gresit
            // Acceptam o "distanta" de 3 greseli (litere lipsa, gresite sau in plus)
            else if (calculateLevenshteinDistance(title, lowerQuery) <= 3) {
                suggestions.add(movie);
            }
        }

        // Returnam maxim 5 sugestii
        return suggestions.stream().limit(5).collect(Collectors.toList());
    }

    // Algoritmul Levenshtein (Matematica din spate)
    // Calculeaza cate modificari trebuie sa faci ca sa ajungi de la cuvantul A la cuvantul B
    private int calculateLevenshteinDistance(String x, String y) {
        int[][] dp = new int[x.length() + 1][y.length() + 1];

        for (int i = 0; i <= x.length(); i++) {
            for (int j = 0; j <= y.length(); j++) {
                if (i == 0) {
                    dp[i][j] = j;
                } else if (j == 0) {
                    dp[i][j] = i;
                } else {
                    dp[i][j] = min(dp[i - 1][j - 1]
                                    + (x.charAt(i - 1) == y.charAt(j - 1) ? 0 : 1),
                            dp[i - 1][j] + 1,
                            dp[i][j - 1] + 1);
                }
            }
        }
        return dp[x.length()][y.length()];
    }

    private int min(int... numbers) {
        int min = Integer.MAX_VALUE;
        for (int num : numbers) {
            if (num < min) min = num;
        }
        return min;
    }
}