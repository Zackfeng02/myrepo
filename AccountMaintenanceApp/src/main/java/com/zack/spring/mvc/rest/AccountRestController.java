package com.zack.spring.mvc.rest;

import com.zack.spring.mvc.dto.AccountDto;
import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.exception.AccountNotFoundException;
import com.zack.spring.mvc.exception.CustomerNotFoundException;
import com.zack.spring.mvc.service.AccountService;
import com.zack.spring.mvc.service.CustomerService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/accounts")
public class AccountRestController {

    @Autowired
    private AccountService accountService;
    
    @Autowired
    private CustomerService customerService;

    // Convert Entity to DTO
    private AccountDto convertToDto(Account account) {
        return new AccountDto(account.getAccountNumber(), account.getAccountTypeCode(), account.getBalance(), account.getOverDraftLimit(), account.getCustomer().getCustomerId());
    }

    // Convert DTO to Entity
    private Account convertToEntity(AccountDto accountDto) {
        Account account = new Account();
        account.setAccountNumber(accountDto.getAccountNumber());
        account.setAccountTypeCode(accountDto.getAccountTypeCode());
        account.setBalance(accountDto.getBalance());
        account.setOverDraftLimit(accountDto.getOverDraftLimit());
        
        if (accountDto.getCustomerId() != null) {
            Customer customer = customerService.getCustomerById(accountDto.getCustomerId());
            if (customer != null) {
                account.setCustomer(customer);
            } else {
                throw new CustomerNotFoundException("Customer not found with ID: " + accountDto.getCustomerId());
            }
        }
        return account;
    }

    // Get all accounts
    @GetMapping
    public ResponseEntity<List<AccountDto>> getAllAccounts() {
        List<Account> accounts = accountService.getAllAccounts();
        List<AccountDto> accountDtos = accounts.stream().map(this::convertToDto).collect(Collectors.toList());
        return ResponseEntity.ok(accountDtos);
    }

    // Get account by account number
    @GetMapping("/{accountNumber}")
    public ResponseEntity<AccountDto> getAccountByNumber(@PathVariable Integer accountNumber) {
        Account account = accountService.findByAccountNumber(accountNumber);
        if (account == null) {
            throw new AccountNotFoundException("Account with number " + accountNumber + " not found");
        }
        return ResponseEntity.ok(convertToDto(account));
    }

    // Create a new account
    @PostMapping
    public ResponseEntity<AccountDto> createAccount(@RequestBody AccountDto accountDto) {
        Account savedAccount = accountService.saveAccount(convertToEntity(accountDto));
        return ResponseEntity.ok(convertToDto(savedAccount));
    }

    // Update an existing account
    @PutMapping("/{accountNumber}")
    public ResponseEntity<AccountDto> updateAccount(@PathVariable Integer accountNumber, @RequestBody AccountDto accountDto) {
        Account existingAccount = accountService.findByAccountNumber(accountNumber);
        if (existingAccount == null) {
            throw new AccountNotFoundException("Account with number " + accountNumber + " not found");
        }
        Account updatedAccount = accountService.saveAccount(convertToEntity(accountDto));
        return ResponseEntity.ok(convertToDto(updatedAccount));
    }

    // Delete an account
    @DeleteMapping("/{accountNumber}")
    public ResponseEntity<Void> deleteAccount(@PathVariable Integer accountNumber) {
        Account account = accountService.findByAccountNumber(accountNumber);
        if (account == null) {
            throw new AccountNotFoundException("Account with number " + accountNumber + " not found");
        }
        accountService.deleteAccount(accountNumber);
        return ResponseEntity.ok().build();
    }
}
