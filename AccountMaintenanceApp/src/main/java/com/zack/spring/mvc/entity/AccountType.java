package com.zack.spring.mvc.entity;


import javax.validation.constraints.NotBlank;
import jakarta.persistence.*;

@Entity
public class AccountType {
    @Id
    private String accountTypeCode;

    @NotBlank
    private String accountTypeName;
    
    @NotBlank
    private String accountTypeDesc;

    // Getters and Setters
    public String getAccountTypeCode() {
        return accountTypeCode;
    }

    public void setAccountTypeCode(String accountTypeCode) {
        this.accountTypeCode = accountTypeCode;
    }

    public String getAccountTypeName() {
        return accountTypeName;
    }

    public void setAccountTypeName(String accountTypeName) {
        this.accountTypeName = accountTypeName;
    }
    
    public String getAccountTypeDesc() {
        return accountTypeDesc;
    }

    public void setAccountTypeDesc(String accountTypeDesc) {
        this.accountTypeDesc = accountTypeDesc;
    }
}

