package com.zack.spring.mvc.dto;

public class AccountDto {
	
	private Integer accountNumber;
    private String accountTypeCode;
    private Double balance;
    private Integer overDraftLimit;
    private Long customerId;
    
    public AccountDto(Integer accountNumber, String accountTypeCode, Double balance, Integer overDraftLimit, Long customerId) {
        this.accountNumber = accountNumber;
        this.accountTypeCode = accountTypeCode;
        this.balance = balance;
        this.overDraftLimit = overDraftLimit;
        this.customerId = customerId;
    }
    
	public Integer getAccountNumber() {
		return accountNumber;
	}
	public void setAccountNumber(Integer accountNumber) {
		this.accountNumber = accountNumber;
	}
	public String getAccountTypeCode() {
		return accountTypeCode;
	}
	public void setAccountTypeCode(String accountTypeCode) {
		this.accountTypeCode = accountTypeCode;
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
	public Long getCustomerId() {
		return customerId;
	}
	public void setCustomerId(Long customerId) {
		this.customerId = customerId;
	}
    
    
}
