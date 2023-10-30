package com.zack.spring.mvc.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.zack.spring.mvc.entity.Customer;
import java.util.List;

public interface CustomerRepository extends JpaRepository<Customer, Long> {
    
	// Read: Custom query to find a customer by email
    @Query("SELECT c FROM Customer c WHERE c.email = :email")
    Customer findByEmail(@Param("email") String email);
    
	// Read: Custom query to find a customer by customer id
    @Query("SELECT c FROM Customer c WHERE c.customer_id = :customer_id")
    Customer findById(@Param("customer_id") Integer customer_id);

    // Read: Custom query to find all customers with a specific name
    @Query("SELECT c FROM Customer c WHERE c.name = :name")
    List<Customer> findByName(@Param("name") String name);

    // Delete: Custom query to delete a customer by email
    @Query("DELETE FROM Customer c WHERE c.email = :email")
    int deleteByEmail(@Param("email") String email);
    
 // Delete: Custom query to delete a customer by email
    @Query("DELETE FROM Customer c WHERE c.customer_id = :customer_id")
    int deleteById(@Param("customer_id") Integer customer_id);
    
    // find username for login
    Customer findByUsername(String username);
}
