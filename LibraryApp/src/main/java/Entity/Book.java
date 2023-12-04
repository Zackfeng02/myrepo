package Entity;

import jakarta.persistence.*;

@Entity
@Table(name = "Book")
public class Book {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer bookId;

    @Column(length = 255)
    private String title;

    @Column(length = 255)
    private String authorLastName;

    @Column(length = 255)
    private String authorFirstName;

    @Column
    private Integer rating; // 1-5

    // Getters and setters
    public Integer getBookId() { return bookId; }
    public void setBookId(Integer bookId) { this.bookId = bookId; }

    public String getTitle() { return title; }
    public void setTitle(String title) { this.title = title; }

    public String getAuthorLastName() { return authorLastName; }
    public void setAuthorLastName(String authorLastName) { this.authorLastName = authorLastName; }

    public String getAuthorFirstName() { return authorFirstName; }
    public void setAuthorFirstName(String authorFirstName) { this.authorFirstName = authorFirstName; }

    public Integer getRating() { return rating; }
    public void setRating(Integer rating) { this.rating = rating; }
    
}
