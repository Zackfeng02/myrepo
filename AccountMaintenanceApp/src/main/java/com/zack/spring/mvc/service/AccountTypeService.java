package com.zack.spring.mvc.service;
import com.zack.spring.mvc.entity.AccountType;
import com.zack.spring.mvc.repository.AccountTypeRepository;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class AccountTypeService {
    @Autowired
    private AccountTypeRepository accountTypeRepository;

    public AccountType saveAccountType(AccountType accountType) {
        return accountTypeRepository.save(accountType);
    }

    public List<AccountType> getAllAccountTypes() {
        return accountTypeRepository.findAll();
    }

    public AccountType getAccountTypeById(Long id) {
        return accountTypeRepository.findById(id).orElse(null);
    }

    public void deleteAccountType(Long id) {
        accountTypeRepository.deleteById(id);
    }
}
