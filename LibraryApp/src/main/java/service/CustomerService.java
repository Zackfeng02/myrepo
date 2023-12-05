package service;

import java.util.List;
import java.util.Optional;

import entity.Customer;

public interface CustomerService {
    List<Customer> findAllCustomers();
    Optional<Customer> findCustomerById(Integer id);
    Customer saveCustomer(Customer customer);
    void deleteCustomer(Integer id);
}

