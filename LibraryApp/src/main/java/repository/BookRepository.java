package repository;

import org.springframework.data.jpa.repository.JpaRepository;

import entity.Book;

import java.util.List;

public interface BookRepository extends JpaRepository<Book, Integer> {

    // Standard CRUD operations are already provided by JpaRepository

    // Example of a custom query method to find books by title
    List<Book> findByTitleContaining(String title);

    // Example of a custom query method to find books by author's last name
    List<Book> findByAuthorLastName(String authorLastName);

    // Example of a custom query method to find a book by ISBN (if ISBN field is added to Book entity)
    // Optional<Book> findByIsbn(String isbn);

    // Add more custom query methods as required by your application
}

