package com.zack.spring.mvc.entity;

import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;

import jakarta.persistence.*;


@Entity
public class Account {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @NotNull
    private String accountType;

    @Min(0)
    private Double balance;

    @NotNull
    private Boolean overDraftLimit;

    @ManyToOne
    @JoinColumn(name = "customer_id")
    private Customer customer;

    // Getters and Setters
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getAccountType() {
        return accountType;
    }

    public void setAccountType(String accountType) {
        this.accountType = accountType;
    }

    public Double getBalance() {
        return balance;
    }

    public void setBalance(Double balance) {
        this.balance = balance;
    }

    public Boolean getOverDraftLimit() {
        return overDraftLimit;
    }

    public void setOverDraftLimit(Boolean overDraftLimit) {
        this.overDraftLimit = overDraftLimit;
    }

    public Customer getCustomer() {
        return customer;
    }

    public void setCustomer(Customer customer) {
        this.customer = customer;
    }
}
