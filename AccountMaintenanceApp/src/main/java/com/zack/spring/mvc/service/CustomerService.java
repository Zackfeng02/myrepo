package com.zack.spring.mvc.service;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.repository.CustomerRepository;

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

    public Customer getCustomerById(Integer customer_id) {
        return customerRepository.findById(customer_id);
    }

    public void deleteCustomer(Integer customer_id) {
        customerRepository.deleteById(customer_id);
    }

    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        Customer customer = customerRepository.findByUsername(username);
        if (customer == null) {
            throw new UsernameNotFoundException("User not found");
        }
        return User.withUsername(customer.getUsername())
                   .password(customer.getPassword())
                   .roles("USER")
                   .build();
    }

	public Customer findByUsername(String username) {
		Customer customer = customerRepository.findByUsername(username);
        if (customer == null) {
            throw new UsernameNotFoundException("User not found");
        }
		return customer;
	}
}
