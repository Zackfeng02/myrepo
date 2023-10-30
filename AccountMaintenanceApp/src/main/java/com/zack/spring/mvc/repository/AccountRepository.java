package com.zack.spring.mvc.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import com.zack.spring.mvc.entity.Account;

public interface AccountRepository extends JpaRepository<Account, Long> {
    // Read: Custom query to find accounts by account type
    @Query("SELECT a FROM Account a WHERE a.accountTypeCode = :accountTypeCode")
    List<Account> findByAccountType(@Param("accountTypeCode") String accountTypeCode);

    // Read: Custom query to find accounts with overdraft limit enabled
    @Query("SELECT a FROM Account a WHERE a.overDraftLimit > 0")
    List<Account> findWithOverDraftLimit();
    
    // Read: list all accounts for customer id
    @Query("SELECT a FROM Account a WHERE a.customer.customer_id = :customerId")
    List<Account> findByCustomerId(@Param("customerId") Long customerId);
    

    // Delete: Custom query to delete accounts by account type
    @Transactional
    @Modifying
    @Query("DELETE FROM Account a WHERE a.accountTypeCode = :accountTypeCode")
    void deleteByAccountTypeCode(@Param("accountTypeCode") String accountTypeCode);
    
 // New query to delete account by account number
    @Transactional
    @Modifying
    @Query("DELETE FROM Account a WHERE a.accountNumber = :accountNumber")
    void deleteByAccountNumber(@Param("accountNumber") Integer accountNumber);
}
