package com.zack.spring.mvc.service;

import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.HashSet;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.entity.Role;
import com.zack.spring.mvc.repository.CustomerRepository;
import com.zack.spring.mvc.repository.RoleRepository;

import jakarta.transaction.Transactional;

@Service
public class UserRegistrationService {

    private final CustomerRepository customerRepository;
    private final RoleRepository roleRepository;
    private final BCryptPasswordEncoder passwordEncoder;
    private static final Logger logger = LoggerFactory.getLogger(UserRegistrationService.class);

    public UserRegistrationService(CustomerRepository customerRepository, BCryptPasswordEncoder passwordEncoder, RoleRepository roleRepository) {
        this.customerRepository = customerRepository;
        this.passwordEncoder = passwordEncoder;
        this.roleRepository = roleRepository;
    }

    @Transactional
    public Customer registerCustomer(Customer customer) {
        
    	logger.info("Registering customer: {}", customer);
    	// Step 1: Validate the customer details (you can add more validations)
        if (customer.getUsername() == null || customer.getPassword() == null) {
            throw new IllegalArgumentException("Username and Password must not be null");
        }

        // Step 2: Check for duplicate username or email
        if (customerRepository.findByUsername(customer.getUsername()) != null) {
            throw new IllegalArgumentException("Username already exists");
        }
        if (customerRepository.findByEmail(customer.getEmailId()) != null) {
            throw new IllegalArgumentException("Email already exists");
        }
        // Step 3: Encrypt the password
        customer.setPassword(passwordEncoder.encode(customer.getPassword()));

        // Step 4: Save the customer to the database
        Role userRole = roleRepository.findByName("USER");
        if (userRole != null) {
            customer.setRoles(new HashSet<>(Arrays.asList(userRole)));
        } else {
            throw new IllegalStateException("USER role not found in the database.");
        }
        
        // Step 5: Save the customer to the database
        return customerRepository.save(customer);
    }
}
