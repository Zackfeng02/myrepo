package com.zack.spring.mvc.service;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.repository.CustomerRepository;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;

@Service
public class CustomerService implements UserDetailsService {
    @Autowired
    private CustomerRepository customerRepository;

    public Customer saveCustomer(Customer customer) {
        return customerRepository.save(customer);
    }

    public List<Customer> getAllCustomers() {
        return customerRepository.findAll();
    }

    public Customer getCustomerById(Long id) {
        return customerRepository.findById(id).orElse(null);
    }

    public void deleteCustomer(Long id) {
        customerRepository.deleteById(id);
    }
    
    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        // Fetch the customer from the database using the repository
        Customer customer = customerRepository.findByUsername(username);
        
        // If the customer is not found, throw an exception
        if (customer == null) {
            throw new UsernameNotFoundException("User not found");
        }
        
        // Create a UserDetails object using the Spring Security User class
        return User.withUsername(customer.getUsername())
                   .password(customer.getPassword())
                   .roles("USER")  // You can set roles based on your requirements
                   .build();
    }
}
