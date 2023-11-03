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
    
    // Account edit
    @GetMapping("/edit/{accountNumber}")
    public String showEditForm(@PathVariable("accountNumber") Integer accountNumber, Model model) {
        // Fetch the account details from the database using the accountService
        Account account = accountService.findByAccountNumber(accountNumber);
        
        // Check if the account exists
        if (account == null) {
            // Handle the case where the account doesn't exist. 
            // This can be redirecting to an error page or the overview page with an error message.
            return "redirect:/cus_overview?error=AccountNotFound";
        }
        
        // Add the account details to the model
        model.addAttribute("account", account);
        
        // Return the view name of the edit form
        return "account-form";  // Assuming the same form is used for both creation and editing
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
    @GetMapping("/delete/{accountNumber}")
    public String deleteAccount(@PathVariable("accountNumber") Integer accountNumber) {
        accountService.deleteAccount(accountNumber);
        return "redirect:/cus_overview";
    }
}


