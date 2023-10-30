package com.zack.spring.mvc.entity;

import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;

import jakarta.persistence.*;


@Entity
public class Account {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "account_number")
    private Integer accountNumber;

    @Column(name = "account_type_code")
    private String accountTypeCode;

    @Min(0)
    @Column(name = "balance")
    private Double balance;

    @NotNull
    @Column(name = "over_draft_limit")
    private Integer overDraftLimit;

    @ManyToOne
    @JoinColumn(name = "customer_id", referencedColumnName = "customer_id")
    private Customer customer;

    // Getters and Setters
    
    public Integer getAccountNumber() {
        return accountNumber;
    }

    public void setAccountNumber(Integer accountNumber) {
        this.accountNumber = accountNumber;
    }

    public String getAccountType() {
        return accountTypeCode;
    }

    public void setAccountType(String accountType) {
        this.accountTypeCode = accountType;
    }

    public Double getBalance() {
        return balance;
    }

    public void setBalance(Double balance) {
        this.balance = balance;
    }

    public Integer getOverDraftLimit() {
        return overDraftLimit;
    }

    public void setOverDraftLimit(Integer overDraftLimit) {
        this.overDraftLimit = overDraftLimit;
    }

    public Customer getCustomer() {
        return customer;
    }

    public void setCustomer(Customer customer) {
        this.customer = customer;
    }
}
