package Entity;

import jakarta.persistence.*;

import java.util.Date;

@Entity
@Table(name = "Transaction")
public class Transaction {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer transactionId;

    @Column
    private Integer customerId;

    @Column
    private Integer bookId;

    @Column
    private Date trxnDate;

    @Column
    @Enumerated(EnumType.STRING)
    private TrxnType trxnType;

    @ManyToOne
    @JoinColumn(name = "CustomerId", insertable = false, updatable = false)
    private Customer customer;

    @ManyToOne
    @JoinColumn(name = "BookId", insertable = false, updatable = false)
    private Book book;

    // Enum for transaction types
    public enum TrxnType {
        Check_in, Check_out
    }

    // Getters and setters
    public Integer getTransactionId() { return transactionId; }

    public void setTransactionId(Integer transactionId) { this.transactionId = transactionId; }

    public Integer getCustomerId() { return customerId; }

    public void setCustomerId(Integer customerId) { this.customerId = customerId; }

    public Integer getBookId() { return bookId; }

    public void setBookId(Integer bookId) { this.bookId = bookId; }

    public Date getTrxnDate() { return trxnDate; }

    public void setTrxnDate(Date trxnDate) { this.trxnDate = trxnDate; }

    public TrxnType getTrxnType() { return trxnType; }

    public void setTrxnType(TrxnType trxnType) { this.trxnType = trxnType; }

    public Customer getCustomer() { return customer; }

    public void setCustomer(Customer customer) { this.customer = customer; }

    public Book getBook() { return book; }

    public void setBook(Book book) { this.book = book; }
}

