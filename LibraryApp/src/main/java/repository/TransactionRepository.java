package repository;

import org.springframework.data.jpa.repository.JpaRepository;

import entity.Transaction;

import java.util.Date;
import java.util.List;

public interface TransactionRepository extends JpaRepository<Transaction, Integer> {

    // Standard CRUD operations are already provided by JpaRepository

    // Example of a custom query method to find transactions by customer ID
    List<Transaction> findByCustomerId(Integer customerId);

    // Example of a custom query method to find transactions by book ID
    List<Transaction> findByBookId(Integer bookId);

    // Example of a custom query method to find transactions within a date range
    List<Transaction> findByTrxnDateBetween(Date startDate, Date endDate);

    // Example of a custom query method to find transactions by transaction type
    List<Transaction> findByTrxnType(Transaction.TrxnType trxnType);

    // Add more custom query methods as required by your application
}
