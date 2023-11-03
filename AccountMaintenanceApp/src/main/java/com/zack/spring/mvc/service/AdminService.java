package com.zack.spring.mvc.service;

import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.repository.AdminRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class AdminService {

    @Autowired
    private AdminRepository adminRepository;

    public Map<String, Long> getCountByAccountType() {
        return adminRepository.getCountByAccountType();
    }

    public List<Customer> getCustomersWithHighBalance() {
        return adminRepository.getCustomersWithHighBalance();
    }

    public List<Customer> getCustomersWithOverdraft() {
        return adminRepository.getCustomersWithOverdraft();
    }
}
