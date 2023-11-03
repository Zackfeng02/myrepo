package com.zack.spring.mvc.entity;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.ManyToMany;

@Entity
public class Role {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    private String name;
    
    @ManyToMany(mappedBy = "roles", fetch = FetchType.LAZY)
    private Set<Customer> customers = new HashSet<>();
    
    public Integer getId (){
    	return id;
    }
    
    public void setId(Integer id) {
        this.id = id;
    }
    
    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
    
    public Set<Customer> getCustomer (){
    	return customers;
    }
    
    public void setCustomer(Set<Customer> customers) {
    	this.customers = customers;
    }
}

