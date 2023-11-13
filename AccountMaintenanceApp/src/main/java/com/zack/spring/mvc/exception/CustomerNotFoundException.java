package com.zack.spring.mvc.exception;

public class CustomerNotFoundException extends RuntimeException {

    private static final long serialVersionUID = 1L; // Add this line

    public CustomerNotFoundException(String message) {
        super(message);
    }
}
