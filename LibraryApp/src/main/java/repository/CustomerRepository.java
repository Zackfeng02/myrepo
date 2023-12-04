package repository;

import org.springframework.data.jpa.repository.JpaRepository;
import Entity.Customer;

import java.util.List;
import java.util.Optional;

public interface CustomerRepository extends JpaRepository<Customer, Integer> {

    // Standard CRUD operations are already provided by JpaRepository

    // Example of a custom query method to find a customer by email
    Optional<Customer> findByEmailId(String emailId);

    // Example of a custom query method to find customers by last name
    List<Customer> findByLastName(String lastName);

    // Add more custom query methods as required by your application
}
