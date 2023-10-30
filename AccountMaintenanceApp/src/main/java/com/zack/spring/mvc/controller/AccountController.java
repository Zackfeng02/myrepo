package com.zack.spring.mvc.controller;
import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.service.AccountService;
import com.zack.spring.mvc.service.CustomerService;

import java.security.Principal;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import jakarta.transaction.Transactional;

@Controller
@RequestMapping("/acc_overview")
public class AccountController {
    @Autowired
    private AccountService accountService;
    
    @Autowired
    private CustomerService customerService;

    // Account creation
    @GetMapping("/new")
    public String showCreateForm(Model model) {
        model.addAttribute("account", new Account());
        return "account-form";
    }

    @Transactional
    @PostMapping("/save")
    public String saveAccount(@ModelAttribute("account") Account account, Principal principal) {
        
        // Fetch the logged-in customer's username
        String username = principal.getName();
        
        // Fetch the customer object from the database
        Customer customer = customerService.findByUsername(username);
        
        // Set the customer to the new account
        account.setCustomer(customer);
    	accountService.saveAccount(account);
        return "redirect:/cus_overview";
    }

    // Account delete
    @Transactional
    @GetMapping("/delete/{id}")
    public String deleteAccount(@PathVariable Long id) {
        accountService.deleteAccount(id);
        return "redirect:/cus_overview";
    }
}


