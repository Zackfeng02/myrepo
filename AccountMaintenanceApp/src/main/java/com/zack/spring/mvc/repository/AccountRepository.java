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
    @Query("SELECT a FROM Account a WHERE a.accountType = :accountType")
    List<Account> findByAccountType(@Param("accountType") String accountType);

    // Read: Custom query to find accounts with overdraft limit enabled
    @Query("SELECT a FROM Account a WHERE a.overDraftLimit > 0")
    List<Account> findWithOverDraftLimit();

    // Delete: Custom query to delete accounts by account type
    @Transactional
    @Modifying
    @Query("DELETE FROM Account a WHERE a.accountType = :accountType")
    void deleteByAccountType(@Param("accountType") String accountType);
}
