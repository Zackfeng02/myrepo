package com.zack.spring.mvc.exception;

public class AccountNotFoundException extends RuntimeException {

    private static final long serialVersionUID = 1L; // Add this line

    public AccountNotFoundException(String message) {
        super(message);
    }
}

