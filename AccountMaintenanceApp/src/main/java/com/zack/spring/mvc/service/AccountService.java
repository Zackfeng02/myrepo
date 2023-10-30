package com.zack.spring.mvc.service;
import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.repository.AccountRepository;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class AccountService {
    @Autowired
    private AccountRepository accountRepository;

    public Account saveAccount(Account account) {
        return accountRepository.save(account);
    }

    public List<Account> getAllAccounts() {
        return accountRepository.findAll();
    }

    public List<Account> getAccountById(Long customer_id) {
        return (List<Account>) accountRepository.findByCustomerId(customer_id);
    }

    public void deleteAccount(Long id) {
        accountRepository.deleteById(id);
    }
}
