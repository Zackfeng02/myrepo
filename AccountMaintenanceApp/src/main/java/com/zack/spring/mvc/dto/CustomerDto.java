package com.zack.spring.mvc.dto;

import java.util.List;

public class CustomerDto {
	
    private Long customerId;
    private String username;
    private String firstname;
    private String lastname;
    private String address;
    private String city;
    private String postalcode;
    private String phone_number;
    private String emailId;
    private List<Integer> accountIds;
    
    public CustomerDto(Long customerId, String username, String firstname, String lastname, String address, String city, String postalcode, String phone_number, String emailId, List<Integer> accountIds) {
    	this.customerId = customerId;
    	this.username = username;
    	this.firstname = firstname;
    	this.lastname = lastname;
    	this.address = address;
    	this.city = city;
    	this.postalcode = postalcode;
    	this.phone_number = phone_number;
    	this.emailId = emailId;
    	this.accountIds = accountIds;
    }
    
	public Long getCustomerId() {
		return customerId;
	}
	public void setCustomerId(Long customerId) {
		this.customerId = customerId;
	}
	public String getUsername() {
		return username;
	}
	public void setUsername(String username) {
		this.username = username;
	}
	public String getFirstname() {
		return firstname;
	}
	public void setFirstname(String firstname) {
		this.firstname = firstname;
	}
	public String getLastname() {
		return lastname;
	}
	public void setLastname(String lastname) {
		this.lastname = lastname;
	}
	public String getAddress() {
		return address;
	}
	public void setAddress(String address) {
		this.address = address;
	}
	public String getCity() {
		return city;
	}
	public void setCity(String city) {
		this.city = city;
	}
	public String getPostalcode() {
		return postalcode;
	}
	public void setPostalcode(String postalcode) {
		this.postalcode = postalcode;
	}
	public String getPhone_number() {
		return phone_number;
	}
	public void setPhone_number(String phone_number) {
		this.phone_number = phone_number;
	}
	public String getEmailId() {
		return emailId;
	}
	public void setEmailId(String emailId) {
		this.emailId = emailId;
	}
	public List<Integer> getAccountIds() {
		return accountIds;
	}
	public void setAccountIds(List<Integer> accountIds) {
		this.accountIds = accountIds;
	}
    
    
}
