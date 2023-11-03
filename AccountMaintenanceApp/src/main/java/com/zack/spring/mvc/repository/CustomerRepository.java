package com.zack.spring.mvc.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.zack.spring.mvc.entity.Customer;
import java.util.List;

public interface CustomerRepository extends JpaRepository<Customer, Long> {
    
	// Read: Custom query to find a customer by email
    @Query("SELECT c FROM Customer c WHERE c.emailId = :emailId")
    Customer findByEmail(@Param("emailId") String emailId);
    
	// Read: Custom query to find a customer by customer id
    @Query("SELECT c FROM Customer c WHERE c.customer_id = :customer_id")
    Customer findByCustomerId(@Param("customer_id") Long customer_id);

    // Read: Custom query to find all customers with a specific first name and last name
    @Query("SELECT c FROM Customer c WHERE c.firstname = :firstname AND c.lastname = :lastname")
    List<Customer> findByName(@Param("firstname") String firstname, @Param("lastname") String lastname);

    // Delete: Custom query to delete a customer by email
    @Query("DELETE FROM Customer c WHERE c.emailId = :emailId")
    int deleteByEmail(@Param("emailId") String emailId);
    
 // Delete: Custom query to delete a customer by customer_id
    @Modifying
    @Query("DELETE FROM Customer c WHERE c.customer_id = :customer_id")
    void deleteByCustomerId(@Param("customer_id") Long customer_id);
    
    // find username for login
    Customer findByUsername(String username);
}
