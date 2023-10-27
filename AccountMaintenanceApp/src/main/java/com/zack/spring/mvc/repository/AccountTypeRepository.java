package com.zack.spring.mvc.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import com.zack.spring.mvc.entity.AccountType;

public interface AccountTypeRepository extends JpaRepository<AccountType, Long> {
    // Read: Custom query to find account type by type name
    @Query("SELECT at FROM AccountType at WHERE at.type = :type")
    AccountType findByType(@Param("type") String type);

    // Delete: Custom query to delete account type by type name
    @Query("DELETE FROM AccountType at WHERE at.type = :type")
    void deleteByType(@Param("type") String type);
}
