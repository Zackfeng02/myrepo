package controller;

import entity.Transaction;
import service.TransactionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/transactions")
public class TransactionController {

    @Autowired
    private TransactionService transactionService;

    @GetMapping
    public List<Transaction> getAllTransactions() {
        return transactionService.findAllTransactions();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Transaction> getTransactionById(@PathVariable Integer id) {
        return transactionService.findTransactionById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public Transaction createTransaction(@RequestBody Transaction transaction) {
        return transactionService.saveTransaction(transaction);
    }

    @PutMapping("/{id}")
    public ResponseEntity<Transaction> updateTransaction(@PathVariable Integer id, @RequestBody Transaction transactionDetails) {
        return transactionService.findTransactionById(id)
                .map(transaction -> {
                    // Update fields
                    return ResponseEntity.ok(transactionService.saveTransaction(transaction));
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteTransaction(@PathVariable Integer id) {
        if (!transactionService.findTransactionById(id).isPresent()) {
            return ResponseEntity.notFound().build();
        }
        transactionService.deleteTransaction(id);
        return ResponseEntity.ok().build();
    }
}

