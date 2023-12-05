package service;

import entity.Transaction;
import java.util.List;
import java.util.Optional;

public interface TransactionService {
    List<Transaction> findAllTransactions();
    Optional<Transaction> findTransactionById(Integer id);
    Transaction saveTransaction(Transaction transaction);
    void deleteTransaction(Integer id);
}

