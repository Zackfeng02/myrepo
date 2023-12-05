package service;

import entity.Book;
import java.util.List;
import java.util.Optional;

public interface BookService {
    List<Book> findAllBooks();
    Optional<Book> findBookById(Integer id);
    Book saveBook(Book book);
    void deleteBook(Integer id);
}

