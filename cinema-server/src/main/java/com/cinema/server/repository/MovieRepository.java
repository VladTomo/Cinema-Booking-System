package com.cinema.server.repository;

import com.cinema.server.entity.Movie;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import java.util.List;

public interface MovieRepository extends JpaRepository<Movie, Long> {

    // Custom Query (Standard JPA): Cauta filme care contin un text (pentru Autocomplete simplu)
    List<Movie> findByTitleContainingIgnoreCase(String title);

    // Cautare exacta daca e nevoie
    Movie findByTitle(String title);
}