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

    // Read: Custom query to find all customers with a specific name
    @Query("SELECT c FROM Customer c WHERE c.name = :name")
    List<Customer> findByName(@Param("name") String name);

    // Delete: Custom query to delete a customer by email
    @Query("DELETE FROM Customer c WHERE c.email = :email")
    void deleteByEmail(@Param("email") String email);
}
