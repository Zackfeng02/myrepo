package com.zack.spring.mvc.controller;

import java.security.Principal;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.service.AccountService;
import com.zack.spring.mvc.service.CustomerService;

@Controller
public class IndexController {

    @Autowired
    private CustomerService customerService;
    
    @Autowired
    private AccountService accountService;
	
    @GetMapping("/")
    public String showIndexPage() {
        return "index";
    }
    
    @GetMapping("/login")
    public String showLoginPage() {
        return "index";
    }
    
    @GetMapping("/register")
    public String showRegistrationForm(Model model) {
        Customer customer = new Customer();
        model.addAttribute("customer", customer);
        return "registration-form";
    }
    
    @GetMapping("/cus_overview")
    public String customerOverview(Model model, Principal principal) {
        String username = principal.getName();
        System.out.println("object: " + username);
        
        Customer customer = customerService.findByUsername(username);
        System.out.println("object: " + customer);
        model.addAttribute("customer", customer);
        
        List<Account> accounts = accountService.getAccountById(customer.getCustomerId());
        System.out.println("object: " + accounts);
            // Check if the accounts list is null or empty
            if (accounts != null && !accounts.isEmpty()) {
                model.addAttribute("accounts", accounts);
            } else {
                // Handle the case where accounts are not found
                model.addAttribute("errorMessage", "No accounts found for this customer.");
                }
        
        return "customer-overview";
    }
}
