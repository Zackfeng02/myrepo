package com.zack.spring.mvc.rest;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.zack.spring.mvc.dto.CustomerDto;
import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.exception.CustomerNotFoundException;
import com.zack.spring.mvc.service.AccountService;
import com.zack.spring.mvc.service.CustomerService;

import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/customers")
public class CustomerRestController {

    @Autowired
    private CustomerService customerService;
    
    @Autowired
    private AccountService accountService;

    // Convert Entity to DTO
    private CustomerDto convertToDto(Customer customer) {
        List<Integer> accountIds = customer.getAccounts().stream()
                .map(Account::getAccountNumber)
                .collect(Collectors.toList());

        return new CustomerDto(
            customer.getCustomerId(),
            customer.getUsername(),
            customer.getFirstname(),
            customer.getLastname(),
            customer.getAddress(),
            customer.getCity(),
            customer.getPostalcode(),
            customer.getPhone(),
            customer.getEmailId(),
            accountIds
        );
    }

    // Convert DTO to Entity
    private Customer convertToEntity(CustomerDto dto) {
        Customer customer = new Customer();
        customer.setCustomerId(dto.getCustomerId());
        customer.setUsername(dto.getUsername());
        customer.setFirstname(dto.getFirstname());
        customer.setLastname(dto.getLastname());
        customer.setAddress(dto.getAddress());
        customer.setCity(dto.getCity());
        customer.setPostalcode(dto.getPostalcode());
        customer.setPhone(dto.getPhone_number());
        customer.setEmailId(dto.getEmailId());
        
        // Fetch and set accounts
        if (dto.getAccountIds() != null && !dto.getAccountIds().isEmpty()) {
            List<Account> accounts = dto.getAccountIds().stream()
                                         .map(accountService::findByAccountNumber)
                                         .collect(Collectors.toList());
            customer.setAccounts(accounts);
        }
        
        return customer;
    }

    // Get all customers
    @GetMapping
    public ResponseEntity<List<CustomerDto>> getAllCustomers() {
        List<Customer> customers = customerService.getAllCustomers();
        List<CustomerDto> customerDtos = customers.stream()
                                                  .map(this::convertToDto)
                                                  .collect(Collectors.toList());
        return ResponseEntity.ok(customerDtos);
    }

    // Get customer by ID
    @GetMapping("/{customerId}")
    public ResponseEntity<CustomerDto> getCustomerById(@PathVariable Long customerId) {
        Customer customer = customerService.getCustomerById(customerId);
        if (customer == null) {
            throw new CustomerNotFoundException("Customer with ID " + customerId + " not found");
        }
        CustomerDto customerDto = convertToDto(customer);
        return ResponseEntity.ok(customerDto);
    }

    // Create a new customer
    @PostMapping
    public ResponseEntity<CustomerDto> createCustomer(@RequestBody CustomerDto customerDto) {
        Customer customer = convertToEntity(customerDto);
        Customer savedCustomer = customerService.saveCustomer(customer);
        CustomerDto savedCustomerDto = convertToDto(savedCustomer);
        return ResponseEntity.ok(savedCustomerDto);
    }

    // Update an existing customer
    @PutMapping("/{customerId}")
    public ResponseEntity<CustomerDto> updateCustomer(@PathVariable Long customerId, @RequestBody CustomerDto customerDto) {
        Customer existingCustomer = customerService.getCustomerById(customerId);
        if (existingCustomer == null) {
            throw new CustomerNotFoundException("Customer with ID " + customerId + " not found");
        }
        Customer updatedCustomer = convertToEntity(customerDto);
        updatedCustomer = customerService.saveCustomer(updatedCustomer);
        CustomerDto updatedCustomerDto = convertToDto(updatedCustomer);
        return ResponseEntity.ok(updatedCustomerDto);
    }

    // Delete a customer
    @DeleteMapping("/{customerId}")
    public ResponseEntity<Void> deleteCustomer(@PathVariable Long customerId) {
        Customer customer = customerService.getCustomerById(customerId);
        if (customer == null) {
            throw new CustomerNotFoundException("Customer with ID " + customerId + " not found");
        }
        customerService.deleteCustomer(customerId);
        return ResponseEntity.ok().build();
    }
}
