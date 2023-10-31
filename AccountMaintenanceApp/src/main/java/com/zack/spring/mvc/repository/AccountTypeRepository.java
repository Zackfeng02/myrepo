package com.zack.spring.mvc.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import com.zack.spring.mvc.entity.AccountType;

import jakarta.transaction.Transactional;

public interface AccountTypeRepository extends JpaRepository<AccountType, Long> {
    // Read: Custom query to find account type by type code
    //@Query("SELECT at FROM AccountType at WHERE at.accountTypeCode = :Code")
    //AccountType findByCode(@Param("Code") String Code);

    // Delete: Custom query to delete account type by type name
    //@Modifying
    //@Transactional
    //@Query("DELETE FROM AccountType at WHERE at.accountTypeCode = :Code")
    //void deleteByType(@Param("Code") String Code);



}
