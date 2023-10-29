package com.zack.spring.mvc.controller;

import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.service.UserRegistrationService;

import jakarta.transaction.Transactional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@Controller
@RequestMapping("/register")
public class UserRegistrationController {

	private static final Logger logger = LoggerFactory.getLogger(UserRegistrationController.class);
	
    @Autowired
    private UserRegistrationService userRegistrationService;

    @GetMapping("/")
    public String register(Model model) {
        model.addAttribute("customer", new Customer());
        // Changed the return value to "registration-form"
        return "registration-form";
    }

    @PostMapping("/")
    public String registerCustomer(@ModelAttribute Customer customer) {
    	logger.info("Received customer data: {}", customer);
        userRegistrationService.registerCustomer(customer);
        return "redirect:/login/";
    }
}
